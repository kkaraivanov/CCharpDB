namespace IntroductionToDBApps
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class ChangeTownNames
    {
        private static StringBuilder sb = new StringBuilder();

        public static string Change(SqlConnection sqlConn, string countryName)
        {
            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();

            string countryId = GetCountryId(sqlConn, countryName);

            if (countryId == null)
                return sb.ToString().TrimEnd();

            var towns = GetTowns(sqlConn, countryId);
            sb.AppendLine($"{towns.Count} town names were affected. ");
            foreach (var town in towns)
            {
                string townId = GetTownId(sqlConn, town);
                UpdateTable(sqlConn, town.ToUpper(), townId, countryId);
            }

            sb.AppendLine($"[{string.Join(", ", towns).ToUpper()}]");
            return sb.ToString().TrimEnd();
        }

        private static void UpdateTable(SqlConnection sqlConn, string townName, string townId, string countryId)
        {
            

            string updateString = @"UPDATE Towns SET Name = @townName WHERE Id = @townId AND CountryCode = @countryId";
            var updateCommand = new SqlCommand(updateString, sqlConn);
            updateCommand.Parameters.AddWithValue("@townName", townName);
            updateCommand.Parameters.AddWithValue("@townId", townId);
            updateCommand.Parameters.AddWithValue("@countryId", countryId);

            updateCommand.ExecuteNonQuery();
        }

        private static List<string> GetTowns(SqlConnection sqlConn, string countryId)
        {
            var result = new List<string>();
            string selectString = @"SELECT [Name] FROM Towns
                                    WHERE CountryCode = @countryId";
            var selectCommand = new SqlCommand(selectString, sqlConn);
            selectCommand.Parameters.AddWithValue("@countryId", countryId);
            SqlDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                string townName = reader["Name"].ToString();
                result.Add(townName);
            }

            return result;
        }

        private static string GetTownId(SqlConnection sqlConn, string townName)
        {
            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();

            string selectString = @"SELECT Id FROM Towns
                                    WHERE [Name] = @townName";
            var selectCommand = new SqlCommand(selectString, sqlConn);
            selectCommand.Parameters.AddWithValue("@townName", townName);
            string getTownId = selectCommand.ExecuteScalar()?.ToString();

            return getTownId;
        }

        private static string GetCountryId(SqlConnection sqlConn, string countryName)
        {
            string selectString = @"SELECT Id FROM Countries
                                    WHERE [Name] = @countryName";
            var selectCommand = new SqlCommand(selectString, sqlConn);
            selectCommand.Parameters.AddWithValue("@countryName", countryName);
            string getCountryId = selectCommand.ExecuteScalar()?.ToString();

            if (getCountryId == null)
                sb.AppendLine("No town names were affected.");
            
            return getCountryId;
        }
    }
}
