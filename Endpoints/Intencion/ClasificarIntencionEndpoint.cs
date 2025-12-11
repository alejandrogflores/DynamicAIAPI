using FastEndpoints;
using System.Text.Json;
using DynamicAIAPI.Models.Requests;
using DynamicAIAPI.Models.Responses;
using DynamicAIAPI.Services;

namespace DynamicAIAPI.Endpoints.Intencion
{
    public class ClasificarIntencionEndpoint : Endpoint<ClasificarIntencionRequest, StandardResponse>
    {
        private readonly OpenAIService _ai;

        public ClasificarIntencionEndpoint(OpenAIService ai)
        {
            _ai = ai;
        }

        public override void Configure()
        {
            Post("/api/intencion/clasificar");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ClasificarIntencionRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.InputUsuario))
            {
                await SendAsync(new StandardResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "El campo 'inputUsuario' es obligatorio."
                }, 400, ct);

                return;
            }

            // Prompt para el modelo (formato snake_case)
            var prompt = $@"
Clasifica la intención del usuario.

Intenciones posibles:
- saludo
- despedida
- necesita_ayuda
- otro

Mensaje del usuario:
{req.InputUsuario}

Devuelve SOLO el siguiente JSON (sin explicaciones extra, sin texto antes ni después):

{{
  ""intencion"": ""saludo | despedida | necesita_ayuda | otro"",
  ""mensaje_original"": ""{req.InputUsuario}""
}}
";

            // 1. Llamar a OpenAI: nos devuelve StandardResponse con Response = string
            var aiResult = await _ai.ExecuteChatAsync(prompt);

            if (!aiResult.IsSuccess || aiResult.Response is not string rawText)
            {
                // Si falló la llamada a OpenAI, devolvemos ese error tal cual
                await SendAsync(aiResult, cancellation: ct);
                return;
            }

            // 2. Extraer solo el JSON válido de la respuesta del modelo
            var first = rawText.IndexOf('{');
            var last = rawText.LastIndexOf('}');

            string json = (first >= 0 && last > first)
                ? rawText.Substring(first, last - first + 1)
                : rawText;

            try
            {
                // 3. Parsear el JSON y mapear a nuestro modelo tipado
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var intencion = root.TryGetProperty("intencion", out var intencionProp)
                    ? intencionProp.GetString() ?? string.Empty
                    : string.Empty;

                var mensajeOriginal = root.TryGetProperty("mensaje_original", out var mensajeProp)
                    ? mensajeProp.GetString() ?? string.Empty
                    : req.InputUsuario;

                var result = new ClasificacionIntencionResult
                {
                    Intencion = intencion,
                    MensajeOriginal = mensajeOriginal
                };

                // 4. Devolver respuesta estándar con objeto tipado
                var outputOk = new StandardResponse
                {
                    IsSuccess = true,
                    Response = result,     // 👈 Objeto → se serializa con snake_case por los JsonPropertyName
                    ErrorMessage = null,
                    DollarCost = aiResult.DollarCost
                };

                await SendAsync(outputOk, cancellation: ct);
            }
            catch (Exception ex)
            {
                // Si el modelo devolvió JSON inválido o algo raro
                var outputError = new StandardResponse
                {
                    IsSuccess = false,
                    Response = json,  // te mando lo que devolvió el modelo para depurar
                    ErrorMessage = $"Error parseando JSON del modelo: {ex.Message}",
                    DollarCost = aiResult.DollarCost
                };

                await SendAsync(outputError, cancellation: ct);
            }
        }
    }
}



