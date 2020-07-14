namespace P03_SalesDatabase.Data.Managment
{
    using System;
    using Contracts;

    public class ConsoleWriter : IWriter
    {
        public void Write(string str) => Console.Write(str);

        public void WriteLine(string str) => Console.WriteLine(str);
    }
}