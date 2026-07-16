using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document.DocumentChunk;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Retrieval
{
    public static class CosineSimilarityCalculator
    {
        public static float Calculate(
            IReadOnlyList<float> vectorA,
            IReadOnlyList<float> vectorB)
        {
            if(vectorA is null)
            {
                throw new ArgumentNullException(nameof(vectorA));
            }
            if(vectorB is null)
            {
                throw new ArgumentNullException(nameof(vectorB));
            }

            if(vectorA.Count != vectorB.Count)
            {
                throw new ArgumentException("Vectors must be of the same length.");
            }

            float dotProduct = 0f;
            float magnitudeA = 0f;
            float magnitudeB = 0f;

            for(int i = 0; i<vectorA.Count; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += vectorA[i] * vectorA[i];
                magnitudeB += vectorB[i] * vectorB[i];
            }

            if(magnitudeA == 0 || magnitudeB == 0)
            {
                return 0f; // Avoid division by zero
            }

            return dotProduct / (float)(Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
        }
    }
}
