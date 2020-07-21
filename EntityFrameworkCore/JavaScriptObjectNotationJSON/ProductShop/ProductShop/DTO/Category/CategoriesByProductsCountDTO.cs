namespace ProductShop.DTO.Category
{
    using Newtonsoft.Json;

    public class CategoriesByProductsCountDTO
    {
        [JsonProperty("category")]
        public string Name { get; set; }

        [JsonProperty("productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public string TotalRevenue { get; set; }
    }
}