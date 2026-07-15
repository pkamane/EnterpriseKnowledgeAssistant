using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results
{
    public class ChunkingResult
    {
        public bool Success{get; init;}
        public string ErrorMessage { get; init; } = string.Empty;
        public IList<DocumentChunk> Chunks { get; init; }
        = new List<DocumentChunk>();
    }
}
