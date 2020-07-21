namespace ProductShop.DTO.Product
{
    using Newtonsoft.Json;

    public class SoldProduct
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("products")]
        public Products Product { get; set; }
    }
}