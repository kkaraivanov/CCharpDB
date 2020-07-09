namespace P03_FootballBetting
{
    using System;
    using Data;

    public class StartUp
    {
        static void Main()
        {
            var dbContext = new FootballBettingContext();
            dbContext.Database.EnsureCreated();


        }
    }
}
