namespace MiniORM
{
    using Data;

    class StartUp
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=.\\SQLKARAIVANOV;Database=master;Integrated Security=true";
            var dbContext = new MiniORMDbContext(connectionString);

            ;
        }
    }
}
