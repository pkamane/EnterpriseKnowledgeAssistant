# Enterprise Knowledge Assistant

A RAG-based (Retrieval-Augmented Generation) enterprise knowledge assistant built with .NET. It processes documents through a multi-stage pipeline — extraction, chunking, embedding, and retrieval — to enable intelligent question answering over enterprise data.

## Pipeline Flow

```
PDF
 │
 ▼
Text Extraction
 │
 ▼
Pages
 │
 ▼
Chunking
 │
 ▼
Embeddings (Ollama)
 │
 ▼
Vector Store
 │
 ▼
Similarity Search
 │
 ▼
RAG Query Pipeline
```

## Current Progress

### Document Processing Pipeline

- ✅ PDF Text Extraction
- ✅ Document Model
- ✅ Fixed Size Chunking
- ✅ Relative IDs
- ✅ Ollama Embedding Generation
- ⏳ In-Memory Vector Store
- ⏳ Similarity Search
- ⏳ RAG Query Pipeline
