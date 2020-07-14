namespace P03_SalesDatabase.Data.Configuration
{
    public static class DbContextConfiguration
    {
        public static string ConnectionString =>
            "Server=.\\SQLKARAIVANOV;Database=Sales;Integrated Security=True;";
    }
}