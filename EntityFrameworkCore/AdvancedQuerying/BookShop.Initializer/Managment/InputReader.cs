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

        /// <summary>
        /// 5. Not Released In
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <param name="year">2000</param>
        /// <returns>Return in a single string all titles of books that are NOT released on a given year</returns>
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var sb = new StringBuilder();
            var books = context
                .Books
                .Where(x => (int)x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 6. Book Titles by Category
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <param name="input">"horror mystery drama"</param>
        /// <returns>Return in a single string the titles of books by a given list of categories</returns>
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var sb = new StringBuilder();
            var categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToList();

            var titlesOfBooks = context
                .Books
                .Where(x => x.BookCategories
                    .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            foreach (var title in titlesOfBooks)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 7. Released Before Date
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <param name="date">"12-04-1992"</param>
        /// <returns>Return the title, edition type and price of all books that are released before a given date.
        /// The date will be a string in format dd-MM-yyyy</returns>
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var sb = new StringBuilder();
            var inputDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
            var books = context
                .Books
                .Where(x => x.ReleaseDate.Value < inputDate)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new { x.Title, x.EditionType, x.Price })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 8. Author Search
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <param name="input">"dy"</param>
        /// <returns>Return the full names of authors, whose first name ends with a given string</returns>
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var sb = new StringBuilder();
            var authors = context
                .Authors
                .Where(x => x.FirstName.EndsWith(input))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Select(x => new { x.FirstName, x.LastName })
                .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 9. Book Search
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <param name="input">"WOR"</param>
        /// <returns>Return the titles of book, which contain a given string. Ignore casing</returns>
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var sb = new StringBuilder();
            var books = context
                .Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 10. Book Search by Author
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <param name="input">"R"</param>
        /// <returns>Return all titles of books and their authors’ names for books,
        /// which are written by authors whose last names start with the given string</returns>
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();
            var books = context
                .Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(x => x.BookId)
                .Select(x => new { x.Title, x.Author.FirstName, x.Author.LastName })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FirstName} {book.LastName})");
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 11. Count Books
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <param name="lengthCheck"></param>
        /// <returns>Return the number of books, which have a title longer than the number given as an input.</returns>
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Count(x => x.Title.Length > lengthCheck);
        }

        /// <summary>
        /// 12. Total Book Copies
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <returns>Return the total number of book copies for each author</returns>
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();
            var totalBooks = context
                .Authors
                .Select(x => new
                {
                    Name = $"{x.FirstName} {x.LastName}",
                    Copies = x.Books.Sum(x => x.Copies)
                })
                .OrderByDescending(x => x.Copies)
                .ToList();

            foreach (var book in totalBooks)
            {
                sb.AppendLine($"{book.Name} - {book.Copies}");
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 13. Profit by Category
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <returns>Return the total profit of all books by category.
        /// Profit for a book can be calculated by multiplying its number of copies by the price per single book. </returns>
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();
            var totalBooks = context
                .Categories
                .Select(x => new
                {
                    Categories = x.Name,
                    TotalProfit = x.CategoryBooks.Sum(x => x.Book.Copies * x.Book.Price)
                })
                .OrderByDescending(x => x.TotalProfit)
                .ThenBy(x => x.Categories)
                .ToList();

            foreach (var book in totalBooks)
            {
                sb.AppendLine($"{book.Categories} ${book.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 14. Most Recent Books
        /// </summary>
        /// <param name="context">dbContext</param>
        /// <returns>Get the most recent books by categories.</returns>
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();
            var totalBooks = context
                .Categories
                .Select(x => new
                {
                    Categories = $"--{x.Name}",
                    Books = x.CategoryBooks.Select(x => x.Book)
                        .OrderByDescending(x => x.ReleaseDate)
                        .Select(x => $"{x.Title} ({x.ReleaseDate.Value.Year})")
                        .Take(3)
                        .ToList()
                })
                .ToList();

            foreach (var book in totalBooks
                .OrderBy(x => x.Categories))
            {
                sb.AppendLine($"{book.Categories}");
                sb.AppendLine(string.Join(Environment.NewLine, book.Books));
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 15. Increase Prices
        /// </summary>
        /// <param name="context">dbContext</param>
        public static void IncreasePrices(BookShopContext context)
        {
            var allBooks = context
                .Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();
            foreach (var book in allBooks)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        /// <summary>
        /// 16. Remove Books
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Remove all books, which have less than 4200 copies.
        /// Return an int - the number of books that were deleted from the database</returns>
        public static int RemoveBooks(BookShopContext context)
        {
            var allBooks = context.Books
                .Where(x => x.Copies < 4200)
                .ToList();

            context.RemoveRange(allBooks);
            context.SaveChanges();

            return allBooks.Count;
        }
    }
}