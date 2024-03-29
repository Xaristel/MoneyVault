BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Goals" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"User_Id"	INTEGER NOT NULL,
	"Account_Id"	INTEGER NOT NULL,
	"Name"	TEXT NOT NULL,
	"Required_Amount"	INTEGER NOT NULL,
	FOREIGN KEY("User_Id") REFERENCES "Users"("Id"),
	FOREIGN KEY("Account_Id") REFERENCES "Accounts"("Id"),
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Accounts" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"User_Id"	INTEGER NOT NULL,
	"Number"	INTEGER,
	"Name"	TEXT NOT NULL,
	"Current_Amount"	INTEGER NOT NULL,
	FOREIGN KEY("User_Id") REFERENCES "Users"("Id"),
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Account_Operations" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Account_Id"	INTEGER NOT NULL,
	"Type"	TEXT NOT NULL,
	"Amount"	INTEGER NOT NULL,
	FOREIGN KEY("Account_Id") REFERENCES "Accounts"("Id"),
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Expense_Types" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL UNIQUE,
	"Note"	TEXT,
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Incomes" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"User_Id"	INTEGER NOT NULL,
	"Income_Type_Id"	INTEGER NOT NULL,
	"Total_Amount"	INTEGER NOT NULL,
	"Date"	TEXT NOT NULL,
	"Note"	TEXT,
	FOREIGN KEY("User_Id") REFERENCES "Users"("Id"),
	FOREIGN KEY("Income_Type_Id") REFERENCES "Income_Types"("Id"),
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Notes" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"User_Id"	INTEGER NOT NULL,
	"Text"	TEXT NOT NULL,
	FOREIGN KEY("User_Id") REFERENCES "Users"("Id"),
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Products" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Type_Id"	INTEGER NOT NULL,
	"Name"	TEXT NOT NULL,
	"Price"	INTEGER NOT NULL,
	FOREIGN KEY("Type_Id") REFERENCES "Product_Types"("Id"),
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Product_Lists" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Expense_Id"	INTEGER NOT NULL,
	"Product_Id"	INTEGER NOT NULL,
	"Quantity"	INTEGER NOT NULL,
	"Price"	INTEGER NOT NULL,
	FOREIGN KEY("Product_Id") REFERENCES "Products"("Id"),
	FOREIGN KEY("Expense_Id") REFERENCES "Expenses"("Id"),
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Product_Types" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Income_Types" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL UNIQUE,
	"Note"	TEXT,
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Shops" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Users" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Login"	TEXT NOT NULL UNIQUE,
	"Password"	TEXT NOT NULL,
	"Name"	TEXT NOT NULL,
	"Surname"	TEXT NOT NULL,
	PRIMARY KEY("Id")
) WITHOUT ROWID;
CREATE TABLE IF NOT EXISTS "Expenses" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"User_Id"	INTEGER NOT NULL,
	"Expense_Type_Id"	INTEGER NOT NULL,
	"Shop_Id"	INTEGER NOT NULL,
	"Total_Price"	INTEGER NOT NULL,
	"Date"	TEXT NOT NULL,
	"Note"	TEXT,
	FOREIGN KEY("Expense_Type_Id") REFERENCES "Expense_Types"("Id"),
	FOREIGN KEY("Shop_Id") REFERENCES "Shops"("Id"),
	FOREIGN KEY("User_Id") REFERENCES "Users"("Id"),
	PRIMARY KEY("Id")
) WITHOUT ROWID;
INSERT INTO "Users" VALUES (0,'Admin','Password','Mikhail','Mavrin');
INSERT INTO "Users" VALUES (1,'TestLogin','TestPassword','Nataly','Mavrina');
COMMIT;
