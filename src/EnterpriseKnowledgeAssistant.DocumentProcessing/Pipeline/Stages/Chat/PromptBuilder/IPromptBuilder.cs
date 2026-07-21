using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.PromptBuilder
{
    public interface IPromptBuilder
    {
        string BuildPrompt(PromptContext context);
    }
}
