/* 処理件数の表示を無効にする（メッセージ "x rows affected" を抑止） */
SET NOCOUNT ON;
GO

/* 暗黙のトランザクションを無効にする（明示的にBEGIN TRANSACTIONが必要になる） */
SET IMPLICIT_TRANSACTIONS OFF;
GO

/* データベース作成前処理 */
USE [master];
IF (EXISTS(SELECT * FROM sysdatabases WHERE name IN('[shoppingdb]','shoppingdb')))
BEGIN
    -- DBが存在する場合、強制的に削除する
    ALTER DATABASE [shoppingdb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; -- 使用中でも強制排他
    ALTER DATABASE [shoppingdb] SET MULTI_USER WITH NO_WAIT;             -- 排他解除
    DROP DATABASE shoppingdb;                                            -- 削除
END
-- データベースを作成（照合順序：日本語、大文字小文字区別なし）
CREATE DATABASE [shoppingdb] COLLATE Japanese_CI_AS;
GO

/* 暗黙のトランザクションを有効にする */
SET IMPLICIT_TRANSACTIONS ON;
GO

/* SQL Serverログイン作成 */
IF EXISTS (SELECT name FROM sys.server_principals WHERE name = N'shopowner')
    DROP LOGIN [shopowner];  -- 既に存在していれば削除
-- ログイン作成（パスワード：password、有効期限・ポリシーなし）
CREATE LOGIN [shopowner] WITH PASSWORD=N'password', DEFAULT_DATABASE=[shoppingdb], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;
GO

/* データベースユーザー作成と権限付与 */
USE [shoppingdb];
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'shopowner')
    DROP USER [shopowner]; -- 既に存在していれば削除
CREATE USER [shopowner] FOR LOGIN [shopowner]; -- DBユーザー作成
GRANT CONTROL TO [shopowner]; -- データベース全体に対する制御権限を付与
GO

-- 顧客マスタ
CREATE TABLE m_customer (
    customer_id VARCHAR(20) PRIMARY KEY NOT NULL, -- 顧客ID
    password VARCHAR(40) NOT NULL,                -- パスワード
    email VARCHAR(255) UNIQUE NOT NULL,            -- メールアドレス（ユニーク制約）
    created_at DATETIME2 NOT NULL DEFAULT GETDATE(),     -- 登録日時
    updated_at DATETIME2 NOT NULL DEFAULT GETDATE(),     -- 更新日時        
    del_flag BIT NOT NULL DEFAULT 0              -- 削除フラグ（0=有効, 1=削除）
);
GO

-- 配送先マスタ
CREATE TABLE m_destination (
    destination_no INT IDENTITY PRIMARY KEY NOT NULL, -- 配送先番号（自動採番）
    customer_id VARCHAR(20) NOT NULL REFERENCES m_customer(customer_id), -- 顧客ID（外部キー）
    name NVARCHAR(20) NOT NULL,                       -- 配送先名
    postcode VARCHAR(10) NOT NULL,                    -- 郵便番号
    address NVARCHAR(100) NOT NULL,                   -- 住所
    phone VARCHAR(20) NOT NULL,                        -- 電話番号
    created_at DATETIME2 NOT NULL DEFAULT GETDATE(),     -- 登録日時
    updated_at DATETIME2 NOT NULL DEFAULT GETDATE(),     -- 更新日時        
    del_flag BIT NOT NULL DEFAULT 0              -- 削除フラグ（0=有効, 1=削除） 
);
GO

-- 商品マスタ
CREATE TABLE m_item (
    jan_cd VARCHAR(13) PRIMARY KEY NOT NULL, -- JANコード（13桁）
    item_nm NVARCHAR(100) NOT NULL,          -- 商品名（日本語対応のNVARCHAR）
    file_nm VARCHAR(255) NOT NULL,          -- 画像ファイル名（相対パスなど）
    created_at DATETIME2 NOT NULL DEFAULT GETDATE(),     -- 登録日時
    updated_at DATETIME2 NOT NULL DEFAULT GETDATE(),     -- 更新日時        
    del_flag BIT NOT NULL DEFAULT 0              -- 削除フラグ（0=有効, 1=削除）     
);
GO

-- 販売マスタ
CREATE TABLE m_sales (
    sales_cd CHAR(10) PRIMARY KEY NOT NULL,             -- 商品コード（10桁固定）
    jan_cd VARCHAR(13) NOT NULL REFERENCES m_item(jan_cd), -- JANコード（外部キー）
    price INT NOT NULL,                                -- 価格
    created_at DATETIME2 NOT NULL DEFAULT GETDATE(),     -- 登録日時
    updated_at DATETIME2 NOT NULL DEFAULT GETDATE(),     -- 更新日時        
    del_flag BIT NOT NULL DEFAULT 0              -- 削除フラグ（0=有効, 1=削除）
);
GO

-- 有効販売ビュー
CREATE VIEW v_sales AS
SELECT 
    s.sales_cd,
    s.jan_cd,
    s.price
FROM
    m_sales s
WHERE
    s.del_flag = '0'; -- 削除フラグが'0'の有効な商品だけを表示
GO

-- 商品販売ビュー
CREATE VIEW v_item_sales AS
SELECT 
    s.sales_cd,
    s.jan_cd,
    s.price,
    i.item_nm,
    i.file_nm
FROM
    v_sales s
INNER JOIN
    m_item i ON s.jan_cd = i.jan_cd
GO

-- 在庫テーブル
CREATE TABLE t_stock (
    stock_no INT IDENTITY PRIMARY KEY NOT NULL,        -- 在庫番号（自動採番）
    jan_cd VARCHAR(13) NOT NULL REFERENCES m_item(jan_cd), -- JANコード（外部キー）
    order_cd CHAR(11),                                 -- 注文コード
    created_at DATETIME2 NOT NULL DEFAULT GETDATE(),     -- 登録日時
    updated_at DATETIME2 NOT NULL DEFAULT GETDATE()     -- 更新日時        
);
GO

