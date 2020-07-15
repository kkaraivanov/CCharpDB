namespace BookShop.Initializer.Managment
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Models.Enums;

    public static class InputReader
    {
        /// <summary>
        /// 2. Age Restriction
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <param name="command"></param>
        /// <returns>that have age restriction</returns>
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            Models.Enums.AgeRestriction restriction;
            var commandPars = command.ToLower().First().ToString().ToUpper() + command.Substring(1).ToLower();
            if (!Enum.TryParse(commandPars, out restriction))
                throw new ArgumentException("Invalid input argument");

            var sb = new StringBuilder();
            var titles = context
                .Books
                .Where(x => x.AgeRestriction == restriction)
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 3. Golden Books
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <returns>titles of the golden edition books that have less than 5000 copies</returns>
        public static string GetGoldenBooks(BookShopContext context)
        {
            var sb = new StringBuilder();
            var titles = context
                .Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 4. Books by Price
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <returns>all titles and prices of books with price higher than 40</returns>
        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();
            var books = context
                .Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => new { x.Title, x.Price })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}