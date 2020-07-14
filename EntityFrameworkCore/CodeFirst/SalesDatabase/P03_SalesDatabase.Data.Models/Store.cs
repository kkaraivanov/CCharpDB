namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;

    public class Store
    {
        public Store()
        {
            Sales = new HashSet<Sale>();
        }

        public int StoreId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}