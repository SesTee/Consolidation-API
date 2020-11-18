-- StatementDB.dbo.Banks definition

-- Drop table

-- DROP TABLE StatementDB.dbo.Banks GO

CREATE TABLE Banks (
	Id int IDENTITY(1,1) NOT NULL,
	logo varchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	bank_code varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	bank_name varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	bank_address varchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	bank_contact_name varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	bank_contact_phone varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	bank_contact_email varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	dateadded datetime NOT NULL,
	datemodified datetime NULL,
	CONSTRAINT Banks_PK PRIMARY KEY (Id),
	CONSTRAINT Banks_UN UNIQUE (bank_code)
) GO
CREATE UNIQUE INDEX Banks_UN ON Banks (bank_code) GO;


-- StatementDB.dbo.Accounts definition

-- Drop table

-- DROP TABLE StatementDB.dbo.Accounts GO

CREATE TABLE Accounts (
	Id int IDENTITY(1,1) NOT NULL,
	account_no varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	account_name varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	bank_code varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	current_balance decimal(18,0) NULL,
	previous_balance decimal(18,0) NULL,
	status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	create_datetime datetime NULL,
	lastupdatetime datetime NULL,
	CONSTRAINT Accounts_PK PRIMARY KEY (Id),
	CONSTRAINT Accounts_UN UNIQUE (account_no,bank_code),
	CONSTRAINT Accounts_FK FOREIGN KEY (bank_code) REFERENCES Banks(bank_code)
) GO
CREATE UNIQUE INDEX Accounts_UN ON Accounts (account_no,bank_code) GO;