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

## 🧾 主な機能

### ✔ 注文取得

* 注文コードで検索
* ヘッダ + 明細をまとめて取得
* ViewModelへ変換

---

### ✔ 注文登録

* 注文番号自動生成
* 注文ヘッダ登録
* 在庫チェック
* 注文明細登録
* 在庫引当

---

## ⚠️ バリデーション / 前提条件

### 注文登録

* カートが空でないこと
* 配送情報が商品数と一致していること
* 在庫が十分に存在すること

---

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
