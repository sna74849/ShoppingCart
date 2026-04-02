# 🛒 ShoppingCart（注文管理サンプルアプリ）

## 📌 概要

本プロジェクトは、ASP.NET Core MVC を用いたショッピングカート・注文管理アプリケーションです。

注文の「参照（Read）」および「登録（Write）」を中心に、以下の設計方針で構築されています。

* ユースケース単位の Service 層
* DB処理を抽象化した Action + Framework パターン
* トランザクション制御の一元化
* DTO / Entity / ViewModel の責務分離

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

---

#### ✔ 設計方針

##### ① ビジネスロジックを持たない

```csharp
return View(service.GetOrder(orderCd));
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

---

##### ③ 状態管理の明確化

* Session：カート情報
* TempData：認証情報（customerId など）

```csharp
var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>("cart");
```

---

##### ④ 認証チェックはControllerで実施

```csharp
if (string.IsNullOrEmpty((string?)TempData.Peek("customerId")))
{
    return RedirectToAction("Login", "Account");
}
```

* 未ログイン状態を早期に排除
* Serviceに不要な責務を持たせない

---

##### ⑤ 例外は画面制御に変換

```csharp
catch (OrderException e)
{
    ViewData["message"] = e.Message;
    return View("Error");
}
```

* 業務例外 → ユーザー向けメッセージ
* システム例外 → エラーページ

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
dbFramework.Execute(action);
```

---

#### ✔ Action

* DB処理の単位
* Read / Writeで責務分離

```csharp
IReadAction<T>
IWriteAction<T>
```

---

#### ✔ DatabaseFramework

* トランザクション管理
* 例外ハンドリング
* ログ出力

---

#### ✔ DAO

* SQL実行のみ担当
* ビジネスロジックを持たない

---

#### ✔ データモデル

* DTO：DB取得用
* Entity：DB更新用
* ViewModel：画面表示用

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

#### ✔ メリット

* デザイナーとの分業が可能
* 表示変更の影響範囲が限定される

---


## 💡 設計のポイント

### ① Actionパターン

```csharp
public interface IReadAction<T>
{
    T? ExecuteQuery();
}

public interface IWriteAction<T>
{
    T? ExecuteNonQuery();
}
```

* DB処理をクラスとしてカプセル化
* Serviceは「何をするか」に集中

---

### ② DatabaseFramework

```csharp
dbFramework.Execute(action);
```

* トランザクション管理を集中
* 例外ログ出力
* Actionの実行のみ責務

---

### ③ Service層

```csharp
public string CreateOrder(...)
```

* ユースケース単位で責務を持つ
* Action生成と実行を担当
* Controllerを薄く保つ

---

### ④ DAOの役割

* SQLの実行のみ担当
* ビジネスロジックは持たない

---
### ⑤ Data Class

| 例外                       | 内容            |
| ------------------------ | ------------- |
| entity                   | テーブル構造と一致 |
| dto                      | 結合テーブル、Sqlビュー       |
| viewModel                | リクエスト・レスポンス        |

## 🔐 トランザクション

* DatabaseFramework により一元管理
* Write処理はすべてトランザクション内で実行

```csharp
using var transaction = connectionManager.BeginTransaction();
```

---

## 🚨 例外設計

| 例外                       | 内容            |
| ------------------------ | ------------- |
| xxxxxException           | 業務エラー（在庫不足など） |
| DatabaseServiceException | DB処理エラー       |
| Exception                | 想定外エラー        |

---

## 👨‍💻 開発方針まとめ

* Controllerは薄く保つ
* Serviceはユースケース単位で設計
* Actionで処理を分離
* DB制御はFrameworkへ集約

---

## 📜 ライセンス

This project is for educational purposes.
