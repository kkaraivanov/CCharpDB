namespace ProductShop.DTO.User
{
    using Newtonsoft.Json;
    using Product;

    public class UsersDTO
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }
    }
}