namespace IntroductionToDBApps
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    class PrintMinion
    {
        private static List<string> Names = new List<string>();

        public static string PrintNames(SqlConnection sqlConn)
        {
            if (sqlConn.State == ConnectionState.Open)
                sqlConn.Close();
            sqlConn.Open();

            MinionsColection(sqlConn);
            var sb = new StringBuilder();

            foreach (var name in Names)
            {
                sb.AppendLine(name.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        private static void MinionsColection(SqlConnection sqlConn)
        {
            var names = new List<string>();
            string selectString = @"SELECT Name FROM Minions";
            var selectCommand = new SqlCommand(selectString, sqlConn);
            SqlDataReader reader = selectCommand.ExecuteReader();
            
            while (reader.Read())
            {
                string name = reader["Name"].ToString();
                names.Add(name);
            }

            var first = new Queue<string>(names);
            var last = new Stack<string>(names);
            for (int i = 0; i < names.Count; i++)
            {
                if(i % 2 == 0)
                    Names.Add(first.Dequeue());
                else
                    Names.Add(last.Pop());
            }
        }
    }
}
