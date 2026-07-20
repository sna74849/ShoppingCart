# 🛒 ShoppingCart（注文管理サンプルアプリ）

## 📌 概要

本プロジェクトは、ASP.NET Core MVC を用いたショッピングカート・注文管理アプリケーションです。

注文の「参照（Read）」および「登録（Write）」を中心に、以下の設計方針で構築されています。

* ユースケース単位の Service 層
* DB処理を抽象化した Action + Framework パターン
* トランザクション制御の一元化
* DTO / Entity / ViewModel の責務分離
* カート操作の非同期（AJAX）化とCSRF対策の徹底

> 🆕 **本バージョンでの主な変更点**
> * カート画面（`/cart`）の数量変更・削除を Razor の再読み込みではなく **Fetch API（PUT / DELETE）による非同期通信**に変更
> * `IAntiforgery` を利用した **CSRFトークンのヘッダー送信方式**を採用（`X-CSRF-TOKEN`）
> * 在庫引当処理に **`XLOCK, ROWLOCK` による排他制御**を追加し、同時注文時の在庫競合を防止
> * ログイン状態の判定を `TempData` から **`Session`** に統一
> * 在庫不足時の専用エラー画面（`NoStockError.cshtml`）を追加
> * カート操作結果を **Toast通知**でユーザーにフィードバック
> * レスポンスヘッダーで **キャッシュ無効化**（戻る操作での古い画面表示を防止）

---

## 🏗️ アーキテクチャ概要

```
Controller
   ↓
Service（ユースケース単位）
   ↓
Action（処理単位）
   ↓
DatabaseFramework（トランザクション管理）
   ↓
DAO（DBアクセス）
   ↓
DBManager.dll（外部ライブラリ）
```

Controllerから見て、カート画面のみは上記に加えて **ブラウザ側JS（Fetch API）** が
`/cart/items/{janCd}` に対して直接 PUT / DELETE リクエストを送信する構成になっています。

```
Browser (Cart/Index.cshtml)
   ↓ fetch(PUT / DELETE, X-CSRF-TOKEN)
CartController
   ↓
CartService → Action → DatabaseFramework → DAO
```

---

## 📂 ディレクトリ構成

```
ShoppingCart
├── Controllers
├── Models
│   ├── Actions
│   ├── Services
│   ├── Daos
│   ├── DatabaseFrameworks
│   ├── Entities
│   ├── Dtos
│   ├── Exceptions
│   └── ViewModels
├── Views
```

---

## 💡 設計のポイント（MVC別）

---

### 🎮 Controller（責務の最小化・ユースケースの入口）

Controllerは **「リクエスト受付とレスポンス制御のみに専念」** し、ビジネスロジックは一切持たない設計とする。

#### ✔ 主な責務

* ルーティング（URLと処理の紐付け）
* リクエストデータの受け取り（Route / Form / Session）
* 入力の最低限の検証（null / 空チェックなど）
* Serviceの呼び出し
* 画面遷移の制御（View / Redirect）
* カート系APIにおけるJSONレスポンスの返却（AJAX対応）

---

#### ✔ 設計方針

##### ① ビジネスロジックを持たない

```csharp
return View(dbService.GetItemSalesStockList());
```

* 処理の実体はすべて Service に委譲
* Controllerは「呼び出すだけ」に徹する

---

##### ② PRGパターン（Post-Redirect-Get）の採用

```csharp
return LocalRedirect($"/orders/{orderCd}");
```

* 二重送信防止
* リロード安全性の確保
* カート追加（`/cart/items` POST）や注文登録（`/orders` POST）で徹底

---

##### ③ 状態管理の明確化（Session / TempData）

**Session**：画面をまたいで保持したい状態（ログインを継続する認証情報・カート内容）

```csharp
HttpContext.Session.SetString("customerId", customerEty.CustomerId);
var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart");
HttpContext.Session.SetInt32("count", cartItemVmList.Count);
```

**TempData**：リダイレクト後1回限り表示したい一時メッセージ（Toast通知など）

```csharp
TempData["ToastMessage"] = "カートは既に空です。";
TempData["ToastType"] = "warning";
return RedirectToAction("Index", "Cart");
```

* `Session.SetObject / GetObject` はJSONシリアライズによる拡張メソッド（`HomeController.SessionExtensions`）で実現
* Session Cookieは `HttpOnly` 化し、タイムアウトは `IOTimeout = 3600秒` に設定（`Program.cs`）

