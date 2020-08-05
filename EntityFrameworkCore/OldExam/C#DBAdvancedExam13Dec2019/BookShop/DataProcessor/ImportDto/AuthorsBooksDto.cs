namespace BookShop.DataProcessor.ImportDto
{
    using Newtonsoft.Json;

    public class AuthorsBooksDto
    {
        [JsonProperty("Id")]
        public int? BookId { get; set; }
    }
}