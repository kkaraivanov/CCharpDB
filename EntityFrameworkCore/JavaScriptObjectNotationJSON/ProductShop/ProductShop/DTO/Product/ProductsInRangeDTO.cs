namespace ProductShop.DTO.Product
{
    using Newtonsoft.Json;

    public class ProductsInRangeDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("seller")]
        public string SellerName { get; set; }
    }
}