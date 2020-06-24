namespace IntroductionToDBApps
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class AddMinion
    {
        public static void ToDatabase(SqlConnection sqlConn, string[] minion, string[] villainName)
        {
            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();
            string minionName = minion[1].Split()[0];
            string minionAge = minion[1].Split()[1];
            string townName = minion[1].Split()[2];
            string villanName = villainName[1];
            string villanId = GetVillanId(sqlConn, villanName);
            string minionId = GetMinionId(sqlConn, minionName);
            string townId = GetTownId(sqlConn, townName);

            if (villanId != null)
                Console.WriteLine(villanId);
            if (minionId != null)
                Console.WriteLine(minionId);
            if (townId != null)
                Console.WriteLine(townId);
        }
        private static string GetVillanId(SqlConnection sqlConn, string villainName)
        {
            string getVillanId = @"SELECT Id FROM Villains WHERE Name = @Name";
            var sqlCommand = new SqlCommand(getVillanId, sqlConn);
            sqlCommand.Parameters.AddWithValue("@Name", villainName);
            string villanId = sqlCommand.ExecuteScalar().ToString();

            return villanId;
        }

        private static string GetMinionId(SqlConnection sqlConn, string minionName)
        {
            string getVillanId = @"SELECT Id FROM Minions WHERE Name = @Name";
            var sqlCommand = new SqlCommand(getVillanId, sqlConn);
            sqlCommand.Parameters.AddWithValue("@Name", minionName);
            string minionId = sqlCommand.ExecuteScalar().ToString();

            return minionId;
        }

        private static string GetTownId(SqlConnection sqlConn, string townName)
        {
            string getVillanId = @"SELECT Id FROM Towns WHERE Name = @Name";
            var sqlCommand = new SqlCommand(getVillanId, sqlConn);
            sqlCommand.Parameters.AddWithValue("@Name", townName);
            string townId = sqlCommand.ExecuteScalar().ToString();

            return townId;
        }
    }
}
