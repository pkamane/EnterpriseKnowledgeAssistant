using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Results;
using DomainDocument =
    EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.Document;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chunking
{
    public class FixedSizeChunker : IChunker
    {
        private readonly int _chunkSize;
        private readonly int _overlapSize;
        public FixedSizeChunker(int chunkSize = 1000, int overlapSize = 100)
        {
            if(chunkSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunkSize));

            if(overlapSize <=0)
                throw new ArgumentOutOfRangeException(nameof(overlapSize));
            
            if(overlapSize >= chunkSize)
                throw new ArgumentException("Overlap must be smaller than chunk size.");
            _chunkSize = chunkSize;
            _overlapSize = overlapSize;
        }

        public Task<ChunkingResult> ChunkAsync(DomainDocument document)
        {
            var chunkingResult = new ChunkingResult() { Success = true };
            int chunkSequence = 1;

            foreach (var page in document.Pages)
            {
                var pageChunks = ChunkPage(document,page, ref chunkSequence);

                foreach (var chunk in pageChunks)
                    chunkingResult.Chunks.Add(chunk);
            }
            return Task.FromResult(chunkingResult);
        }

        private IEnumerable<DocumentChunk> ChunkPage(
       DomainDocument document,
       DocumentPage page,
       ref int chunkSequence)
        {
            var chunks = new List<DocumentChunk>();

            string text = page.Text;
            if (string.IsNullOrWhiteSpace(text))
                return chunks;

            int position = 0;
            while (position < text.Length) 
            {
                int remainingCharacters = text.Length - position;
                int currentChunkSize = Math.Min(_chunkSize, remainingCharacters);
                int candidateEnd = position + currentChunkSize;
                int chunkEnd = FindChunkBoundary(
                text,
                position,
                candidateEnd);

                string chunkText = text.Substring(
               position,
               chunkEnd - position).Trim();
                chunks.Add(new DocumentChunk
                {
                    DocumentId = document.Id,
                    ChunkSequence = chunkSequence++,

                    StartPage = page.PageNumber,
                    EndPage = page.PageNumber,

                    StartOffset = position,
                    EndOffset = chunkEnd,

                    Text = chunkText
                });

                if (chunkEnd >= text.Length)
                    break;

                int candidateStart = Math.Max(
                 chunkEnd - _overlapSize,
                 position + 1);

                position = FindChunkStart(
                    text,
                    candidateStart);

            }

            return chunks;
        }

        private int FindChunkBoundary(
        string text,
        int start,
        int candidateEnd)
        {
            if (candidateEnd >= text.Length)
                return text.Length;

            int boundary = candidateEnd;

            while (boundary > start)
            {
                if (char.IsWhiteSpace(text[boundary]))
                    break;

                boundary--;
            }

            // No whitespace found.
            // Split at the original candidate position.
            if (boundary == start)
                return candidateEnd;

            return boundary;
        }

        private int FindChunkStart(
    string text,
    int candidateStart)
        {
            if (candidateStart <= 0)
                return 0;

            int start = candidateStart;

            // If we are already at whitespace,
            // move forward to the first non-whitespace character.
            while (start < text.Length && char.IsWhiteSpace(text[start]))
            {
                start++;
            }

            // Otherwise, if we are in the middle of a word,
            // move backwards until we reach whitespace.
            while (start > 0 && !char.IsWhiteSpace(text[start - 1]))
            {
                start--;
            }

            return start;
        }
    }
}
