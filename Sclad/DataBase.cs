using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Threading;

namespace Sklad
{
    static class DataBase
    {
        public static readonly string dbFile = "Sklad.sdf";
        public static string ConStrDB { get; set; } = @"Data Source = " + dbFile + "; Persist Security Info=False;";

        public static bool CheckExistDB()
        {
            return File.Exists(dbFile);
        }

        public static void CreateDB()
        {
            SqlCeEngine engine = new SqlCeEngine(ConStrDB);
            engine.CreateDatabase();
            engine.Dispose();
        }

        public static void CreateAllTabels()
        {
            using (SqlCeConnection connection = new SqlCeConnection(ConStrDB))
            {
                connection.Open();
                // Создание таблицы Product_category
                string expression = @"CREATE TABLE P_category
                                    (
                                    id int IDENTITY NOT NULL PRIMARY KEY,
                                    name  nvarchar(25) NOT NULL
                                    )";
                SqlCeCommand cmd = new SqlCeCommand(expression, connection);

                    cmd.ExecuteNonQuery();

                    // Создание таблицы Product
                    expression = @"CREATE TABLE Product
                                    (
                                    code int NOT NULL PRIMARY KEY,
                                    name  nvarchar(80) NOT NULL,
                                    category int NOT NULL,
                                    FOREIGN KEY (category) REFERENCES P_category (id)
                                    )";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();



                    // Создание таблицы Catalog_type
                    expression = @"CREATE TABLE C_type
                                    (
                                    id int IDENTITY NOT NULL PRIMARY KEY,
                                    type nvarchar(30) NOT NULL
                                    )";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                    // Создание таблицы Catalog_period_year
                    expression = @"CREATE TABLE C_p_year
                                    (
                                    id int IDENTITY NOT NULL PRIMARY KEY,
                                    year int NOT NULL
                                    )";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                    // Создание таблицы Catalog_period
                    expression = @"CREATE TABLE C_period
                                    (
                                    id int IDENTITY NOT NULL PRIMARY KEY,
                                    number int NOT NULL,
                                    year int NOT NULL,
                                    FOREIGN KEY (year) REFERENCES C_p_year (id)
                                    )";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                    // Создание таблицы Catalog
                    expression = @"CREATE TABLE Catalog
                                    (
                                    id int IDENTITY NOT NULL PRIMARY KEY,
                                    period int NOT NULL,
                                    FOREIGN KEY (period) REFERENCES C_period (id),
                                    type int NOT NULL,
                                    FOREIGN KEY (type) REFERENCES C_type (id)
                                    )";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                    // Создание таблицы Price
                    expression = @"CREATE TABLE Price
                                    (
                                    id int IDENTITY NOT NULL PRIMARY KEY,
                                    code int NOT NULL,
                                    FOREIGN KEY (code) REFERENCES Product (code),
                                    pricePC money NOT NULL,
                                    priceDC money NOT NULL,
                                    catalog int NOT NULL,
                                    FOREIGN KEY (catalog) REFERENCES Catalog (id),
                                    quantity int NOT NULL,
                                    discont bit NOT NULL,
                                    description nvarchar(200)
                                    )";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteDB()
        {
            File.Delete(dbFile);
        }
        
        public static void FillTestData()
        {
            using (SqlCeConnection connection = new SqlCeConnection(ConStrDB))
            {
                connection.Open();
                // Заполнение таблицы Product_category
                string expression = @"INSERT INTO P_category
                                (name)
                                SELECT 'Декоративная косметика' UNION ALL
                                SELECT 'Парфюмы' UNION ALL
                                SELECT 'Wellness' UNION ALL
                                SELECT 'Аксессуары' UNION ALL
                                SELECT 'Уход для мужчин' UNION ALL
                                SELECT 'Уход за телом' UNION ALL
                                SELECT 'Уход за лицом' UNION ALL
                                SELECT 'Уход за волосами' UNION ALL
                                SELECT 'Детская серия'";
                SqlCeCommand cmd = new SqlCeCommand(expression, connection);
                    cmd.ExecuteNonQuery();

                    //Заполнение таблицы Product
                    expression = @"INSERT INTO Product 
                                (code, name, category)
                                SELECT '20533', 'Тушь для ресниц «УЛЬТРАобъем»', 1 UNION ALL
                                SELECT '20779','Лак для ногтей «100 % цвета» — Розовый Иней',1 UNION ALL
                                SELECT '23410','Мыло «Инжир и лаванда»',3 UNION ALL
                                SELECT '26261','Лак для ногтей «100 % цвета» — Спелая Слива',1 UNION ALL
                                SELECT '29054','Косметичка',5";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                    //Заполнение таблицы C_type
                    expression = @"INSERT INTO C_type
                                (type)
                                SELECT 'Основной' UNION ALL
                                SELECT 'Бизнес Класс' UNION ALL
                                SELECT 'Распродажа' UNION ALL
                                SELECT 'Акционный'";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                    //Заполнение таблицы C_p_type
                    expression = @"INSERT INTO C_p_year
                                (year)
                                SELECT 2016 UNION ALL
                                SELECT 2017 UNION ALL
                                SELECT 2018 UNION ALL
                                SELECT 2019";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                    //Заполнение таблицы C_period
                    expression = @"INSERT INTO C_period
                            (number, year)
                            SELECT 1, 1 UNION ALL
                            SELECT 2, 1 UNION ALL
                            SELECT 3, 1 UNION ALL
                            SELECT 4, 1 UNION ALL
                            SELECT 5, 1 UNION ALL
                            SELECT 6, 1 UNION ALL
                            SELECT 7, 1 UNION ALL
                            SELECT 8, 1 UNION ALL
                            SELECT 9, 1 UNION ALL
                            SELECT 10, 1 UNION ALL
                            SELECT 11, 1 UNION ALL
                            SELECT 12, 1 UNION ALL
                            SELECT 13, 1 UNION ALL
                            SELECT 14, 1 UNION ALL
                            SELECT 15, 1 UNION ALL
                            SELECT 16, 2 UNION ALL
                            SELECT 17, 1 UNION ALL
                            SELECT 1, 2 UNION ALL 
                            SELECT 2, 2 UNION ALL 
                            SELECT 3, 2";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                    //Заполнение таблицы Catalog
                    expression = @"INSERT INTO Catalog
                                (period, type)
                                SELECT 14, 1 UNION ALL
                                SELECT 14, 2 UNION ALL
                                SELECT 14, 3 UNION ALL
                                SELECT 15, 1 UNION ALL
                                SELECT 15, 2 UNION ALL
                                SELECT 15, 3 UNION ALL
                                SELECT 16, 1";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                    //Заполнение таблицы Price
                    expression = @"INSERT INTO Price
                                (code, pricePC, priceDC, catalog, quantity, discont)
                                SELECT 20779,   31.9,    25.5,    1,   1,   1 UNION ALL
                                SELECT 20533,   79.0,    63.18,   1,   4,   1 UNION ALL
                                SELECT 23410,   49.0,    39.18,   1,   2,   0 UNION ALL
                                SELECT 20779,   50.6,    45.15,   4,   3,   0 UNION ALL
                                SELECT 26261,   31.9,    25.5,    1,   1,   1 UNION ALL
                                SELECT 29054,   219.0,   175.2,   4,   7,   1 UNION ALL
                                SELECT 29054,   285.0,   267.1,   7,   2,   0 UNION ALL
                                SELECT 20779,   15.1,    11.5,    6,   3,   1";
                    cmd.CommandText = expression;
                    cmd.ExecuteNonQuery();

                
            }
        }

    }
}