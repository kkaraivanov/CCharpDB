namespace IntroductionToDBApps
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class VillainNames
    {
        public static string GetVillainNames(SqlConnection sqlConn)
        {
            var sb = new StringBuilder();
            string sqlString = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                   FROM Villains AS v 
                                   JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                   GROUP BY v.Id, v.Name 
                                     HAVING COUNT(mv.VillainId) > 3 
                                   ORDER BY COUNT(mv.VillainId) DESC";

            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();

            var getVillainInfo = new SqlCommand(sqlString, sqlConn);
            SqlDataReader read = getVillainInfo.ExecuteReader();

            while (read.Read())
            {
                string minionName = read["Name"].ToString();
                string minionsCount = read["MinionsCount"].ToString();

                sb.AppendLine($"{minionName} - {minionsCount}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
