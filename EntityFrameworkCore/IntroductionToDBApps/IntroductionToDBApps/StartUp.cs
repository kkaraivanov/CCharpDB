namespace IntroductionToDBApps
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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
            Console.WriteLine(VillainNames.GetVillainNames(sqlConn));

            // 3.	Minion Names
            int villanId = int.Parse(Console.ReadLine());
            Console.WriteLine(MinionNames.GetMinionsInfoAboutVillain(sqlConn, villanId));

            // 4.	Add Minion
            var minion = Console.ReadLine().Split(' ', 2).ToArray();
            var villainName = Console.ReadLine().Split(' ', 2).ToArray();

            Console.WriteLine(AddMinion.ToDatabase(sqlConn, minion, villainName));

            // 5.	Change Town Names Casing
            string countryName = Console.ReadLine();
            Console.WriteLine(ChangeTownNames.Change(sqlConn, countryName));

            // 6.	*Remove Villain 
            int villainId = int.Parse(Console.ReadLine());
            Console.WriteLine(Villain.Remove(sqlConn, villainId));

            // 7.	Print All Minion Names
            Console.WriteLine(PrintMinion.PrintNames(sqlConn));

            // 8.	Increase Minion Age
            var minionsId = Console.ReadLine()
                .Split().Select(int.Parse)
                .ToArray();
            for (int i = 0; i < minionsId.Length; i++)
            {
                UpdateMinions(sqlConn, minionsId[i]);
            }

            Console.WriteLine(DisplayMinions(sqlConn).ToString().TrimEnd());

            // 9.	Increase Age Stored Procedure 
            int minionId = int.Parse(Console.ReadLine());
            RunProcedure(sqlConn, minionId);
        }

        #region Problem 1

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

        #endregion

        #region Problem 8

        private static StringBuilder DisplayMinions(SqlConnection sqlConn)
        {
            string selectString = @"SELECT Name, Age FROM Minions";
            var selectCommand = new SqlCommand(selectString, sqlConn);
            SqlDataReader readr = selectCommand.ExecuteReader();
            var sb = new StringBuilder();

            while (readr.Read())
            {
                string name = readr["Name"].ToString();
                string age = readr["Age"].ToString();

                sb.AppendLine($"{name} {age}");
            }

            return sb;
        }

        private static void UpdateMinions(SqlConnection sqlConn, int minionId)
        {
            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();

            string updateString = @"UPDATE Minions 
                                    SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1 
                                    WHERE Id = @Id";
            var updateCommand = new SqlCommand(updateString, sqlConn);
            updateCommand.Parameters.AddWithValue("@Id", minionId);
            updateCommand.ExecuteNonQuery();
        }

        #endregion

        #region Problem 9

        private static void RunProcedure(SqlConnection sqlConn, int minionId)
        {
            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();

            string updateString = @"usp_GetOlder";
            var updateCommand = new SqlCommand(updateString, sqlConn);
            updateCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter param;
            param = updateCommand.Parameters.Add("@Id", SqlDbType.Int);
            param.Value = minionId;
            updateCommand.ExecuteNonQuery();

            string selectString = @"SELECT Name, Age FROM Minions WHERE Id = @Id";
            var selectCommand = new SqlCommand(selectString, sqlConn);
            selectCommand.Parameters.AddWithValue("@Id", minionId);
            SqlDataReader readMinion = selectCommand.ExecuteReader();

            while (readMinion.Read())
            {
                string minionName = readMinion["Name"].ToString();
                string minionAge = readMinion["Age"].ToString();

                Console.WriteLine($"{minionName} – {minionAge} years old");
            }
        }

        #endregion

        
    }
}
