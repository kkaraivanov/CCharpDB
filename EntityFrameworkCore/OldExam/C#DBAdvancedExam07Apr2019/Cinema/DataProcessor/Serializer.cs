namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using ExportDto;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context
                .Movies
                .Where(r => r.Rating >= rating && r.Projections.Any(t => t.Tickets.Count > 0))
                .OrderByDescending(r => r.Rating)
                .ThenByDescending(p => p.Projections.Sum(t => t.Tickets.Sum(pc => pc.Price)))
                .Select(x => new
                {
                    MovieName = x.Title,
                    Rating = x.Rating.ToString("f2"),
                    TotalIncomes = x.Projections.Sum(t => t.Tickets.Sum(p => p.Price)).ToString("F2"),
                    Customers = x.Projections.SelectMany(t => t.Tickets)
                        .Select(c => new
                        {
                            FirstName = c.Customer.FirstName,
                            LastName = c.Customer.LastName,
                            Balance = c.Customer.Balance.ToString("f2"),
                        })
                        .OrderByDescending(b => b.Balance)
                        .ThenBy(f => f.FirstName)
                        .ThenBy(l => l.LastName)
                        .ToArray()
                })
                .Take(10)
                .ToArray();
            ;
            var serializer = Engine.JsonSerializer(movies);

            return serializer;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var masterAttribut = "Customers";
            var customers = context.Customers
                .Where(x => x.Age >= age)
                .Select(x => new ExportCustomerDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SpentMoney = decimal.Parse(x.Tickets.Sum(t => t.Price).ToString("f2")),
                    SpentTime = new TimeSpan(x.Tickets
                        .Select(t => t.Projection.Movie.Duration.Ticks).Sum())
                        .ToString("hh\\:mm\\:ss")
                })
                .OrderByDescending(x => x.SpentMoney)
                .Take(10)
                .ToArray();

            var serializer = Engine.XmlSerializer<ExportCustomerDto>(customers, masterAttribut);

            return serializer;
        }
    }
}