using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Extraction
{
    public class PdfTextExtractor : ITextExtractor
    {
        bool ITextExtractor.CanHandle(string contentType)
        {
            //The Upload API should determine the MIME/content type.
            return contentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase);
        }

        public Task<ExtractionResult> ExtractTextAsync(Stream stream)
        {
            try
            {
                var pages = new List<DocumentPage>();

                using var document = PdfDocument.Open(stream);

                foreach (var page in document.GetPages())
                {
                    pages.Add(new DocumentPage
                    {
                        PageNumber = page.Number,
                        Text = page.Text
                    });
                }

                var result = new ExtractionResult
                {
                    Success = true,
                    Pages = pages
                };

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ExtractionResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
