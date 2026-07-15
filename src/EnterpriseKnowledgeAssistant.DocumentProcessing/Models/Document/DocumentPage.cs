using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document
{
    public class DocumentPage
    {
        public Guid DocumentId { get; init; }
        public string RelativeId =>
      $"{DocumentId}:{PageNumber}";
        public int PageNumber { get; init; }
        public string Text { get; init; } = string.Empty;
        //public Dictionary<string, string> Metadata { get; init; }        = new();
    }
}
