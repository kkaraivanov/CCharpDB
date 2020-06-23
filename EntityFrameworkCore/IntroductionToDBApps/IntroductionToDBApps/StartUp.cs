namespace IntroductionToDBApps
{
    using System;
    using System.Data;
    using Microsoft.Data.SqlClient;

    class StartUp
    {
        private static string connectionString = "Server=.\\SQLKARAIVANOV; Database=MinionsDB; User Id = user; Password=password";
        private static SqlDataReader reader = null;
        private static SqlConnection conn = null;
        private static SqlCommand command = null;

        static void Main(string[] args)
        {
            CreateDB();
            CreateDbTables();

            
        }

        private static void ExecuteSQLStatement(string sql)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();

            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                command = new SqlCommand(sql, conn);

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

        static void CreateDbTables()
        {
            string tableName = null;
            string filds = null;

            tableName = "Countries";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(50)";
            CreateTable(tableName, filds);

            tableName = "Towns";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(50)," +
                    "CountryCode INT FOREIGN KEY REFERENCES Countries(Id)";
            CreateTable(tableName, filds);

            tableName = "Minions";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(30)," +
                    "Age INT," +
                    "TownId INT FOREIGN KEY REFERENCES Towns(Id)";
            CreateTable(tableName, filds);

            tableName = "EvilnessFactors";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(50)";
            CreateTable(tableName, filds);

            tableName = "Villains";
            filds = "Id INT PRIMARY KEY IDENTITY," +
                    "Name VARCHAR(50)," +
                    "EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id)";
            CreateTable(tableName, filds);

            tableName = "MinionsVillains";
            filds = "MinionId INT FOREIGN KEY REFERENCES Minions(Id)," +
                    "VillainId INT FOREIGN KEY REFERENCES Villains(Id)," +
                    "CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId)";
            CreateTable(tableName, filds);

            Console.WriteLine("All tables is Created");
        }

        private static void CreateTable(string tableName, string filds)
        {
            string tableString = $"CREATE TABLE {tableName}({filds})";
            ExecuteSQLStatement(tableString);
        }

        private static void CreateDB()
        {
            string conString = "Server=.\\SQLKARAIVANOV; Database=master; User Id = user; Password=password";
            string dbCheck = "SELECT top(1) name FROM master.dbo.sysdatabases where name='MinionsDB'";
            string sqlCreate = "CREATE DATABASE MinionsDB";
            bool DbNotExist = false;

            using (conn = new SqlConnection(conString))
            {
                conn.Open();
                command = new SqlCommand(dbCheck, conn);
                reader = command.ExecuteReader();
                string str = null;

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        str = (string) reader[i];
                    }
                }

                if (str == null)
                    DbNotExist = true;
            }

            if (DbNotExist)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    command = new SqlCommand(sqlCreate, conn);

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
            else
                return;
        }
    }
}
