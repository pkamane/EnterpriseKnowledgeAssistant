using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Retrieval
{
    public class VectorSearchResult
    {
        public DocumentChunk     Chunk { get; init; } = default!;
        public float SimilarityScope { get; init; }
    }
}
