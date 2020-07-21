namespace ProductShop.DTO.User
{
    using Newtonsoft.Json;

    public class UserWithSoldProductDTO
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public UserSoldProductDTO[] SoldProducts { get; set; }
    }
}