﻿namespace CarDealer.Data
{
    public static class DbContextConfiguration
    {
        public static string DatabaseName => "CarDealer";
        public static string ConnectionString =>
            $"Server=.\\SQLKARAIVANOV;Database={DatabaseName};Integrated Security=True;";
    }
}