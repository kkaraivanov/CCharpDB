namespace VaporStore.DataProcessor
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
    using Dto;
    using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
    using Microsoft.EntityFrameworkCore.Internal;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			var sb = new StringBuilder();
            var gamesDto = Engine.JsonDeserializer<ImportGamesDto[]>(jsonString);
			var games = new List<Game>();
            var developers = new List<Developer>();
            var genres = new List<Genre>();
            var tags = new List<Tag>();

            foreach (var dto in gamesDto)
            {
				if (!IsValid(dto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime parseDate;
                var dateIsValid = DateTime.TryParseExact(
                    dto.ReleaseDate,
					"yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parseDate);
                if (!dateIsValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (string.IsNullOrEmpty(dto.Name))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (string.IsNullOrEmpty(dto.Developer))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (string.IsNullOrEmpty(dto.Genre))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (dto.Tags.Length == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var developer = developers.FirstOrDefault(x => x.Name == dto.Developer);
                if(developer == null)
                {
                    developer = new Developer() {Name = dto.Developer};
                    developers.Add(developer);
                }

                var genre = genres.FirstOrDefault(x => x.Name == dto.Genre);
                if (genre == null)
                {
                    genre = new Genre() { Name = dto.Genre };
                    genres.Add(genre);
                }
                var game = new Game()
                {
                    Name = dto.Name,
                    Price = decimal.Parse(dto.Price),
                    ReleaseDate = parseDate
                };

                developer.Games.Add(game);
                genre.Games.Add(game);
                game.Developer = developer;
                game.Genre = genre;

                foreach (var dtoTag in dto.Tags)
                {
                    if (string.IsNullOrWhiteSpace(dtoTag))
                    {
                        continue;
                    }

                    var tag = tags.FirstOrDefault(x => x.Name == dtoTag);
                    if (tag == null)
                    {
                        tag = new Tag() { Name = dtoTag };
                        tags.Add(tag);
                    }

                    var gameTag = new GameTag()
                    {
                        Game = game,
                        Tag = tag
                    };

                    game.GameTags.Add(gameTag);
                }

                games.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
            var sb = new StringBuilder();
            var usersDto = Engine.JsonDeserializer<ImportUsersDto[]>(jsonString);
            var users = new List<User>();
            
            foreach (var dto in usersDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var cardsIsValid = true;
                CardType type;
                foreach (var dtoCard in dto.Cards)
                {
                    if (!IsValid(dtoCard))
                    {
                        cardsIsValid = false;
                        break;
                    }

                    if (!Enum.TryParse(dtoCard.Type, out type))
                    {
                        cardsIsValid = false;
                        break;
                    }
                }

                if (!cardsIsValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User()
                {
                    FullName = dto.FullName,
                    Username = dto.Username,
                    Email = dto.Email,
                    Age = dto.Age
                };

                foreach (var cardse in dto.Cards)
                {
                    Enum.TryParse(cardse.Type, out type);
                    var card = new Card()
                    {
                        Number = cardse.Number,
                        Cvc = cardse.Cvc,
                        Type = type
                    };

                    user.Cards.Add(card);
                }

                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
            var sb = new StringBuilder();
            var root = "Purchases";
            var purchasesDto = Engine.XmlDeserializer<ImportPurchasesDto>(xmlString, root);
            var purchases = new List<Purchase>();

            foreach (var dto in purchasesDto)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime parseDate;
                var dateIsValid = DateTime.TryParseExact(
                    dto.Date,
                    "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parseDate);

                if (!dateIsValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = context.Games.FirstOrDefault(x => x.Name == dto.GameName);
                if (game == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                PurchaseType type;
                if(!Enum.TryParse(dto.TypeS, out type))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var card = context.Cards.FirstOrDefault(x => x.Number == dto.Card);
                if (card == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase()
                {
                    ProductKey = dto.ProductKey,
                    Type = type,
                    Date = parseDate,
                    Card = card,
                    Game = game
                };

                var user = context.Users.First(x => x.Cards.Any(c => c.Number == card.Number));

                purchases.Add(purchase);
                sb.AppendLine($"Imported {game.Name} for {card.User.Username}");
            }

            context.Purchases.AddRange(purchases);
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