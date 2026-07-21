using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Options
{
    public class OpenAIChatOptions
    {
        public string ApiKey { get; init; } = string.Empty;

        public string Model { get; init; } = string.Empty;
    }
}
