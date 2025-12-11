using FastEndpoints;
using OpenAI.Chat;
using DynamicAIAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrar FastEndpoints
builder.Services.AddFastEndpoints();

// 2. Configurar ChatClient de OpenAI
var apiKey =
    builder.Configuration["OpenAI:ApiKey"] ??
    Environment.GetEnvironmentVariable("OPENAI_API_KEY") ??
    throw new InvalidOperationException("No se encontró la API key de OpenAI.");

var model = builder.Configuration["OpenAI:Model"] ?? "gpt-5-nano";

builder.Services.AddSingleton(new ChatClient(
    model: model,
    apiKey: apiKey
));

// Registrar nuestro servicio de dominio
builder.Services.AddScoped<OpenAIService>();

var app = builder.Build();

// 3. Servir archivos estáticos desde wwwroot
app.UseStaticFiles();

// 4. Habilitar FastEndpoints
app.UseFastEndpoints();

// 5. Ejecutar API
app.Run();
