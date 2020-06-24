namespace IntroductionToDBApps
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class MinionNames
    {
        public static string GetMinionsInfoAboutVillain(SqlConnection sqlConn, int villainId)
        {
            string sql = @"SELECT [Name] FROM Villains WHERE Id = @villianId";
            var sb = new StringBuilder();

            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();

            SqlCommand command = new SqlCommand(sql, sqlConn);
            command.Parameters.AddWithValue("@villianId", villainId);
            string villainName = GetVillainName(sqlConn, villainId);

            if (villainName == null)
                sb.AppendLine($"No villain with ID {villainId} exists in the database.");
            else
            {
                sb.AppendLine($"Villain: {villainName}");
                string readVillain = @"SELECT m.[Name], m.Age FROM Villains v
                                     LEFT JOIN MinionsVillains mv ON v.Id = mv.VillainId
                                     LEFT JOIN Minions m ON mv.MinionId = m.Id
                                     WHERE v.[Name] = @villianName
                                     ORDER BY m.[Name]";
                var getMinionInfo = new SqlCommand(readVillain, sqlConn);
                getMinionInfo.Parameters.AddWithValue("@villianName", villainName);
                SqlDataReader read = getMinionInfo.ExecuteReader();

                int row = 1;
                while (read.Read())
                {
                    string minionName = read["Name"].ToString();
                    string minionAge = read["Age"].ToString();

                    if (minionName == "" && minionAge == "")
                        sb.AppendLine("(no minions)");
                    else
                    {
                        sb.AppendLine($"{row}. {minionName} {minionAge}");
                        row++;
                    }
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static string GetVillainName(SqlConnection sqlConn, int villainId)
        {
            string sql = @"SELECT [Name] FROM Villains WHERE Id = @villianId";
            var sb = new StringBuilder();
            SqlCommand command = new SqlCommand(sql, sqlConn);
            command.Parameters.AddWithValue("@villianId", villainId);
            string villainName = command.ExecuteScalar()?.ToString();

            return villainName;
        }
    }
}
