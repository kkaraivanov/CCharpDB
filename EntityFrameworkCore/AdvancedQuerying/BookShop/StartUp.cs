namespace BookShop
{
    using System;
    using Data;

    public class StartUp
    {
        static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            
            try
            {
                //Console.WriteLine(InputReader.GetMostRecentBooks(db));
                //Console.WriteLine(InputReader.RemoveBooks(db));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        
    }
}