---

##### ④ 認証チェックはControllerで実施

```csharp
if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerId")))
{
    // View("../Account/Login")で指定するとブラウザにパスが残るためRedirectを使用
    return RedirectToAction("Login", "Account");
}
```

* 未ログイン状態を各Controllerの入口で早期に排除
* 認証情報は `Session` に統一し、`TempData` は一時的な通知専用とする
* Serviceに認証の責務を持たせない

---

##### ⑤ 例外は画面制御に変換

```csharp
catch (Exception e)
{
    if (e.InnerException is OrderException orderEx)
    {
        ViewData["message"] = orderEx.Message;
        return View("NoStockError");
    }
    return View("Error");
}
```

* `DatabaseFramework` がトランザクション内例外を `DatabaseServiceException` でラップして送出するため、
  Controller側では `e.InnerException` を判定して業務例外（在庫不足など）を取り出す
* 業務例外（在庫不足など）→ 専用エラー画面（`NoStockError.cshtml`）でユーザーに状況を明示
* システム例外 → 共通エラー画面（`Error.cshtml`）

---

##### ⑥ CSRF対策（AntiForgery）

通常のフォーム送信には `@Html.AntiForgeryToken()` ＋ `[ValidateAntiForgeryToken]` を使用する一方、
カート操作のようなAJAX（PUT/DELETE）通信では、Cookieに加えてトークンをHTTPヘッダーで送る必要がある。

```csharp
// Program.cs
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});
```

```csharp
// CartController
[AutoValidateAntiforgeryToken]
public class CartController(CartService dbService) : Controller
```

View側ではトークンを `<meta>` タグに埋め込み、JSから読み取ってヘッダーに設定する。

```html
@inject Microsoft.AspNetCore.Antiforgery.IAntiforgery Xsrf
<meta name="csrf-token" content="@Xsrf.GetAndStoreTokens(Context).RequestToken" />
```

```javascript
const token = document.querySelector('meta[name="csrf-token"]').content;
fetch(`/cart/items/${janCd}`, {
    method: 'PUT',
    headers: { 'X-CSRF-TOKEN': token, 'Content-Type': 'application/x-www-form-urlencoded' },
    body: new URLSearchParams({ qty })
});
```

---

##### ⑦ カートAPIのJSONレスポンス設計

数量変更・削除は画面遷移せず、更新後のカート内容をJSONで返してJS側で再描画する。

```csharp
[HttpPut("/cart/items/{janCd}")]
public IActionResult Update([FromRoute] string janCd, [FromForm] int qty)
{
    ...
    return Ok(cartItemVmList);
}
```

* `AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)` により
  プロパティ名は camelCase 変換されず、C#側のPascalCaseのままJSに渡される（`item.JanCd` など）
* 異常時（対象商品なし等）は `BadRequest()` を返し、JS側でユーザーにアラート表示する

---

#### ✔ Controllerを薄く保つメリット

* テストしやすい（ロジックがない）
* 可読性向上
* 責務が明確
* Serviceの再利用性向上

---

### 🧠 Model（責務分離とビジネスロジック集約）

Modelは以下の要素で構成される：

* Service（ユースケース単位）
* Action（処理単位）
* DAO（DBアクセス）
* データモデル（DTO / Entity / ViewModel）

---

#### ✔ Service

* ユースケースの入口
* Actionの生成と実行
* トランザクション境界の統一

```csharp
public class OrderService(DatabaseFramework dbFramework,
                        OrderDao orderDao,
                        OrderHeaderDao orderHeaderDao,
                        OrderDetailDao orderDetailDao,
                        StockDao stockDao,
                        SalesDao salesDao)
{
    public string CreateOrder(OrderWriteViewModel orderWriteVm, List<CartItemViewModel> cartItemVmList)
    {
        var action = new OrderWriteAction(orderWriteVm, cartItemVmList,
            orderHeaderDao, orderDetailDao, stockDao, salesDao);
        return dbFramework.Execute(action)!;
    }
}
```

* 用意されているService：`AccountService` / `CartService` / `ItemService` / `OrderService` / `RegisterService`
* いずれも DI（`AddTransient`）によりControllerへ注入される（`Program.cs`）

---

#### ✔ Action

* DB処理の単位
* Read / Writeで責務分離

