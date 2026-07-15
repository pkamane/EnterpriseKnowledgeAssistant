using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results
{
    public class ExtractionResult : PipelineResult
    {
        public IReadOnlyList<DocumentPage> Pages { get; init; }
         = new List<DocumentPage>();
    }
}
