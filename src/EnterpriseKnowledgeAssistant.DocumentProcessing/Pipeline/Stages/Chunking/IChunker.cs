using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using DomainDocument =
    EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.Document;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chunking
{
    public interface IChunker
    {
        Task<ChunkingResult> ChunkAsync(DomainDocument document);

    }
}
