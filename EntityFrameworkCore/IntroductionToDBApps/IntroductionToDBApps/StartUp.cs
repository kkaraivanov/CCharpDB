namespace IntroductionToDBApps
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    class StartUp
    {
        static void Main(string[] args)
        {
            string sqlServer = ".\\SQLKARAIVANOV";
            string baseName = "MinionsDB";
            string userName = "user";
            string password = "password";

            // 1.	Initial Setup 
            var dataBaseCreator = new DatabaseCreator(sqlServer, baseName, userName, password);
            //var dataBaseCreator = new DatabaseCreator(sqlServer, baseName);
            var dataBaseTables = TablesPattern.Tables();
            dataBaseCreator.CreateDatabase();
            dataBaseCreator.CreateTables(dataBaseTables);

            using SqlConnection sqlConn = new SqlConnection(dataBaseCreator.ConnectionString);
            foreach (var (tableName, value) in InsertInToTables())
            {
                //Insert(sqlConn, tableName, value);
            }

            // 2.	Villain Names
            //Console.WriteLine(VillainNames.GetVillainNames(sqlConn));

            // 3.	Minion Names
            //int villainId = int.Parse(Console.ReadLine());
            //Console.WriteLine(MinionNames.GetMinionsInfoAboutVillain(sqlConn, villainId));

            // 4.	Add Minion
            var minion = Console.ReadLine().Split(' ', 2).ToArray();
            var villainName = Console.ReadLine().Split(' ', 2).ToArray();

            AddMinion.ToDatabase(sqlConn, minion, villainName);
            ;
            //Console.WriteLine(AddMinion.GetVillanId(sqlConn, villainName));
        }

        private static Dictionary<string, string> InsertInToTables()
        {
            var temp = new Dictionary<string, string>();
            string tableName = "Countries";
            string value = "([Name]) " +
                           "VALUES ('Bulgaria')," +
                           "('England')," +
                           "('Cyprus')," +
                           "('Germany')," +
                           "('Norway')";
            if (!temp.ContainsKey(tableName))
                temp[tableName] = null;
            temp[tableName] = value;

            tableName = "Towns";
            value = "([Name], CountryCode) " +
                    "VALUES ('Plovdiv', 1)," +
                    "('Varna', 1)," +
                    "('Burgas', 1)," +
                    "('Sofia', 1)," +
                    "('London', 2)," +
                    "('Southampton', 2)," +
                    "('Bath', 2)," +
                    "('Liverpool', 2)," +
                    "('Berlin', 3)," +
                    "('Frankfurt', 3)," +
                    "('Oslo', 4)";
            if (!temp.ContainsKey(tableName))
                temp[tableName] = null;
            temp[tableName] = value;

            tableName = "Minions";
            value = "(Name,Age, TownId) " +
                    "VALUES('Bob', 42, 3)," +
                    "('Kevin', 1, 1)," +
                    "('Bob ', 32, 6)," +
                    "('Simon', 45, 3)," +
                    "('Cathleen', 11, 2)," +
                    "('Carry ', 50, 10)," +
                    "('Becky', 125, 5)," +
                    "('Mars', 21, 1)," +
                    "('Misho', 5, 10)," +
                    "('Zoe', 125, 5)," +
                    "('Json', 21, 1)";
            if (!temp.ContainsKey(tableName))
                temp[tableName] = null;
            temp[tableName] = value;

            tableName = "EvilnessFactors";
            value = "(Name) " +
                    "VALUES ('Super good')," +
                    "('Good')," +
                    "('Bad')," +
                    "('Evil')," +
                    "('Super evil')";
            if (!temp.ContainsKey(tableName))
                temp[tableName] = null;
            temp[tableName] = value;

            tableName = "Villains";
            value = "(Name, EvilnessFactorId) " +
                    "VALUES ('Gru',2)," +
                    "('Victor',1)," +
                    "('Jilly',3)," +
                    "('Miro',4)," +
                    "('Rosen',5)," +
                    "('Dimityr',1)," +
                    "('Dobromir',2)";
            if (!temp.ContainsKey(tableName))
                temp[tableName] = null;
            temp[tableName] = value;

            tableName = "MinionsVillains";
            value = "(MinionId, VillainId) " +
                    "VALUES (4,2)," +
                    "(1,1)," +
                    "(5,7)," +
                    "(3,5)," +
                    "(2,6)," +
                    "(11,5)," +
                    "(8,4)," +
                    "(9,7)," +
                    "(7,1)," +
                    "(1,3)," +
                    "(7,3)," +
                    "(5,3)," +
                    "(4,3)," +
                    "(1,2)," +
                    "(2,1)," +
                    "(2,7)";
            if (!temp.ContainsKey(tableName))
                temp[tableName] = null;
            temp[tableName] = value;

            return temp;
        }

        private static void Insert(SqlConnection sqlConn, string tableName, string values)
        {
            string sql = $"INSERT INTO {tableName}{values}";
            ExecuteSQLStatement(sqlConn, sql);
        }
        private static void ExecuteSQLStatement(SqlConnection sqlConn, string sql)
        {
            var command = new SqlCommand(sql, sqlConn);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException ae)
            {
                Console.WriteLine(ae.Message.ToString());
            }
        }
    }
}
