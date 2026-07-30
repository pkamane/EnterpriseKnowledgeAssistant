using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Models
{
    public class ChatResponse
    {
        public bool Success { get; init; }
        public string Response { get; init; } = string.Empty;
        public string? ErrorMessage { get; init; }

    }
}
