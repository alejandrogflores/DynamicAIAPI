using FastEndpoints;
using System.Text.Json;
using DynamicAIAPI.Models.Requests;
using DynamicAIAPI.Models.Responses;
using DynamicAIAPI.Services;

namespace DynamicAIAPI.Endpoints.Texto
{
    public class ResumenTextoEndpoint : Endpoint<ResumenTextoRequest, StandardResponse>
    {
        private readonly OpenAIService _ai;

        public ResumenTextoEndpoint(OpenAIService ai)
        {
            _ai = ai;
        }

        public override void Configure()
        {
            Post("/api/texto/resumir");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ResumenTextoRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Texto))
            {
                await SendAsync(new StandardResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "El campo 'texto' es obligatorio."
                }, 400, ct);

                return;
            }

            // Prompt para el modelo (resumen en snake_case)
            var prompt = $@"
Resume el siguiente texto en un párrafo claro, preciso y conciso.

Texto:
{req.Texto}

Devuelve SOLO este JSON:

{{
  ""resumen"": ""Aquí va el resumen…"",
  ""texto_original"": ""{req.Texto}""
}}
";

            var aiResult = await _ai.ExecuteChatAsync(prompt);

            if (!aiResult.IsSuccess || aiResult.Response is not string rawText)
            {
                await SendAsync(aiResult, cancellation: ct);
                return;
            }

            var first = rawText.IndexOf('{');
            var last = rawText.LastIndexOf('}');

            string json = (first >= 0 && last > first)
                ? rawText.Substring(first, last - first + 1)
                : rawText;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var resumen = root.TryGetProperty("resumen", out var resumenProp)
                    ? resumenProp.GetString() ?? string.Empty
                    : string.Empty;

                var original = root.TryGetProperty("texto_original", out var origProp)
                    ? origProp.GetString() ?? string.Empty
                    : req.Texto;

                var result = new ResumenTextoResult
                {
                    Resumen = resumen,
                    TextoOriginal = original
                };

                await SendAsync(new StandardResponse
                {
                    IsSuccess = true,
                    Response = result,
                    DollarCost = aiResult.DollarCost
                }, cancellation: ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new StandardResponse
                {
                    IsSuccess = false,
                    Response = json,
                    ErrorMessage = $"Error parseando JSON del modelo: {ex.Message}",
                    DollarCost = aiResult.DollarCost
                }, cancellation: ct);
            }
        }
    }
}
