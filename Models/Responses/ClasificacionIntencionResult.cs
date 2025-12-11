using System.Text.Json.Serialization;

namespace DynamicAIAPI.Models.Responses;

public class ClasificacionIntencionResult
{
    [JsonPropertyName("intencion")]
    public string Intencion { get; set; } = string.Empty;

    [JsonPropertyName("mensaje_original")]
    public string MensajeOriginal { get; set; } = string.Empty;
}


