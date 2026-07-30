using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat
{
    public interface IChatService
    {
        Task<ChatResponse> GenerateResponseAsync(
        ChatRequest request,
        CancellationToken cancellationToken);
    }
}