-- 商品販売在庫ビュー
CREATE VIEW v_item_sales_stock AS
SELECT 
    i.jan_cd,
    i.price,
    i.item_nm,
    i.file_nm,
    COUNT(s.stock_no) AS qty      -- 在庫が無ければ 0 になる
FROM 
    v_item_sales i
LEFT JOIN 
    t_stock s 
    ON s.jan_cd = i.jan_cd
    AND s.order_cd IS NULL 
GROUP BY 
    i.jan_cd, i.sales_cd, i.price, i.item_nm, i.file_nm
GO

-- 注文ヘッダー
CREATE TABLE t_order_header (
    order_cd CHAR(11) PRIMARY KEY NOT NULL,                     -- 注文コード（固定長）
    destination_no INT NOT NULL REFERENCES m_destination(destination_no), -- 配送先番号
    created_at DATETIME2 NOT NULL DEFAULT GETDATE()             -- 注文日時
);
GO

-- 注文明細
CREATE TABLE t_order_detail (
    order_cd CHAR(11) NOT NULL REFERENCES t_order_header(order_cd), -- 注文コード
    sales_cd CHAR(10) NOT NULL REFERENCES m_sales(sales_cd), -- 商品コード
    seq_no INT NOT NULL,                             -- 枝番（明細番号）
    scheduled_delivery_at DATE,                         -- 配送予定日
    shipped_at DATE,                                    -- 出荷日
    cancelled_at DATE,                                  -- キャンセル日
    created_at DATETIME2 NOT NULL DEFAULT GETDATE(),    -- 登録日時
    PRIMARY KEY(order_cd, sales_cd, seq_no)                    -- 主キー（注文コード＋枝番）
);
GO

-- 注文商品明細ビュー
CREATE VIEW v_order_item_sales AS
SELECT
    d.order_cd,
    d.sales_cd,
    d.scheduled_delivery_at,
    d.shipped_at,
    d.cancelled_at,
    si.jan_cd,
    si.price,
    si.item_nm,
    si.file_nm,
    COUNT (d.seq_no) AS qty -- 注文数
FROM
    t_order_detail d 
INNER JOIN
    (SELECT 
    s.sales_cd,
    s.jan_cd,
    s.price,
    i.item_nm,
    i.file_nm
FROM
    m_sales s
INNER JOIN
    m_item i ON s.jan_cd = i.jan_cd
) si ON d.sales_cd = si.sales_cd
GROUP BY 
    d.order_cd, d.sales_cd, d.scheduled_delivery_at, d.shipped_at, d.cancelled_at, si.jan_cd, si.price, si.item_nm, si.file_nm
GO

-- 注文ビュー
CREATE VIEW v_order AS
SELECT 
    h.order_cd,
    h.destination_no,
    m.customer_id,
    m.name AS destination_name,
    m.postcode,
    m.address,
    m.phone,
    d.sales_cd,
    d.scheduled_delivery_at,
    d.shipped_at,
    d.cancelled_at,
    d.jan_cd,
    d.item_nm,
    d.file_nm,
    d.price,
    d.qty
FROM
    t_order_header h
INNER JOIN
    m_destination m ON h.destination_no = m.destination_no
INNER JOIN
    v_order_item_sales d ON h.order_cd = d.order_cd
GO


-- 顧客情報 登録（テストデータ）
INSERT INTO m_customer(customer_id, password, email)
VALUES('account','p','account1@emtech.com');

-- JANコード情報 登録（テストデータ）
INSERT INTO m_item (jan_cd, item_nm, file_nm) VALUES('0000000000000','カメラ','PENTAXk33PAR53709_TP_V4.jpg');
INSERT INTO m_item (jan_cd, item_nm, file_nm) VALUES('0000000000001','エアコン','kotetsuPAR515612017_TP_V4.jpg');

-- 販売商品 登録（テストデータ）
INSERT INTO m_sales(sales_cd, jan_cd, price, del_flag)
VALUES('0000000000','0000000000000',10000,1); -- 削除フラグ付き
INSERT INTO m_sales(sales_cd, jan_cd, price, del_flag)
VALUES('0000000002','0000000000000',9980,0);
INSERT INTO m_sales(sales_cd, jan_cd, price, del_flag)
VALUES('0000000001','0000000000001',50000,0);

-- 在庫情報 登録（テストデータ）
-- カメラ（JAN:0000000000000）の在庫を10個登録
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');
INSERT INTO t_stock(jan_cd) VALUES('0000000000000');

-- エアコン（JAN:0000000000001）の在庫を1個登録
INSERT INTO t_stock(jan_cd) VALUES('0000000000001');

-- 配送先 登録（テストデータ）
INSERT INTO m_destination(customer_id, name, postcode, address, phone)
VALUES('account', 'エンベックス', '102-0083','東京都千代田区麹町5丁目3番地 麹町中田ビル5F','03-6384-1435');
INSERT INTO m_destination(customer_id, name, postcode, address, phone)
VALUES('account', 'エンベックス', '510-0086','三重県四日市市諏訪栄町4-10 アピカビル4F','059-340-8140');
INSERT INTO m_destination(customer_id, name, postcode, address, phone)
VALUES('account', 'エンベックス', '231-0015','神奈川県横浜市中区尾上町4丁目47番地 リスト関内ビル201','03-6384-1435');

-- トランザクションの確定（SET IMPLICIT_TRANSACTIONS ON による明示的なCOMMIT）
COMMIT;
GO
