using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding
{
    public sealed class EmbeddingOptions
    {
        public string ApiKey { get; init; } = string.Empty;
        public string Model { get; init; } = "text-embedding-3-small";
    }
}
