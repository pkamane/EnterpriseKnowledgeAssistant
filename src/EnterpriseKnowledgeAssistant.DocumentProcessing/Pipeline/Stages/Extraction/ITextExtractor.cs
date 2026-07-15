using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Extraction
{
    public interface ITextExtractor
    {
        bool CanHandle(string contentType);
        Task<ExtractionResult> ExtractTextAsync(Stream stream);
    }
}
