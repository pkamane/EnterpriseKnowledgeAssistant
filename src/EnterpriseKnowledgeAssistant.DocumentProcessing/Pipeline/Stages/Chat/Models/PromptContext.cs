using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Retrieval;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Models
{
    public class PromptContext
    {
        public string Question { get; init; } = string.Empty;
        public IReadOnlyList<VectorSearchResult> SearchResults { get; init; } = Array.Empty<VectorSearchResult>();
    }
}
