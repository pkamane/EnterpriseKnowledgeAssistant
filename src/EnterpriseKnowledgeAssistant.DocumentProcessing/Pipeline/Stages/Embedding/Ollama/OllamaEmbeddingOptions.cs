using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.Ollama
{
    public class OllamaEmbeddingOptions
    {
        public string BaseUrl { get; init; } = "http://localhost:11434/";

        public string Model { get; init; } = "nomic-embed-text";
    }
}
