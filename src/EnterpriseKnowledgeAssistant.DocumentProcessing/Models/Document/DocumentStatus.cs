using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document
{
    public enum DocumentStatus
    {
        Uploaded,
        Extracted,
        Chunked,
        Embedded,
        Indexed,
        Failed
    }
}
