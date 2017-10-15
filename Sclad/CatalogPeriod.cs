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
    static class CatalogPeriod
    {
        public static List<CatalogPeriodOne> catalogPeriod;

        // Выбираем две таблицы C_period,C_p_year и создаем список существующих каталогов
        public static void MakeList()
        {
            catalogPeriod = new List<CatalogPeriodOne>();

            using (SqlCeConnection connection = new SqlCeConnection(DataBase.ConStrDB))
            {
                string sql = @"SELECT C_period.id, C_period.number, C_p_year.id, C_p_year.year
                                FROM C_period, C_p_year
                                WHERE C_period.year = C_p_year.id
                                ORDER BY C_p_year.year, C_period.number";

                connection.Open();
                SqlCeCommand command = new SqlCeCommand(sql, connection);
                SqlCeDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CatalogPeriodOne catalogPeriodOne = new CatalogPeriodOne();

                    catalogPeriodOne.PeriodId = (int)reader[0];
                    catalogPeriodOne.Number = (int)reader[1];
                    catalogPeriodOne.YearId = (int)reader[2];
                    catalogPeriodOne.Year = (int)reader[3];

                    catalogPeriod.Add(catalogPeriodOne);
                }
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

                //Заполнение таблицы C_p_type
                string expression = @"INSERT INTO C_p_year
                                    (year)
                                    SELECT 2016 UNION ALL
                                    SELECT 2017 UNION ALL
                                    SELECT 2018 UNION ALL
                                    SELECT 2019 UNION ALL
                                    SELECT 2020";
                SqlCeCommand cmd = new SqlCeCommand(expression, connection);
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
            }
        }

        /// <summary>
        /// Поиск каталожного периода в коллекции периодов
        /// </summary>
        /// <param name="periodTEXT"></param>
        /// <returns></returns>
        public static bool SearchSuchCatalog(string periodTEXT)
        {
            foreach (CatalogPeriodOne item in CatalogPeriod.catalogPeriod)
            {
                if (item.CatalogPeriodText == periodTEXT)
                {
                    return true;
                }
            }
            return false;
        }
    }


    struct CatalogPeriodOne
    {
        public int PeriodId { get; set; }
        public int Number { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public string CatalogPeriodText
        {
            get
            {
                return SkladBase.ConvertPeriodToText(Number, Year);
            }
        }
    }

}
