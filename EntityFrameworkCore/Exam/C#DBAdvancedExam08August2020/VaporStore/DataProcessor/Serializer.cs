namespace VaporStore.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Data.Models.Enums;
    using Dto;
    using Newtonsoft.Json;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var export = context.Genres
                .Where(x => genreNames.Contains(x.Name))
                .AsEnumerable()
                .Select(x => new
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games
                        .Where(g => g.Purchases.Any())
                        .Select(g => new
                        {
                            Id = g.Id,
                            Title = g.Name,
                            Developer = g.Developer.Name,
                            Tags = string.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
                            Players = g.Purchases.Count
                        })
                        .OrderByDescending(g => g.Players)
                        .ThenBy(g => g.Id)
                        .ToArray(),
                    TotalPlayers = x.Games.Sum(g => g.Purchases.Count)
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToArray();
            ;
            var result = JsonConvert.SerializeObject(export, Formatting.Indented);
            return result;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var root = "Users";
            Enum.TryParse(storeType, out PurchaseType type);
            var users = context.Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any()))
                .Select(u => new ExportUserPurchasesByTypeDto()
                {
                    UserName = u.Username,
                    Purchases = context.Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == type)
                        .OrderBy(p => p.Date)
                        .Select(p => new ExportPurchase()
                        {
                            CardNumber = p.Card.Number,
                            CardCvc = p.Card.Cvc,
                            CardDate = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGame()
                            {
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .ToArray(),
                    TotalSpent = context.Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == type)
                        .Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Length > 0)
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.UserName)
                .ToArray();
            
            var result = Engine.XmlSerializer<ExportUserPurchasesByTypeDto[]>(users, root);
            
            return result;
        }
    }
}