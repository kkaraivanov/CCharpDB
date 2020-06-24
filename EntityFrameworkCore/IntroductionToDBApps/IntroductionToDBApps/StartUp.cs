namespace IntroductionToDBApps
{
    using System;
    using System.Collections.Generic;

    class StartUp
    {
        static void Main(string[] args)
        {
            var dataBaseCreator = new DatabaseCreator(".\\SQLKARAIVANOV", "MinionsDB","user", "password");
            var dataBaseTables = TablesPattern.Tables();
            dataBaseCreator.CreateDatabase();
            dataBaseCreator.CreateTables(dataBaseTables);
            

        }

        
    }
}
