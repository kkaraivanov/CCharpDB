namespace ProductShop.DTO.User
{
    using Newtonsoft.Json;

    public class GetUsersWithProducts
    {
        [JsonProperty("usersCount")]
        public int UsersCount { get; set; }

        [JsonProperty("users")]
        public UsersDTO Users { get; set; }
    }
}