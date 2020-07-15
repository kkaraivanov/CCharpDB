namespace BookShop
{
    using System;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using Data;
    using Initializer;
    using Initializer.Managment;
    using Models.Enums;

    public class StartUp
    {
        static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            
            try
            {
                Console.WriteLine(GetBooksNotReleasedIn(db, int.Parse(Console.ReadLine())));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

        }
    }
}
