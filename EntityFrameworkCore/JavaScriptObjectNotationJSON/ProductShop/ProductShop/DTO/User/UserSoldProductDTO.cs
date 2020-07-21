namespace ProductShop.DTO.User
{
    using Newtonsoft.Json;

    public class UserSoldProductDTO
    {
        [JsonProperty("name")]
        public string ProductName { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("buyerFirstName")]
        public string FirstName { get; set; }

        [JsonProperty("buyerLastName")]
        public string LastName { get; set; }
    }
}