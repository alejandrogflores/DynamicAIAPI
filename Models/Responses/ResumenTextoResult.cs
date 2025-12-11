using System.Text.Json.Serialization;

namespace DynamicAIAPI.Models.Responses
{
    public class ResumenTextoResult
    {
        [JsonPropertyName("resumen")]
        public string Resumen { get; set; } = string.Empty;

        [JsonPropertyName("texto_original")]
        public string TextoOriginal { get; set; } = string.Empty;
    }
}