```csharp
public interface IReadAction<T> { T? ExecuteQuery() { throw new NotImplementedException(); } }
public interface IWriteAction<T> { T? ExecuteNonQuery() { throw new NotImplementedException(); } }
```

代表例：注文登録処理 `OrderWriteAction`（下記「在庫引当と排他制御」参照）

---

#### ✔ DatabaseFramework

* トランザクション管理（`IWriteAction` 実行時に自動でBegin/Commit/Rollback）
* 例外ハンドリング（接続エラーとDBエラーを区別して`DatabaseServiceException`に変換）
* ログ出力（`ExceptionLogger` による日次ログファイル）

```csharp
public T? Execute<T>(IWriteAction<T> action)
{
    using var transaction = connectionManager!.BeginTransaction();
    try
    {
        var result = action.ExecuteNonQuery();
        transaction.Commit();
        return result;
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        throw new Exception(ex.Message, ex);
    }
}
```

---

#### ✔ DAO

* SQL実行のみ担当
* ビジネスロジックを持たない
* `BaseEntityDao<T>` / `BaseDtoDao<T>`（DBManager提供）を継承して実装

| DAO | 対象 | 備考 |
|---|---|---|
| `CustomerDao` | `m_customer` | ログイン認証 |
| `DestinationDao` | `m_destination` | 配送先一覧取得 |
| `ItemSalesStockDao` | `v_item_sales_stock` | 商品＋在庫数の結合ビュー |
| `SalesDao` | `v_sales` | 有効な販売情報 |
| `StockDao` | `t_stock` | 在庫引当（排他制御あり） |
| `OrderDao` | `v_order` | 注文詳細の結合ビュー |
| `OrderHeaderDao` / `OrderDetailDao` | `t_order_header` / `t_order_detail` | 注文登録 |

---

#### ✔ データモデル

* **DTO**：DB取得用（結合テーブル・SQLビュー由来）
  例）`ItemSalesStockDto`（商品＋在庫）、`OrderDto`（注文ヘッダ＋配送先＋明細の結合結果）、`OrderItemDto`（`OrderDetailEntity`を継承し表示用に`Item`を付加）
* **Entity**：DB更新用（テーブル単位）
  例）`CustomerEntity` / `DestinationEntity` / `SalesEntity` / `StockEntity` / `OrderHeaderEntity` / `OrderDetailEntity`
* **ViewModel**：画面表示・入力バインディング用
  例）`LoginViewModel` / `CartItemViewModel` / `OrderWriteViewModel` / `OrderScheduledDeliveryViewModel` / `OrderReadViewModel`

---

#### ✔ 在庫引当と排他制御

複数ユーザーが同時に同じ商品を注文しても在庫が二重に引き当てられないよう、
在庫取得SQLに行ロックを付与している。

```csharp
// StockDao.Find
string query = $@"
    SELECT TOP {qty} stock_no
    FROM t_stock WITH (XLOCK, ROWLOCK)
    WHERE order_cd IS NULL AND jan_cd = @janCd";
```

`OrderWriteAction`（注文登録）は以下の流れで実行され、全体が単一トランザクション内で完結する。

1. 現在日時から注文番号を生成
2. `t_order_header` に注文ヘッダを登録
3. カート内商品ごとに、未引当在庫を `XLOCK, ROWLOCK` 付きで取得
4. 取得数が要求数を下回る／0件の場合は `OrderException` を送出（→ロールバック）
5. 在庫1件ごとに `t_order_detail` へ注文明細を登録
6. 対象在庫に `order_cd` を設定して引当を確定（`StockDao.Patch`）

在庫不足は業務エラーとして扱われ、`DatabaseFramework` → `OrdersController` を通じて
`NoStockError.cshtml` にユーザー向けメッセージとして表示される。

---

### 🖥️ View（表示責務の限定）

Viewは **「表示のみ」** に責務を限定する。

#### ✔ 方針

* ロジックを持たない（if最小限）
* ViewModelのみを使用
* HTML生成に専念

---

#### ✔ 禁止事項

* DBアクセス
* Service呼び出し
* 複雑な計算ロジック

---

#### ✔ カート画面のJSレンダリングとXSS対策

カート画面（`Views/Cart/Index.cshtml`）は初期表示以降、Razorの再レンダリングを行わず、
サーバーから受け取ったJSONをJavaScriptでDOM構築する方式に変更されている。

