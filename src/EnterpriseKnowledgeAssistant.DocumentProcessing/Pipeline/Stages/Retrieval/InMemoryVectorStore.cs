using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Retrieval
{
    public class InMemoryVectorStore : IVectorStore
    {
        private readonly List<DocumentChunk> _chunks = new();

        public InMemoryVectorStore()
        {
            
        }

        public Task AddAsync(DocumentChunk chunk, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if(chunk is null)
            {
                throw new ArgumentNullException(nameof(chunk));
            }
            
            if(chunk.Embedding is null)
            {
                throw new ArgumentException($"Chunk '{chunk.RelativeId}' does not contain an embedding.", nameof(chunk));
            }

            _chunks.Add(chunk);
            return Task.CompletedTask;
        }

        public Task AddRangeAsync(IEnumerable<DocumentChunk> chunks, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if(chunks is null)
            {
                throw new ArgumentNullException(nameof(chunks));
            }

            foreach(var chunk in chunks)
            {
                if (chunk is null)
                {
                    throw new ArgumentException("One or more chunks are null.", nameof(chunks));
                }
                if (chunk.Embedding is null)
                {
                    throw new ArgumentException($"Chunk '{chunk.RelativeId}' does not contain an embedding.", nameof(chunks));
                }
                _chunks.Add(chunk);
            }
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<VectorSearchResult>> SearchAsync(IReadOnlyList<float> queryEmbedding, int topK, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if(queryEmbedding is null)
            {
                throw new ArgumentNullException(nameof(queryEmbedding));
            }

            if(topK <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(topK), "topK must be greater than zero.");
            }

            List<VectorSearchResult> results = new();

            foreach (var chunk in _chunks)
            {
                if (chunk.Embedding is null)
                {
                    continue; 
                }


            
                float similarity =  CosineSimilarityCalculator.Calculate(queryEmbedding, chunk.Embedding.Values);
                results.Add(new VectorSearchResult { Chunk = chunk, SimilarityScope = similarity });
            }

            var topResults = results.OrderByDescending(r => r.SimilarityScope).Take(topK).ToList();
            return Task.FromResult<IReadOnlyList<VectorSearchResult>>(topResults);
        }
    }
}
