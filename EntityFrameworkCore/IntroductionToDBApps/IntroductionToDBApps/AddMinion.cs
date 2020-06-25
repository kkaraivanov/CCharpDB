namespace IntroductionToDBApps
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class AddMinion
    {
        private static StringBuilder printResult = new StringBuilder();
        public static string ToDatabase(SqlConnection sqlConn, string[] minion, string[] villainName)
        {
            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();
            string minionName = minion[1].Split()[0];
            string minionAge = minion[1].Split()[1];
            string townName = minion[1].Split()[2];

            if (CanCreateRecord(sqlConn, minionName, minionAge, villainName[1]))
            {
                string townId = GetTownId(sqlConn, townName);
                string villainId = GetVillanId(sqlConn, villainName[1]);
                string minionId = GetMinionId(sqlConn, minionName, minionAge, townId);

                InsertMinionsVillains(sqlConn, minionName, minionId, villainId, villainName[1]);
                return printResult.ToString().TrimEnd();
            }

            return "Error!";
        }

        private static void InsertMinionsVillains(
            SqlConnection sqlConn,
            string minionName, 
            string minionId,
            string villainId,
            string villainName)
        {
            string insertMinionsVillainsString = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";
            var insertMinionsVillainsCommand = new SqlCommand(insertMinionsVillainsString, sqlConn);
            insertMinionsVillainsCommand.Parameters.AddWithValue("@minionId", minionId);
            insertMinionsVillainsCommand.Parameters.AddWithValue("@villainId", villainId);
            insertMinionsVillainsCommand.ExecuteNonQuery();

            printResult.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");
        }

        private static string GetVillanId(SqlConnection sqlConn, string villainName)
        {
            string getVillanIdString = @"SELECT Id FROM Villains WHERE Name = @name";
            var sqlCommand = new SqlCommand(getVillanIdString, sqlConn);
            sqlCommand.Parameters.AddWithValue("@name", villainName);
            string villanId = sqlCommand.ExecuteScalar()?.ToString();

            if (villanId == null)
            {
                string evilnessFactor = @"SELECT Id FROM EvilnessFactors WHERE Name = 'Evil'";
                var factorCommand = new SqlCommand(evilnessFactor, sqlConn);
                string factorId = factorCommand.ExecuteScalar()?.ToString();
                string insertString = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, @factorId)";
                var insertCommand = new SqlCommand(insertString, sqlConn);
                insertCommand.Parameters.AddWithValue("@villainName", villainName);
                insertCommand.Parameters.AddWithValue("@factorId", factorId);
                insertCommand.ExecuteNonQuery();
                villanId = sqlCommand.ExecuteScalar()?.ToString();

                printResult.AppendLine($"Villain {villainName} was added to the database.");
            }

            return villanId;
        }

        private static string GetMinionId(SqlConnection sqlConn, string minionName, string minionAge, string townId)
        {
            string getMinionIdString = @"SELECT Id FROM Minions WHERE Name = @name AND Age = @minionAge";
            var sqlCommand = new SqlCommand(getMinionIdString, sqlConn);
            sqlCommand.Parameters.AddWithValue("@name", minionName);
            sqlCommand.Parameters.AddWithValue("@minionAge", minionAge);
            string minionId = sqlCommand.ExecuteScalar()?.ToString();

            if (minionId == null)
            {
                InsertMinion(sqlConn, minionName, minionAge, townId);
                minionId = sqlCommand.ExecuteScalar()?.ToString();
            }

            return minionId;
        }

        private static void InsertMinion(
            SqlConnection sqlConn,
            string minionName,
            string minionAge,
            string townId)
        {
            string insertMinionString = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @minionAge, @townId)";
            var insertMinionCommand = new SqlCommand(insertMinionString, sqlConn);
            insertMinionCommand.Parameters.AddWithValue("@name", minionName);
            insertMinionCommand.Parameters.AddWithValue("@minionAge", minionAge);
            insertMinionCommand.Parameters.AddWithValue("@townId", townId);
            insertMinionCommand.ExecuteNonQuery();
        }

        private static string GetTownId(SqlConnection sqlConn, string townName)
        {
            string getTownsIdString = @"SELECT Id FROM Towns WHERE Name = @Name";
            var sqlCommand = new SqlCommand(getTownsIdString, sqlConn);
            sqlCommand.Parameters.AddWithValue("@Name", townName);
            string townId = sqlCommand.ExecuteScalar()?.ToString();

            if (townId == null)
            {
                string insertString = @"INSERT INTO Towns (Name) VALUES (@townName)";
                var insertCommand = new SqlCommand(insertString, sqlConn);
                insertCommand.Parameters.AddWithValue("@townName", townName);
                insertCommand.ExecuteNonQuery();
                townId = sqlCommand.ExecuteScalar()?.ToString();

                printResult.AppendLine($"Town {townName} was added to the database.");
            }

            return townId;
        }

        static bool CanCreateRecord(SqlConnection sqlConn, string minionName, string minionAge, string villainName)
        {
            bool result = false;

            string getMinionIdString = @"SELECT Id FROM Minions WHERE Name = @name AND Age = @minionAge";
            var minionCommand = new SqlCommand(getMinionIdString, sqlConn);
            minionCommand.Parameters.AddWithValue("@name", minionName);
            minionCommand.Parameters.AddWithValue("@minionAge", minionAge);
            string minionId = minionCommand.ExecuteScalar()?.ToString();

            string getVillainIdString = @"SELECT Id FROM Villains WHERE Name = @name";
            var villainCommand = new SqlCommand(getVillainIdString, sqlConn);
            villainCommand.Parameters.AddWithValue("@name", villainName);
            string villainId = villainCommand.ExecuteScalar()?.ToString();

            if (minionId == null || villainId == null)
                result = true;
            
            if (minionId != null && villainId != null)
            {
                string minionsVillainsString = @"SELECT ROW_NUMBER() OVER(ORDER BY MinionId, VillainId) 
                                                FROM MinionsVillains
                                                WHERE MinionId = @minionId AND VillainId = @villainId";
                var sqlCommand = new SqlCommand(minionsVillainsString, sqlConn);
                sqlCommand.Parameters.AddWithValue("@minionId", minionId);
                sqlCommand.Parameters.AddWithValue("@villainId", villainId);
                string getMinionsVillainsInfo = sqlCommand.ExecuteScalar()?.ToString();

                if (getMinionsVillainsInfo == null)
                    result = true;
            }

            return result;
        }
    }
}
