using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Options
{
    public class OllamaChatOptions
    {
        public string BaseUrl { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
    }
}
