using EnterpriseKnowledgeAssistant.DocumentProcessing.Models;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.Ollama
{
    public class OllamaEmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _httpClient;
        OllamaEmbeddingOptions _options;

        public OllamaEmbeddingService(HttpClient httpClient, OllamaEmbeddingOptions options)
        {
            _httpClient = httpClient;
            _options = options;
        }
        public async Task<EmbeddingResult> GenerateEmbeddingAsync(DocumentChunk chunk, CancellationToken cancellationToken)
        {
            try
            {
                //var request = new OllamaEmbedRequest
                //{
                //    Model = _options.Model,
                //    Input = chunk.Text
                //};
                //var response = await _httpClient.PostAsJsonAsync("api/embed", request, cancellationToken);
                //response.EnsureSuccessStatusCode();

                //var embeddingResponse = await response.Content.ReadFromJsonAsync<OllamaEmbedResponse>(cancellationToken: cancellationToken);
                //if(embeddingResponse == null || embeddingResponse.Embeddings.Count==0)
                //{
                //    return new EmbeddingResult
                //    {
                //        Success = false,
                //        ErrorMessage = "Failed to get embedding from Ollama API.",
                //        Chunk = chunk
                //    };
                //}
                //chunk.Embedding = new EmbeddingVector { Values = embeddingResponse.Embeddings[0],
                //                                        Model = _options.Model,
                //                                        CreatedAtUtc = DateTime.UtcNow};

                chunk.Embedding = await GenerateEmbeddingAsync(chunk.Text, cancellationToken);
                return new EmbeddingResult
                {
                    Success = true,
                    Chunk = chunk
                };
            }
            catch(Exception ex)
            {
                return new EmbeddingResult
                {
                    Success = false,
                    ErrorMessage = $"Exception occurred while generating embedding: {ex.Message}",
                    Chunk = chunk
                };
            }
        }

        public async Task<IReadOnlyList<EmbeddingResult>> GenerateEmbeddingsAsync(IEnumerable<DocumentChunk> chunks, CancellationToken cancellationToken)
        {
          var results = new List<EmbeddingResult>();
            foreach(var chunk in chunks)
            {
                var result = await GenerateEmbeddingAsync(chunk, cancellationToken);
                results.Add(result);
            }
            return results;
        }

        public async Task<EmbeddingVector> GenerateEmbeddingAsync(
    string text,
    CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be empty.", nameof(text));

            var request = new OllamaEmbedRequest
            {
                Model = _options.Model,
                Input = text
            };

            using var response = await _httpClient.PostAsJsonAsync(
                "api/embed",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var embeddingResponse =
                await response.Content.ReadFromJsonAsync<OllamaEmbedResponse>(
                    cancellationToken: cancellationToken);

            if (embeddingResponse is null ||
                embeddingResponse.Embeddings.Count == 0)
            {
                throw new InvalidOperationException(
                    "Ollama did not return an embedding.");
            }

            return new EmbeddingVector
            {
                Values = embeddingResponse.Embeddings[0],
                Model = _options.Model,
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}
