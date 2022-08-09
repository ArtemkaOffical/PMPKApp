using System.Text.Json.Serialization;

namespace PMPK.Models
{
    public class Specialists
    {
        public int Id { get; set; }
        [JsonPropertyName("ФИО")]
        public string FullName { get; set; }
        [JsonPropertyName("Должность")]
        public string Type { get; set; }
    }
}
