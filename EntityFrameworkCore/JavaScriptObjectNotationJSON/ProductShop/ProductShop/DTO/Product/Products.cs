namespace ProductShop.DTO.Product
{
    using Newtonsoft.Json;

    public class Products
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }
    }
}