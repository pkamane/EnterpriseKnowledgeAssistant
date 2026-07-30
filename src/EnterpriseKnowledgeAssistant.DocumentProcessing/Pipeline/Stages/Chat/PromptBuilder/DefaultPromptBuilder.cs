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
            prompt.AppendLine("Guidelines:");
            prompt.AppendLine("- Give a clear, helpful answer in 2-4 sentences.");
            prompt.AppendLine("- Explain what the answer is and briefly why it matters or how it relates to the context.");
            prompt.AppendLine("- Use specific details from the context when they are available.");
            prompt.AppendLine("- Do not invent facts that are not supported by the context.");
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
