using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.Contracts;
using OpenAI.Embeddings;
using Microsoft.Extensions.Configuration;
namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.OpenAI
{
    public class OpenAIEmbeddingService : IEmbeddingService
    {
        private readonly EmbeddingClient _client;
        private readonly EmbeddingOptions _options;

        public OpenAIEmbeddingService(EmbeddingOptions options)
        {
            //_embeddingClient = embeddingClient;
            _options = options;
            _client = new EmbeddingClient(model: options.Model,
                                            apiKey: options.ApiKey);

            
        }
        public async Task<EmbeddingResult> GenerateEmbeddingAsync(DocumentChunk chunk, CancellationToken cancellationToken)
        {
            //var results = await GenerateEmbeddingsAsync(new[] { chunk });

            //return results.First();
            try
            {
                var response = await _client.GenerateEmbeddingAsync(
                    chunk.Text,
                    cancellationToken: cancellationToken);

                var embedding = response.Value;

                chunk.Embedding = new EmbeddingVector
                {
                    Values = embedding.ToFloats().ToArray(),
                    Model = _options.Model,
                    CreatedAtUtc = DateTime.UtcNow
                };

                return new EmbeddingResult
                {
                    Success = true,
                    Chunk = chunk
                };
            }
            catch (Exception ex)
            {
                return new EmbeddingResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Chunk = chunk
                };
            }
        }

        
        public async Task<IReadOnlyList<EmbeddingResult>> GenerateEmbeddingsAsync(IEnumerable<DocumentChunk> chunks, CancellationToken cancellationToken)
        {
            var chunkList = chunks.ToList();
            var inputs = chunkList
                        .Select(c => c.Text)
                        .ToList();

            var response = await _client.GenerateEmbeddingsAsync(
                                              inputs,
                                              cancellationToken: cancellationToken);
            var results = new List<EmbeddingResult>();

            for (int i = 0; i < chunkList.Count; i++)
            {
                var embedding = response.Value[i];

                chunkList[i].Embedding = new EmbeddingVector
                {
                    //Values = embedding.ToFloats(),

                    Model = _options.Model,

                    CreatedAtUtc = DateTime.UtcNow
                };

                results.Add(new EmbeddingResult
                {
                    Success = true,

                    Chunk = chunkList[i]
                });
            }

            return results;
        }
    }
}
