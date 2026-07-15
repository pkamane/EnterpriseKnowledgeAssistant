using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.Ollama
{
    public class OllamaEmbedRequest
    {
        public string Model { get; init; } = string.Empty;

        public string Input { get; init; } = string.Empty;
    }
}
