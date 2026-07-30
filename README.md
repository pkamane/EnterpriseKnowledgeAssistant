# Enterprise Knowledge Assistant

A RAG-based (Retrieval-Augmented Generation) enterprise knowledge assistant built with .NET. It processes documents through a multi-stage pipeline — extraction, chunking, embedding, retrieval, and chat — to enable intelligent question answering over enterprise data.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Ollama](https://ollama.com/) running locally (`http://localhost:11434/`)

Pull the required models before running the Playground:

```bash
ollama pull nomic-embed-text   # embeddings
ollama pull llama3.2           # chat (or another chat-capable model)
```

Verify installed models:

```bash
ollama list
```

## Configuration

`appsettings.json` uses separate Ollama sections for embeddings and chat:

| Section | Model (default) | Purpose |
|---------|-----------------|---------|
| `Ollama` | `nomic-embed-text` | Embedding generation via `/api/embed` |
| `OllamaChat` | `llama3.2` | Chat responses via `/api/chat` |

Update `OllamaChat:Model` to match a chat model you have installed (`ollama list`).

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
RAG Query Pipeline (Ollama Chat)
```

## Running the Playground

```bash
cd src/EnterpriseKnowledgeAssistant.Playground
dotnet run
```

## Current Progress

### Document Processing Pipeline

- ✅ PDF document extraction
- ✅ Intelligent text chunking
- ✅ Embedding generation (OpenAI & Ollama)
- ✅ In-memory vector store
- ✅ Cosine similarity search
- ✅ Prompt engineering
- ✅ Local RAG pipeline using Ollama
- 🚧 Conversation memory (coming next)
- 🚧 Streaming responses
- 🚧 Persistent vector database
- 🚧 Multi-agent architecture
