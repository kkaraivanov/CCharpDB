namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Data.Model;
    using Data.Model.Enums;
    using ImportDto;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
           var serializer = new XmlSerializer(typeof(ImportBooksDto[]), new XmlRootAttribute("Books"));
           var sb = new StringBuilder();

           using (StringReader sr = new StringReader(xmlString))
           {
               var bookDto = (ImportBooksDto[]) serializer.Deserialize(sr);
               var books = new List<Book>();

               foreach (ImportBooksDto book in bookDto)
               {
                   if (!IsValid(book))
                   {
                       sb.AppendLine(ErrorMessage);
                       continue;
                   }

                   DateTime publishedOn;
                   var isValidDate = DateTime
                       .TryParseExact(book.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out publishedOn);

                   if (!isValidDate)
                   {
                       sb.AppendLine(ErrorMessage);
                       continue;
                   }

                   var importBook = new Book()
                   {
                       Name = book.Name,
                       Genre = (Genre) book.Genre,
                       Price = book.Price,
                       Pages = book.Pages,
                       PublishedOn = publishedOn
                   };
                   
                   books.Add(importBook);
                   var validMessage = string.Format(SuccessfullyImportedBook, importBook.Name, importBook.Price);
                   sb.AppendLine(validMessage);
               }
               
               context.Books.AddRange(books);
               context.SaveChanges();
           }

           return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsDto = JsonConvert.DeserializeObject<ImportAuthorsDto[]>(jsonString);
            //var jsonNamespace = json
            var sb = new StringBuilder();
            List<Author> authors = new List<Author>();

            foreach (ImportAuthorsDto author in authorsDto)
            {
                if (!IsValid(author))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (authors.Any(x => x.Email == author.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var importAuthor = new Author()
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Email = author.Email,
                    Phone = author.Phone
                };
                
                foreach (var book in author.AuthorsBooks)
                {
                    if (!book.BookId.HasValue)
                        continue;

                    var currentBook = context.Books.FirstOrDefault(x => x.Id == book.BookId);
                    if(currentBook == null)
                        continue;

                    importAuthor.AuthorsBooks.Add(new AuthorBook()
                    {
                        Author = importAuthor,
                        Book = currentBook
                    });
                }

                if (importAuthor.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(importAuthor);
                var validMessage = string.Format(
                    SuccessfullyImportedAuthor, 
                    importAuthor.FirstName + " " + importAuthor.LastName, 
                    importAuthor.AuthorsBooks.Count);
                sb.AppendLine(validMessage);
            }

            context.Authors.AddRange(authors);
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