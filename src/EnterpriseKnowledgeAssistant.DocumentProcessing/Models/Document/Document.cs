using EnterpriseKnowledgeAssistant.DocumentProcessing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document
{
    public class Document
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public DocumentType Type { get; init; }

        public DocumentStatus Status { get; set; }

        public string? SourceIdentifier { get; init; }

        public Dictionary<string, string> Metadata { get; init; }
            = new();

        public IList<DocumentPage> Pages { get; init; } = new List<DocumentPage>();
    }
}
