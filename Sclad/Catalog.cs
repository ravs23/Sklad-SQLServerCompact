using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad
{
    static class Catalog
    {
        public static List<CatalogOne> catalog;

        // Выбираем таблицу Catalog и создаем список существующих каталогов и типов
        public static void MakeList()
        {

            catalog = new List<CatalogOne>();

            using (SqlCeConnection connection = new SqlCeConnection(DataBase.ConStrDB))
            {
                string sql = @"SELECT Catalog.id, Catalog.period, Catalog.type
                               FROM Catalog";
                //
                SqlCeDataAdapter adapter = new SqlCeDataAdapter(sql, connection);

                connection.Open();
                SqlCeCommand command = new SqlCeCommand(sql, connection);
                SqlCeDataReader reader = command.ExecuteResultSet(ResultSetOptions.Scrollable);

                //if (reader.HasRows)
                //{
                    while (reader.Read())
                    {
                        CatalogOne catalogOne = new CatalogOne();

                        catalogOne.Id = (int)reader[0];
                        catalogOne.Period = (int)reader[1];
                        catalogOne.Type = (int)reader[2];

                        catalog.Add(catalogOne);
                    }
                //}

                reader.Close();

            }
        }

        // Искуственный фил во время разработки. В рабочей программе не используем этот метод,
        // так как программа поставляется с пустыми таблицами, заполняемыми в этом методе
        public static void FillDBCatalog()
        {
            using (SqlCeConnection connection = new SqlCeConnection(DataBase.ConStrDB))
            {
                connection.Open();

                //Заполнение таблицы Catalog
                string expression = @"INSERT INTO Catalog
                                    (period, type)
                                    SELECT 14, 1 UNION ALL
                                    SELECT 14, 2 UNION ALL
                                    SELECT 14, 3 UNION ALL
                                    SELECT 15, 1 UNION ALL
                                    SELECT 15, 2 UNION ALL
                                    SELECT 15, 3 UNION ALL
                                    SELECT 16, 1";
                SqlCeCommand cmd = new SqlCeCommand(expression, connection);
                cmd.ExecuteNonQuery();
            }
        }
    }


    struct CatalogOne
    {
        public int Id { get; set; }
        public int Period { get; set; }
        public int Type { get; set; }
    }
}
