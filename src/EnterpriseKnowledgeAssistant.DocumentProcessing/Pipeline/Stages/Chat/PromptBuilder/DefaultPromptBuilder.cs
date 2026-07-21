using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.PromptBuilder
{
    public class DefaultPromptBuilder : IPromptBuilder
    {
        public string BuildPrompt(PromptContext context)
        {
            if(context == null) throw new ArgumentNullException(nameof(context));
            var prompt = new StringBuilder();
            prompt.AppendLine("You are a helpful AI assistant.");
            prompt.AppendLine();
            prompt.AppendLine("Answer the user's question using ONLY the information provided in the context below.");
            prompt.AppendLine();
            prompt.AppendLine("If the answer cannot be found in the context, say:");
            prompt.AppendLine("\"I don't have enough information to answer that question.\"");
            prompt.AppendLine();

            prompt.AppendLine("========== CONTEXT ==========");

            foreach (var result in context.SearchResults)
            {
                prompt.AppendLine();
                prompt.AppendLine($"Chunk: {result.Chunk.RelativeId}");
                prompt.AppendLine(result.Chunk.Text);
            }

            prompt.AppendLine();
            prompt.AppendLine("=============================");
            prompt.AppendLine();
            prompt.AppendLine($"Question: {context.Question}");
            prompt.AppendLine();
            prompt.Append("Answer:");

            return prompt.ToString();
        }
    }
}
