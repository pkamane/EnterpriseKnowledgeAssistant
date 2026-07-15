using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.Ollama
{
    public class OllamaEmbedResponse
    {
        [JsonPropertyName("embeddings")] 
        public List<List<float>> Embeddings { get; init; } = [];
    }
}