```javascript
const items = @Html.Raw(System.Text.Json.JsonSerializer.Serialize(ViewBag.Items));
const cart  = @Html.Raw(System.Text.Json.JsonSerializer.Serialize(Model));
document.addEventListener("DOMContentLoaded", () => renderCart(cart));
```

* 数量変更（PUT）・削除（DELETE）はフォーム送信ではなく `fetch` で非同期実行し、
  レスポンスのJSONで画面を再描画する
* 在庫数がカート内数量を下回っている場合、JS側で自動的に数量調整・在庫0の場合は自動削除を行い、
  ユーザーにアラートで通知する
* 商品名などのテキストは `textContent` を用いて挿入し、HTMLとして解釈されないようにする（XSS対策）
* 画像URLは `isSafeImageUrl()` で `http` / `https` のみ許可し、不正なプロトコル（`javascript:` 等）を排除する

---

#### ✔ Toast通知

サーバー側で `TempData` にセットしたメッセージを、Bootstrapの Toast コンポーネントで表示する。

```html
@if (TempData["ToastMessage"] != null)
{
    <div id="serverToast" class="toast text-bg-@(TempData["ToastType"] ?? "primary")">
        <div class="toast-body">@TempData["ToastMessage"]</div>
    </div>
}
```

主に「カートが空の状態でレジ・注文確定にアクセスした場合」のフィードバックに利用される
（`RegisterController` / `OrdersController`）。

---

#### ✔ メリット

* デザイナーとの分業が可能
* 表示変更の影響範囲が限定される
* JS側での即時フィードバックによりUX向上

---

## 🔒 横断的な対策

* **キャッシュ制御**：全レスポンスに `Cache-Control: no-store, no-cache, must-revalidate` を付与し、
  ブラウザバック時に古いカート状態が表示されるのを防止（`Program.cs` のミドルウェア）
* **セッション有効化**：`AddSession` によりCookieベースのセッションを利用（`HttpOnly` / `IsEssential`）
* **CSRF対策**：フォーム送信は`AntiForgeryToken`、AJAX通信はヘッダートークン方式で統一的に対策
* **例外ログ**：DB接続エラー・業務例外を問わず `ExceptionLogger` で日次ログに記録（`logs/` ディレクトリ）

---

## 🔗 外部依存（DBManager）

本プロジェクトは、以下の外部ライブラリに依存しています。

### 📦 DBManager.dll

* リポジトリ
  https://github.com/sna74849/DBManager

* 役割
  データベースアクセスを抽象化する共通基盤ライブラリ

---

### 🧩 主な機能

DBManager は以下の機能を提供します：

* DBコネクション管理（単一コネクション・単一トランザクション方式）
* SQL実行ラッパー（`SqlCommandBuilder` によるビルダーパターン）
* DAO基底クラスの提供

  * `IReadableDao<T>` / `IWritableDao<T>`
  * `BaseEntityDao<T>`（読み書き両対応）
  * `BaseDtoDao<T>`（読み取り専用）
* トランザクション制御の補助（`ITransactionManager`）
* `SqlDataReader` 拡張メソッドによる安全な値取得（`GetNonNullString` / `GetNullableDateTime` など）
* 例外ロギング（`ExceptionLogger`）

---

### 🏗️ 本プロジェクトとの関係

```text
ShoppingCart
   ↓
DatabaseFramework（トランザクション制御）
   ↓
DAO（OrderDao など）
   ↓
DBManager.dll ← ★ここに依存
   ↓
Database
```

---

### 🎯 設計上の位置付け

本ライブラリは以下の責務を担う：

* インフラ層（Infrastructure Layer）
* DBアクセスの詳細を隠蔽
* DAOの実装を簡素化

---

### 💡 利用例

```csharp
using var cmd = new SqlCommandBuilder()
    .WithCommandText(query)
    .AddParameter("@orderCd", orderCd)
    .Build();

using var reader = cmd.ExecuteReader();
```

---

### ⚠️ 注意事項

* 本プロジェクトは DBManager に強く依存している
* 別のORM（Entity Framework等）への置き換えは設計変更が必要
* DAO層は DBManager の抽象に従って実装されている
* DBManager自体は `.NET Framework 4.8` をターゲットとしたクラスライブラリであり、
  ASP.NET Core（ShoppingCart本体）からNuGet/参照の形で利用される

---

## 📜 ライセンス

This project is for educational purposes.
