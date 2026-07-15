using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.Contracts
{
    public interface IEmbeddingService
    {
        Task<EmbeddingResult> GenerateEmbeddingAsync(DocumentChunk chunk, CancellationToken cancellationToken);

         
        Task<IReadOnlyList<EmbeddingResult>> GenerateEmbeddingsAsync(
                                                IEnumerable<DocumentChunk> chunks, CancellationToken cancellationToken);
    }
}
