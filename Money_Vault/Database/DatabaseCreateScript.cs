namespace Money_Vault.Database
{
    public static class DatabaseCreateScript
    {
        public static string DatabaseCreateCommand = @"
		CREATE TABLE IF NOT EXISTS 'Users' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'Login'	TEXT NOT NULL UNIQUE,
			'Password'	TEXT NOT NULL,
			'Name'	TEXT,
			'Surname'	TEXT,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Expenses' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'User_Id'	INTEGER NOT NULL,
			'Expense_Type_Id'	INTEGER NOT NULL,
			'Shop_Id'	INTEGER NOT NULL,
			'Total_Price'	INTEGER NOT NULL,
			'Date'	TEXT NOT NULL,
			'Note'	TEXT,
			FOREIGN KEY('User_Id') REFERENCES 'Users'('Id') ON DELETE CASCADE,
			FOREIGN KEY('Shop_Id') REFERENCES 'Shops'('Id') ON DELETE CASCADE,
			FOREIGN KEY('Expense_Type_Id') REFERENCES 'Expense_Types'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Incomes' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'User_Id'	INTEGER NOT NULL,
			'Income_Type_Id'	INTEGER NOT NULL,
			'Total_Amount'	INTEGER NOT NULL,
			'Date'	TEXT NOT NULL,
			'Note'	TEXT,
			FOREIGN KEY('Income_Type_Id') REFERENCES 'Income_Types'('Id') ON DELETE CASCADE,
			FOREIGN KEY('User_Id') REFERENCES 'Users'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Expense_Items' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'Expense_Id'	INTEGER NOT NULL,
			'Expense_Subtype_Id'	INTEGER NOT NULL,
			'Price'	INTEGER NOT NULL,
			FOREIGN KEY('Expense_Id') REFERENCES 'Expenses'('Id') ON DELETE CASCADE,
			FOREIGN KEY('Expense_Subtype_Id') REFERENCES 'Expense_Subtypes'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Notes' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'User_Id'	INTEGER NOT NULL,
			'Name'	TEXT NOT NULL,
			'Text'	TEXT,
			FOREIGN KEY('User_Id') REFERENCES 'Users'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Expense_Subtypes' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'UserId'	INTEGER NOT NULL,
			'Name'	TEXT NOT NULL UNIQUE,
			'Note'	TEXT,
			FOREIGN KEY('UserId') REFERENCES 'Users'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Expense_Types' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'UserId'	INTEGER NOT NULL,
			'Name'	TEXT NOT NULL UNIQUE,
			'Note'	TEXT,
			FOREIGN KEY('UserId') REFERENCES 'Users'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Income_Types' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'UserId'	INTEGER NOT NULL,
			'Name'	TEXT NOT NULL UNIQUE,
			'Note'	TEXT,
			FOREIGN KEY('UserId') REFERENCES 'Users'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Shops' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'UserId'	INTEGER NOT NULL,
			'Name'	TEXT NOT NULL UNIQUE,
			'Note'	TEXT,
			FOREIGN KEY('UserId') REFERENCES 'Users'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Account_Operations' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'Account_Id'	INTEGER NOT NULL,
			'Type'	TEXT NOT NULL,
			'Amount'	INTEGER NOT NULL,
			'Date'	TEXT NOT NULL,
			FOREIGN KEY('Account_Id') REFERENCES 'Accounts'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Accounts' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'User_Id'	INTEGER NOT NULL,
			'Number'	INTEGER NOT NULL,
			'Name'	TEXT,
			'Current_Amount'	INTEGER NOT NULL,
			FOREIGN KEY('User_Id') REFERENCES 'Users'('Id'),
			PRIMARY KEY('Id' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Goals' (
			'Id'	INTEGER NOT NULL UNIQUE,
			'User_Id'	INTEGER NOT NULL,
			'Account_Id'	INTEGER NOT NULL,
			'Name'	TEXT NOT NULL,
			'Required_Amount'	INTEGER NOT NULL,
			'Note'	TEXT,
			FOREIGN KEY('User_Id') REFERENCES 'Users'('Id') ON DELETE CASCADE,
			FOREIGN KEY('Account_Id') REFERENCES 'Accounts'('Id') ON DELETE CASCADE,
			PRIMARY KEY('Id' AUTOINCREMENT)
		);";

        public static string FillTestDataCommand = @"INSERT INTO 'Users' VALUES (0,'admin','-1982223992','Mikhail','Mavrin');
		INSERT INTO 'Expenses' VALUES(0,0,1,1,300000,'02.03.2022','Шпроты <Морской ключ> невкусные');
        INSERT INTO 'Expenses' VALUES(1,0,1,2,130000,'05.03.2022','Чипсы по акции');
        INSERT INTO 'Expenses' VALUES(6,0,1,2,130000,'10.03.2022','');
        INSERT INTO 'Expenses' VALUES(7,0,2,7,2000000,'12.03.2022','Худи + куртка');
        INSERT INTO 'Expenses' VALUES(8,0,2,7,500000,'12.03.2022','');
        INSERT INTO 'Expenses' VALUES(9,0,3,6,1500000,'14.03.2022','Удочка');
        INSERT INTO 'Expenses' VALUES(10,0,3,6,60000,'14.03.2022','Чехол');
        INSERT INTO 'Expenses' VALUES(11,0,4,5,5000000,'01.04.2022','Для кухни');
        INSERT INTO 'Expenses' VALUES(12,0,2,7,160000,'04.04.2022','');
        INSERT INTO 'Expenses' VALUES(13,0,1,2,400000,'05.04.2022','');
        INSERT INTO 'Expenses' VALUES(14,0,3,6,80000,'06.04.2022','Баночки 100мл для самолета');
        INSERT INTO 'Expenses' VALUES(15,0,1,1,400000,'05.04.2022','');
        INSERT INTO 'Expenses' VALUES(16,0,1,2,250000,'09.04.2022','');
        INSERT INTO 'Expenses' VALUES(17,0,2,7,420000,'14.04.2022','');
        INSERT INTO 'Expenses' VALUES(18,0,1,1,150000,'15.04.2022','');
        INSERT INTO 'Expenses' VALUES(19,0,1,3,320000,'23.04.2022','');
        INSERT INTO 'Expenses' VALUES(20,0,3,6,150000,'25.04.2022','');
        INSERT INTO 'Expenses' VALUES(21,0,4,5,1500000,'28.04.2022','');
        INSERT INTO 'Expenses' VALUES(22,0,4,5,600000,'01.05.2022','');
        INSERT INTO 'Expenses' VALUES(23,0,1,3,280000,'03.05.2022','');
        INSERT INTO 'Expenses' VALUES(24,0,2,7,240000,'05.05.2022','');
        INSERT INTO 'Incomes' VALUES(1,0,0,4500000,'02.02.2022','Аванс');
        INSERT INTO 'Incomes' VALUES(2,0,1,1200000,'04.02.2022', NULL);
        INSERT INTO 'Incomes' VALUES(3,0,2,600000,'06.02.2022','С основного');
        INSERT INTO 'Incomes' VALUES(4,0,0,4300000,'17.02.2022', NULL);
        INSERT INTO 'Incomes' VALUES(5,0,0,4500000,'02.03.2022','Аванс');
        INSERT INTO 'Incomes' VALUES(6,0,1,1100000,'04.03.2022', NULL);
        INSERT INTO 'Incomes' VALUES(7,0,2,610000,'06.03.2022','С основного');
        INSERT INTO 'Incomes' VALUES(8,0,0,4300000,'17.03.2022', NULL);
        INSERT INTO 'Incomes' VALUES(9,0,0,4500000,'02.04.2022','Аванс');
        INSERT INTO 'Incomes' VALUES(10,0,1,1230000,'04.04.2022', NULL);
        INSERT INTO 'Incomes' VALUES(11,0,2,620000,'06.04.2022','С основного');
        INSERT INTO 'Incomes' VALUES(12,0,0,4300000,'17.04.2022', NULL);
        INSERT INTO 'Incomes' VALUES(13,0,0,4500000,'02.05.2022','Аванс');
        INSERT INTO 'Incomes' VALUES(14,0,1,1000000,'04.05.2022', NULL);
        INSERT INTO 'Incomes' VALUES(15,0,2,630000,'06.05.2022','С основного');
        INSERT INTO 'Expense_Items' VALUES(1,19,1,250000);
        INSERT INTO 'Expense_Items' VALUES(2,19,2,40000);
        INSERT INTO 'Expense_Items' VALUES(3,19,3,30000);
        INSERT INTO 'Expense_Items' VALUES(4,18,1,90000);
        INSERT INTO 'Expense_Items' VALUES(5,18,2,10000);
        INSERT INTO 'Expense_Items' VALUES(6,18,0,50000);
        INSERT INTO 'Expense_Items' VALUES(7,23,0,80000);
        INSERT INTO 'Expense_Items' VALUES(8,23,1,150000);
        INSERT INTO 'Expense_Items' VALUES(9,23,2,50000);
        INSERT INTO 'Notes' VALUES(1,0,'Список покупок для ремонта','Обои, клей строительный, шпатель, люстра, плафоны...');
        INSERT INTO 'Notes' VALUES(2,0,'Список документов для работы','Оригинал паспорта, копия СНИЛС, копия паспорта...');
        INSERT INTO 'Notes' VALUES(3,0,'Личное','Купить электрощётку, заменить зеркало в ванной');
        INSERT INTO 'Expense_Subtypes' VALUES(0,0,'Лекарства', NULL);
        INSERT INTO 'Expense_Subtypes' VALUES(1,0,'Еда', NULL);
        INSERT INTO 'Expense_Subtypes' VALUES(2,0,'Хозяйственное', NULL);
        INSERT INTO 'Expense_Subtypes' VALUES(3,0,'Животные', NULL);
        INSERT INTO 'Expense_Types' VALUES(1,0,'Супермаркеты', NULL);
        INSERT INTO 'Expense_Types' VALUES(2,0,'Торговые центры', NULL);
        INSERT INTO 'Expense_Types' VALUES(3,0,'Интернет-магазины', NULL);
        INSERT INTO 'Expense_Types' VALUES(4,0,'Строительство', NULL);
        INSERT INTO 'Income_Types' VALUES(0,0,'Зарплата', NULL);
        INSERT INTO 'Income_Types' VALUES(1,0,'Дивиденды', NULL);
        INSERT INTO 'Income_Types' VALUES(2,0,'Проценты с накоп. счёта', NULL);
        INSERT INTO 'Shops' VALUES(1,0,'Yarche','Большой выбор');
        INSERT INTO 'Shops' VALUES(2,0,'KB','Дешево, но маленький выбор');
        INSERT INTO 'Shops' VALUES(3,0,'Пятерочка','Неудобно расположен');
        INSERT INTO 'Shops' VALUES(4,0,'Мария Ра','Дорого');
        INSERT INTO 'Shops' VALUES(5,0,'Леруа','Далеко');
        INSERT INTO 'Shops' VALUES(6,0,'Ozon', NULL);
        INSERT INTO 'Shops' VALUES(7,0,'ТЦ Аура', NULL);
        INSERT INTO 'Account_Operations' VALUES(1,0,'Пополнение',20000000,'14.03.2022');
        INSERT INTO 'Account_Operations' VALUES(2,0,'Пополнение',15000000,'28.04.2022');
        INSERT INTO 'Account_Operations' VALUES(3,0,'Пополнение',5000000,'15.05.2022');
        INSERT INTO 'Account_Operations' VALUES(4,1,'Пополнение',2000000,'14.03.2022');
        INSERT INTO 'Account_Operations' VALUES(5,1,'Снятие',500000,'20.03.2022');
        INSERT INTO 'Account_Operations' VALUES(6,1,'Снятие',350000,'30.04.2022');
        INSERT INTO 'Account_Operations' VALUES(7,1,'Пополнение',1200000,'15.05.2022');
        INSERT INTO 'Account_Operations' VALUES(8,2,'Пополнение',200000,'14.03.2022');
        INSERT INTO 'Account_Operations' VALUES(9,2,'Пополнение',3100000,'19.04.2022');
        INSERT INTO 'Account_Operations' VALUES(10,2,'Снятие',2500000,'29.04.2022');
        INSERT INTO 'Account_Operations' VALUES(11,2,'Пополнение',1300000,'01.05.2022');
        INSERT INTO 'Accounts' VALUES(0,0,4321,'Основной накопительный',120000000);
        INSERT INTO 'Accounts' VALUES(1,0,4542,'Основной',2500000);
        INSERT INTO 'Accounts' VALUES(2,0,3435,'Доп. накопительный 1',5000000);
        INSERT INTO 'Accounts' VALUES(3,0,2526,'Доп. накопительный 2',300000);
        INSERT INTO 'Goals' VALUES(1,0,1,'Купить бумагу',200000,'2 пачки');
        INSERT INTO 'Goals' VALUES(2,0,3,'Новые колонки',1000000,'В DNS');
        INSERT INTO 'Goals' VALUES(3,0,2,'Брекеты',12000000,'Центр стоматологии');
        INSERT INTO 'Goals' VALUES(4,0,0,'Купить квартиру',800000000,'Двухкомнатная');
        COMMIT;";
    }
}
