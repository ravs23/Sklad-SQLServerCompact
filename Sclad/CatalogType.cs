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

    static class CatalogType
    {
        public static List<string> catalogType;

        public static void MakeList()
        {
            FillDBCatalogType();

            catalogType = new List<string>()
            {
                "Основной",
                "Бизнес Класс",
                "Распродажа",
                "Акционный"
            };
        }

        // если в БД таблица пустая - записать в неё типы каталогов
        static void FillDBCatalogType()
        {
            if (CheckTableCatalogType())
            {
                using (SqlCeConnection connection = new SqlCeConnection(DataBase.ConStrDB))
                {
                    connection.Open();
                    string expression = @"INSERT INTO C_type
                                (type)
                                SELECT 'Основной' UNION ALL
                                SELECT 'Бизнес Класс' UNION ALL
                                SELECT 'Распродажа' UNION ALL
                                SELECT 'Акционный'";
                    SqlCeCommand cmd = new SqlCeCommand(expression, connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // проверяем БД на наличие в таблице C_type записей
        static bool CheckTableCatalogType()
        {
            using (SqlCeConnection connection = new SqlCeConnection(DataBase.ConStrDB))
            {
                connection.Open();
                string expression = @"SELECT COUNT(*) FROM C_type";
                SqlCeCommand cmd = new SqlCeCommand(expression, connection);
                int count = (int)cmd.ExecuteScalar();
                return count == 0;
            }
        }


    }
}