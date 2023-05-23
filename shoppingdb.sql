/* 処理件数の表示を無効 */
SET NOCOUNT ON;
GO

/* 暗黙のトランザクションを無効 */
SET IMPLICIT_TRANSACTIONS OFF;
GO

/* DB作成 */
use [master];
if (EXISTS(SELECT * FROM sysdatabases WHERE name IN('[shoppingdb]','shoppingdb')))
begin
	ALTER DATABASE [shoppingdb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE [shoppingdb] SET MULTI_USER WITH NO_WAIT;
	DROP DATABASE shoppingdb;
end
CREATE DATABASE [shoppingdb] COLLATE Japanese_CI_AS;
GO


/* 暗黙のトランザクションを有効 */
SET IMPLICIT_TRANSACTIONS ON;
GO


/* ログインを作成 */
if exists (select name from sys.server_principals where name = N'shopowner')
	DROP LOGIN [shopowner];
CREATE LOGIN [shopowner] WITH PASSWORD=N'password', DEFAULT_DATABASE=[shoppingdb], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;
GO


/* データベースユーザを作成して権限を付与 */
USE [shoppingdb];
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'shopowner')
	DROP USER [shopowner];
CREATE USER [shopowner] FOR LOGIN [shopowner];
GRANT CONTROL TO [shopowner];
GO

CREATE TABLE m_customer (
    customer_id VARCHAR(20) PRIMARY KEY NOT NULL,
    password VARCHAR(40) NOT NULL
);
GO

CREATE TABLE m_jancode (
    jan_cd VARCHAR(13) PRIMARY KEY NOT NULL,
    item_nm NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE m_salse_item (
    item_cd CHAR(10) PRIMARY KEY NOT NULL,
    jan_cd VARCHAR(13) FOREIGN KEY(jan_cd) REFERENCES m_jancode(jan_cd) NOT NULL , 
    price INT NOT NULL,
    creade_at DATETIME2 NOT NULL,
    del_flag CHAR(1) 
);
GO

CREATE TABLE t_order (
    order_cd CHAR(11) NOT NULL,
    branch_no INT NOT NULL,
    customer_id VARCHAR(20) FOREIGN KEY(customer_id) REFERENCES m_customer(customer_id) NOT NULL,
    item_cd CHAR(10) FOREIGN KEY(item_cd) REFERENCES m_salse_item(item_cd) NOT NULL,
    creade_at DATETIME2 NOT NULL,
    PRIMARY KEY(order_cd,branch_no)
);
GO

CREATE TABLE t_stock (
    stock_no INT IDENTITY PRIMARY KEY NOT NULL,
    jan_cd VARCHAR(13) FOREIGN KEY(jan_cd) REFERENCES m_jancode(jan_cd) NOT NULL, 
    order_cd CHAR(11),
    branch_no INT,
    creade_at DATETIME2 NOT NULL,
    update_at DATETIME2 NOT NULL,
    FOREIGN KEY(order_cd,branch_no) REFERENCES t_order(order_cd,branch_no)
);
GO

CREATE TABLE m_jancode_photo (
    jan_cd VARCHAR(13) PRIMARY KEY FOREIGN KEY(jan_cd) REFERENCES m_jancode(jan_cd) NOT NULL, 
    file_path VARCHAR(100) NOT NULL
);
GO

CREATE TABLE m_destination (
    destination_no INT IDENTITY PRIMARY KEY NOT NULL, 
    customer_id VARCHAR(20) FOREIGN KEY(customer_id) REFERENCES m_customer(customer_id) NOT NULL,
    name NVARCHAR(20) NOT NULL,
    postcode VARCHAR(10) NOT NULL,
    address NVARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    email VARCHAR(100) NOT NULL
);
GO

CREATE TABLE t_delivery (
    order_cd CHAR(11),
    branch_no INT,
    destination_no INT FOREIGN KEY(destination_no) REFERENCES m_destination(destination_no) NOT NULL, 
    delivery_at DATE,
    send_at DATE,
    create_at DATETIME2,
    FOREIGN KEY(order_cd,branch_no) REFERENCES t_order(order_cd,branch_no)

);
GO

INSERT INTO m_customer(customer_id, password) VALUES('account1@emtech.com','p');

INSERT INTO m_jancode (jan_cd, item_nm) VALUES('0000000000000','カメラ');
INSERT INTO m_jancode (jan_cd, item_nm) VALUES('0000000000001','エアコン');

INSERT INTO m_salse_item(item_cd, jan_cd, price, creade_at, del_flag) VALUES('0000000000','0000000000000',10000,GETDATE(),'1');
INSERT INTO m_salse_item(item_cd, jan_cd, price, creade_at, del_flag) VALUES('0000000000','0000000000000',9980,GETDATE(),'0');
INSERT INTO m_salse_item(item_cd, jan_cd, price, creade_at, del_flag) VALUES('0000000001','0000000000001',50000,GETDATE(),'0');

INSERT INTO m_jancode_photo(jan_cd, file_path) VALUES('0000000000000','/img/PENTAXk33PAR53709_TP_V4.jpg');
INSERT INTO m_jancode_photo(jan_cd, file_path) VALUES('0000000000001','/img/kotetsuPAR515612017_TP_V4.jpg');

INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000',NULL,NULL,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000',NULL,NULL,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000',NULL,NULL,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000',NULL,NULL,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000',NULL,NULL,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000',NULL,NULL,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000',NULL,NULL,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000',NULL,NULL,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000',NULL,NULL,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000000','00000000001',1,GETDATE(),GETDATE());

INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000001','00000000001',2,GETDATE(),GETDATE());
INSERT INTO t_stock(jan_cd, order_cd, branch_no, creade_at, update_at) VALUES('0000000000001',NULL,NULL,GETDATE(),GETDATE());

INSERT INTO t_order(order_cd, branch_no, customer_id, item_cd, creade_at) VALUES('00000000001',1,'account1@emtech.com','0000000000',GETDATE());
INSERT INTO t_order(order_cd, branch_no, customer_id, item_cd, creade_at) VALUES('00000000001',2,'account1@emtech.com','0000000001',GETDATE());

INSERT INTO m_destination(customer_id, name, postcode, address, phone, email ) VALUES('account1@emtech.com', 'エンベックス', '102-0083','東京都千代田区麹町5丁目3番地 麹町中田ビル5F','03-6384-1435','account1@emtech.com');
INSERT INTO m_destination(customer_id, name, postcode, address, phone, email ) VALUES('account1@emtech.com', 'エンベックス', '510-0086','三重県四日市市諏訪栄町4-10 アピカビル4F','059-340-8140','account1@emtech.com');
INSERT INTO m_destination(customer_id, name, postcode, address, phone, email ) VALUES('account1@emtech.com', 'エンベックス', '231-0015','神奈川県横浜市中区尾上町4丁目47番地 リスト関内ビル201','03-6384-1435','account1@emtech.com');

/* トランザクションの確定 */
COMMIT;
GO