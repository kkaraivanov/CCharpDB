namespace PetStore.Data.Configuration
{
    using System.IO;
    using Newtonsoft.Json;

    public static class DbContextConfiguration
    {
        private static string DatabaseName = "PetStore";
        private static string ServerName = "";

        public static string ConnectionString =>
            $"Server=.\\{ServerName};Database={DatabaseName};Integrated Security=True;";
    }
}