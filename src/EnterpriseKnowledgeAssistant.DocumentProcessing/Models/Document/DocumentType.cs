using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document
{
    public enum DocumentType
    {
        Unknown = 0,
        Pdf,
        Word,
        Excel,
        PowerPoint,
        Html,
        Markdown,
        Text,
        Image,
        Email
    }
}
