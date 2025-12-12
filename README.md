# DynamicAIAPI – API Dinámica con .NET 8 + FastEndpoints + OpenAI

Este proyecto implementa una API dinámica utilizando .NET 8, FastEndpoints y el SDK oficial de OpenAI, diseñada como parte de una prueba técnica.
La API expone endpoints que procesan texto mediante modelos de lenguaje, permitiendo:
- Clasificar la intención del usuario.
- Resumir texto.
- Retornar respuestas estandarizadas con manejo de errores.


# Tecnologías utilizadas
- .NET 8
- FastEndpoints
- OpenAI SDK v2.x (ChatClient)
- C# 12
- Variables de entorno para manejo seguro de apikey

Estructura: 
``` DynamicAIAPI/
 ├── Endpoints/
 │    ├── Intencion/
 │    │     └── ClasificarIntencionEndpoint.cs
 │    └── Texto/
 │          └── ResumenTextoEndpoint.cs
 ├── Models/
 │    ├── Requests/
 │    └── Responses/
 ├── Services/
 │    └── OpenAIService.cs
 ├── wwwroot/
 ├── Program.cs
 ├── appsettings.json
 └── README.md
```
---
# Configuración de la API Key
 La API no incluye credenciales dentro del repositorio.
Para ejecutar el proyecto debes definir la API Key como variable de entorno:

macOS / Linux: export OPENAI_API_KEY="TU_API_KEY_AQUI"

Para hacerla permanente: nano ~/.zshrc

Agregar: export OPENAI_API_KEY="TU_API_KEY_AQUI"

Recargar: source ~/.zshrc
setx OPENAI_API_KEY "TU_API_KEY_AQUI"
_ _ _

# Ejecutar el proyecto

Desde la raíz del proyecto:
```
dotnet restore
dotnet build
dotnet run
```

Salida esperada:
# Ejecutar el proyecto

Desde la raíz del proyecto:

```
dotnet restore
dotnet build
dotnet run
```

Salida esperada:


Now listening on: http://localhost:5144
Endpoints disponibles

# Clasificar intención del usuario

POST

/api/intencion/clasificar

Body
{
  "inputUsuario": "Hola, necesito ayuda con un problema."
}

Respuesta
{
  "isSuccess": true,
  "response": {
    "intencion": "necesita_ayuda",
    "mensaje_original": "Hola, necesito ayuda con un problema."
  },
  "errorMessage": null,
  "dollarCost": 0
}

# Resumen de texto

POST

/api/texto/resumir

Body
{
  "texto": "En un pequeño pueblo rodeado de montañas vivían tres amigos..."
}

Respuesta
{
  "isSuccess": true,
  "response": {
    "resumen": "Tres amigos soñaban con construir una máquina voladora.",
    "texto_original": "En un pequeño pueblo rodeado de montañas vivían tres amigos..."
  },
  "errorMessage": null,
  "dollarCost": 0
}

# Validaciones implementadas

Los endpoints incluyen validaciones:

- Campos obligatorios (inputUsuario, texto)

- Respuestas estandarizadas

- Manejo de errores con mensajes claros en español

- Extracción segura del JSON generado por OpenAI

- Conversión tipada mediante modelos DTO

# Colección de Postman

https://alejandrogflores2015-4297215.postman.co/workspace/Jorge-Alejandro-Garc%C3%ADa-Flores's~1c4c776d-9ff9-4c3a-8536-0d677e83f7f6/collection/50724924-300dd66e-c732-40ef-a40b-b5569a54845e?action=share&creator=50724924&live=uh10c29d5i

# Cumplimiento de los requisitos de la prueba técnica

Este proyecto cumple con:

API .NET 8 + FastEndpoints

Llamadas al SDK oficial de OpenAI

Validación de parámetros

Respuesta estándar requerida

Manejo seguro de la API Key

Código mantenible, limpio y organizado

# Autor

Jorge Alejandro García Flores
