namespace IntroductionToDBApps
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class DatabaseCreator
    {
        /// <summary>
        /// private filds used in DatabaseCreator Class
        /// </summary>
        private SqlDataReader reader = null;
        private SqlConnection conn = null;
        private SqlCommand command = null;
        private string userName = null;
        private string password = null;
        private string sqlServer = null;
        private string dataBase = null;
        private string connectionString = null;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="sqlServer">This is server name for example - localhost\serverName if there exists</param>
        /// <param name="dataBase">This is database name</param>
        public DatabaseCreator(string sqlServer, string dataBase)
        {
            this.sqlServer = sqlServer;
            this.dataBase = dataBase;
            this.connectionString = $"Server={sqlServer}; Database=master; Integrated Security=true";
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="sqlServer">This is server name for example - localhost\serverName if there exists</param>
        /// <param name="dataBase">This is database name</param>
        /// <param name="userName">user name for access to MSSQL Server</param>
        /// <param name="password">password for access to MSSQL Server if there exists</param>
        public DatabaseCreator(string sqlServer, string dataBase, string userName, string password)
            :this(sqlServer, dataBase)
        {
            this.userName = userName;
            this.password = password;
            this.connectionString = $"Server={sqlServer}; Database=master; User Id={userName}; Password={password}";
        }

        /// <summary>
        /// Return Server name from fild
        /// </summary>
        public string SqlServer => this.sqlServer;

        /// <summary>
        /// Return Database name from fild
        /// </summary>
        public string BaseName => this.dataBase;

        /// <summary>
        /// Return Database connection string
        /// </summary>
        public string ConnectionString => this.connectionString;

        /// <summary>
        /// Database creation method
        /// </summary>
        public void CreateDatabase()
        {
            string sqlCreate = $"CREATE DATABASE {this.dataBase}";
            
            if (!DataBaseExist())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                using (conn = new SqlConnection(this.connectionString))
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
            if (DataBaseExist())
                this.connectionString = 
                    this.userName == null ? $"Server={this.sqlServer}; Database={this.BaseName}; Integrated Security=true" :
                    $"Server={this.sqlServer}; Database={this.BaseName}; User Id={this.userName}; Password={this.password}";
        }

        /// <summary>
        /// Tables creation method
        /// </summary>
        /// <param name="tablesColection">Collection with table structure</param>
        public void CreateTables(Dictionary<string, string> tablesColection)
        {
            if (DataBaseExist())
            {
                if (tablesColection.Count > 0)
                {
                    foreach (var (tableName, fildValue) in tablesColection)
                    {
                        if(TableNameExist(tableName))
                            continue;

                        CreateTable(tableName, fildValue);
                    }
                }
            }
        }

        private bool TableNameExist(string tableName)
        {
            string checkTable = $"Select TOP(1) TABLE_NAME " +
                                $"From INFORMATION_SCHEMA.COLUMNS " +
                                $"WHERE TABLE_NAME = '{tableName}'";
            bool result = false;

            using (conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                var currentCommand = new SqlCommand(checkTable, conn);
                var currentReader = currentCommand.ExecuteReader();
                string dbName = null;

                while (currentReader.Read())
                {
                    for (int i = 0; i < currentReader.FieldCount; i++)
                    {
                        dbName = (string)currentReader[i];
                    }
                }

                if (dbName != null)
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// The method checks if the database exists
        /// </summary>
        /// <returns>True if it exists. False if it does not exist.</returns>
        private bool DataBaseExist()
        {
            string connectionStr = this.userName == null ? $"Server={sqlServer}; Database=master; Integrated Security=true" : 
                $"Server={this.sqlServer}; Database=master; User Id = {this.userName}; Password={this.password}";
            string checkDatabaseName = $"SELECT top(1) name FROM master.dbo.sysdatabases where name='{this.dataBase}'";
            bool result = false;

            using (conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                var currentCommand = new SqlCommand(checkDatabaseName, conn);
                var currentReader = currentCommand.ExecuteReader();
                string dbName = null;

                while (currentReader.Read())
                {
                    for (int i = 0; i < currentReader.FieldCount; i++)
                    {
                        dbName = (string)currentReader[i];
                    }
                }

                if (dbName != null)
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Performs connection with MSSQL Server
        /// </summary>
        /// <param name="sql">Query to the server</param>
        private void ExecuteSQLStatement(string sql)
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

        /// <summary>
        /// Auxiliary method
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filds"></param>
        private void CreateTable(string tableName, string filds)
        {
            string tableString = $"CREATE TABLE {tableName}({filds})";
            ExecuteSQLStatement(tableString);
        }
    }
}
