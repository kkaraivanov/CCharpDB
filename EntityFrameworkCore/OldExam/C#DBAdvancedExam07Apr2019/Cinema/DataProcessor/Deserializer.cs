namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var movieDto = Engine.JsonDeserializer<ImportMoviesDto[]>(jsonString);
            var movies = new List<Movie>();
            var sb = new StringBuilder();

            foreach (var dto in movieDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (movies.Any(x => x.Title == dto.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Genre genre;
                var genreIsValid = Enum.TryParse(dto.Genre, out genre);

                if (!genreIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = new Movie()
                {
                    Title = dto.Title,
                    Genre = (Genre)genre,
                    Duration = dto.Duration,
                    Rating = dto.Rating,
                    Director = dto.Director
                };
                
                movies.Add(movie);
                var result = string.Format(SuccessfulImportMovie, movie.Title, movie.Genre, movie.Rating.ToString("f2"));
                sb.AppendLine(result);
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallDto = Engine.JsonDeserializer<ImportHallSeatsDto[]>(jsonString);
            var halls = new List<Hall>();
            var sb = new StringBuilder();

            foreach (var dto in hallDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall()
                {
                    Name = dto.Name,
                    Is4Dx = dto.Is4Dx,
                    Is3D = dto.Is3D
                };

                for (int i = 0; i < dto.Seats; i++)
                {
                    var seats = new Seat()
                    {
                        Hall = hall
                    };

                    hall.Seats.Add(seats);
                }

                halls.Add(hall);
                string hallType =
                    hall.Is4Dx && hall.Is3D ? "4Dx/3D" : 
                    !hall.Is3D && !hall.Is4Dx ? "Normal" : 
                    hall.Is3D ? "3D" : "4Dx";
                var result = string.Format(SuccessfulImportHallSeat, hall.Name, hallType, hall.Seats.Count);
                sb.AppendLine(result);
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var projectionDto = Engine.XmlDeserializer<ImportProjectionsDto>(xmlString, "Projections");
            var projections = new List<Projection>();
            var sb = new StringBuilder();

            foreach (var dto in projectionDto)
            {
                var movieExist = context.Movies.Any(x => x.Id == dto.MovieId);
                var hallExist = context.Halls.Any(x => x.Id == dto.HallId);

                if (!movieExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!hallExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                DateTime parsedDate;
                var isValidDate = DateTime.TryParseExact(
                    dto.DateTime, "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, out parsedDate);
                if (!isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var date = parsedDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                var projection = new Projection()
                {
                    Hall = context.Halls.FirstOrDefault(x => x.Id == dto.HallId),
                    Movie = context.Movies.FirstOrDefault(x => x.Id == dto.MovieId),
                    DateTime = parsedDate
                };
                
                projections.Add(projection);
                var result = string.Format(SuccessfulImportProjection, projection.Movie.Title, date);
                sb.AppendLine(result);
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var customerDto = Engine.XmlDeserializer<ImportCustomerDto>(xmlString, "Customers");
            var customers = new List<Customer>();
            var sb = new StringBuilder();

            foreach (var dto in customerDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                 
                var customer = new Customer()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    Balance = dto.Balance
                };

                var tickets = new List<Ticket>();
                var ticketsTotalPrice = 0.0m;
                foreach (var dtoTicket in dto.Tickets)
                {
                    if (!IsValid(dtoTicket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!context.Projections.Any(x => x.Id == dtoTicket.ProjectionId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var projection = context.Projections.First(x => x.Id == dtoTicket.ProjectionId);
                    var ticket = new Ticket()
                    {
                        Price = dtoTicket.Price,
                        Projection = projection,
                        Customer = customer
                    };


                    ticketsTotalPrice += ticket.Price;
                    customer.Tickets.Add(ticket);
                }

                customers.Add(customer);
                var result = string.Format(
                    SuccessfulImportCustomerTicket,
                    customer.FirstName, 
                    customer.LastName,
                    customer.Tickets.Count);
                sb.AppendLine(result);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}