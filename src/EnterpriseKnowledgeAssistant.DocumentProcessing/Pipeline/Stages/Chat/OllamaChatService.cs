using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Models;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat
{

    // --------------------------------------------------------------------
    // Internal DTOs representing Ollama's REST API contract.
    // These classes should never be exposed outside this service.
    // --------------------------------------------------------------------
    public class OllamaChatService : IChatService
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private readonly HttpClient _httpClient;
        private readonly OllamaChatOptions _options;

        public OllamaChatService(HttpClient httpClient, OllamaChatOptions options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options ?? throw new ArgumentNullException(nameof(options));

        }

        public async Task<ChatResponse> GenerateResponseAsync(
                                            ChatRequest request,
                                            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

           ArgumentNullException.ThrowIfNull(request, nameof(request));
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if(string.IsNullOrWhiteSpace(request.Prompt))
            {
                throw new ArgumentException("Prompt cannot be empty.", nameof(request));
            }

            try
            {
                // Build Ollama request

                var ollamaRequest = new OllamaChatRequest
                {
                    Model = _options.Model,
                    Messages = new List<OllamaMessage>
                {
                    new OllamaMessage
                    {
                        Role = "user",
                        Content = request.Prompt
                    }
                },
                    Stream = false
                };

                var json = JsonSerializer.Serialize(ollamaRequest, JsonOptions);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

                using var response = await _httpClient.PostAsync("api/chat", requestContent, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
                var ollamaResponse = JsonSerializer.Deserialize<OllamaChatResponse>(responseJson, JsonOptions);
                if (ollamaResponse?.Message is not { Content: { Length: > 0 } assistantContent })
                {
                    throw new InvalidOperationException("Invalid or empty response from Ollama API.");
                }

                // Return ChatResponse
                return new ChatResponse
                {
                    Success = true,
                    Response = assistantContent
                };
            }
            catch (Exception ex)
            {
                // Log the exception (logging not implemented in this example)
                return new ChatResponse
                {
                    Success = false,
                    Response = string.Empty,
                    ErrorMessage = ex.Message
                };
            }

        }

        private sealed class OllamaChatRequest
        {
            public string Model { get; init; } = string.Empty;

            public List<OllamaMessage> Messages { get; init; } = new();

            public bool Stream { get; init; } = false;
        }

        private sealed class OllamaMessage
        {
            public string Role { get; init; } = string.Empty;

            public string Content { get; init; } = string.Empty;
        }

        private sealed class OllamaChatResponse
        {
            public OllamaMessage Message { get; init; } = new();
        }

        
    }
}
