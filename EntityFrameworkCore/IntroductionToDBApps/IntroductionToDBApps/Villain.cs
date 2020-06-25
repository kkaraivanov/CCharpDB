namespace IntroductionToDBApps
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    class Villain
    {
        public static string Remove(SqlConnection sqlConn, int villainId)
        {
            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();

            string villainName = GetVillainName(sqlConn, villainId);

            if (villainName == null)
                return "No such villain was found.";

            int minions = RemovFromMinionsVillains(sqlConn, villainId);

            if(minions == 0)
                RemoveFromVillains(sqlConn, villainId);

            return $"{villainName} was deleted." +
                   $"{Environment.NewLine}" +
                   $"{minions} minions were released.";
        }

        private static void RemoveFromVillains(SqlConnection sqlConn, int villainId)
        {
            string removeString = @"DELETE FROM Villains WHERE Id = @villainId";
            var removeCommand = new SqlCommand(removeString, sqlConn);
            removeCommand.Parameters.AddWithValue("@villainId", villainId);
            removeCommand.ExecuteNonQuery();
        }

        private static int RemovFromMinionsVillains(SqlConnection sqlConn, int villainId)
        {
            string removeString = @"DELETE FROM MinionsVillains WHERE VillainId = @villainId";
            string selectString = @"SELECT COUNT(VillainId) FROM MinionsVillains WHERE VillainId = @villainId";
            var removeCommand = new SqlCommand(removeString, sqlConn);
            var selectCommand = new SqlCommand(selectString, sqlConn);
            removeCommand.Parameters.AddWithValue("@villainId", villainId);
            selectCommand.Parameters.AddWithValue("@villainId", villainId);
            string count = selectCommand.ExecuteScalar()?.ToString();
            int minionCount = 0;

            if (count != null)
            {
                minionCount = int.Parse(count);
                removeCommand.ExecuteNonQuery();
                RemoveFromVillains(sqlConn, villainId);
            }

            return minionCount;
        }

        private static string GetVillainName(SqlConnection sqlConn, int villainId)
        {
            string sql = @"SELECT [Name] FROM Villains WHERE Id = @villianId";
            SqlCommand command = new SqlCommand(sql, sqlConn);
            command.Parameters.AddWithValue("@villianId", villainId);
            string villainName = command.ExecuteScalar()?.ToString();

            return villainName;
        }
    }
}
