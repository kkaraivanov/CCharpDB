namespace PetStore.Data.Configuration
{
    public static class DbContextConfiguration
    {
        public const string DatabaseName = "PetStore";
        public const string ServerName = "\\SQLKARAIVANOV";

        public static string ConnectionString =>
            $"Server=.{ServerName};Database={DatabaseName};Integrated Security=True;";
    }
}