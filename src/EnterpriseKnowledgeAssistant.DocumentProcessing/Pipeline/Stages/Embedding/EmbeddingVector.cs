using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding
{
    public  class EmbeddingVector
    {
        public IReadOnlyList<float> Values { get; init; } = Array.Empty<float>();

        public int Dimensions => Values.Count;
        public string Model { get; init; } = string.Empty;

        public DateTime CreatedAtUtc { get; init; }
    }
}
