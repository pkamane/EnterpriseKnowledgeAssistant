using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results
{
    public sealed class EmbeddingResult
    {
        public bool Success { get; init; }

        public string? ErrorMessage { get; init; }

        public DocumentChunk Chunk { get; init; } = default!;
    }
}
