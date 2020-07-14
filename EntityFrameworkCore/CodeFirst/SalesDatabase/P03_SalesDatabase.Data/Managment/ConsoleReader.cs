namespace P03_SalesDatabase.Data.Managment
{
    using System;
    using Contracts;

    public class ConsoleReader : IReader
    {
        public string ReadLine() => Console.ReadLine();
    }
}