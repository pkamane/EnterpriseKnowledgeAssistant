using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Retrieval
{
    public interface IVectorStore
    {
        Task AddAsync(
            DocumentChunk chunk,
            CancellationToken cancellationToken);

        Task AddRangeAsync(
            IEnumerable<DocumentChunk> chunks,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<VectorSearchResult>> SearchAsync(
            IReadOnlyList<float> queryEmbedding,
            int topK,
            CancellationToken cancellationToken);
    }
}
