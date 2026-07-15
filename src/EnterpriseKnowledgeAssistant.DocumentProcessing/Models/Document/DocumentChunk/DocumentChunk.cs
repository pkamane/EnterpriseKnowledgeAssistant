using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk
{
    public class DocumentChunk
    {
        public string RelativeId =>
        $"{DocumentId}:{ChunkSequence}";
        public Guid DocumentId { get; init; }

        public int ChunkSequence { get; init; }

        public int StartPage { get; init; }

        public int EndPage { get; init; }

        public int StartOffset { get; init; }

        public int EndOffset { get; init; }

        public string Text { get; init; } = string.Empty;
        public EmbeddingVector? Embedding { get; set; }

        public Dictionary<string, string> Metadata { get; } = new();
    }
}
