using System.Text.Json.Serialization;

namespace MarkdownNoteTakingAPI.Data
{
    public class EditDTO
    {
        [JsonPropertyName("description")]
        public string description { get; set; }

        [JsonPropertyName("error_type")]
        public string errorType { get; set; }

        [JsonPropertyName("replacement")]
        public string replacement { get; set; }
        [JsonPropertyName("sentence")]
        public string sentence { get; set; }
    }
}
