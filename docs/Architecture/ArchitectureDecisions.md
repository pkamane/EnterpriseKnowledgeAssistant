# Enterprise Knowledge Assistant
## Architecture Decisions

---

# AD-001 : Domain-First Development

## Status
✅ Frozen

## Decision

Before implementing any feature, we first design the domain model.

Implementation follows the domain, not vice versa.

## Why?

The domain model represents the business concepts of the application.

Algorithms, databases and AI providers may change, but the domain remains relatively stable.

This approach reduces refactoring and produces cleaner software.

---

# AD-002 : Document is the Aggregate Root

## Status
✅ Frozen

## Decision

`Document` is the root entity of the knowledge processing pipeline.

Everything else belongs to a document.

```
Document
│
├── DocumentPage
├── DocumentChunk
├── Embeddings
└── Metadata
```

## Why?

- A page cannot exist without a document.
- A chunk cannot exist without a document.
- Reprocessing a document recreates all downstream artifacts.
- Deleting a document removes all associated data.

This follows the Aggregate Root concept from Domain Driven Design.

---

# AD-003 : Identity Strategy

## Status
✅ Frozen

## Decision

Identity is assigned according to ownership.

| Object | Identity |
|---------|----------|
| Document | Guid |
| DocumentPage | Document + PageNumber |
| DocumentChunk | DocumentId + ChunkSequence |

## Why?

Only independent entities receive globally unique identifiers.

Child objects derive their identity from their parent whenever possible.

This avoids unnecessary identifiers and clearly models ownership.

---

# AD-004 : DocumentPage is a Child Object

## Status
✅ Frozen

## Decision

DocumentPage does not contain its own Guid.

DocumentPage does not contain DocumentId.

It always belongs to a Document.

## Why?

Pages never exist independently.

During processing we always access pages through the owning document.

---

# AD-005 : DocumentChunk Carries DocumentId

## Status
✅ Frozen

## Decision

DocumentChunk stores DocumentId.

## Why?

Chunks eventually leave the Document aggregate.

They are:

- embedded
- indexed
- searched
- retrieved independently

Keeping DocumentId allows every chunk to be traced back to its source document.

---

# AD-006 : Pipeline Architecture

## Status
✅ Frozen

## Decision

The document processing pipeline consists of independent stages.

```
Upload

↓

Extraction

↓

Chunking

↓

Embedding

↓

Indexing
```

Each stage has:

- one interface
- one or more implementations
- one result object

## Why?

Every stage should be replaceable without affecting the others.

Examples:

OpenAI Embedding

↓

Ollama Embedding

↓

AWS Bedrock

↓

Azure OpenAI

No upstream code changes.

---

# AD-007 : Each Stage Produces a New Object

## Status
✅ Frozen

## Decision

Pipeline stages never modify the output of previous stages.

Instead, they produce a new result.

Example

```
Stream

↓

ExtractionResult

↓

ChunkingResult

↓

EmbeddingResult
```

## Why?

This makes every stage:

- testable
- independent
- reusable

---

# AD-008 : Business-Oriented Folder Structure

## Status
✅ Frozen

## Decision

Folders are organised by business capability rather than technical type.

Example

```
Pipeline

Extraction

Chunking

Embedding

Indexing
```

instead of

```
Interfaces

Helpers

Enums

Utilities
```

## Why?

Developers think in terms of business capabilities.

The folder structure should reflect the business language.

---

# AD-009 : Keep the Domain Model Minimal

## Status
✅ Frozen

## Decision

Only properties that have a real consumer are added.

Future requirements should not be anticipated without evidence.

## Example

Removed:

DocumentPage.Metadata

Reason:

No pipeline stage currently uses it.

It can be added later without breaking existing code.

---

# AD-010 : Chunking Strategy

## Status
🟡 Planned

## Decision

Version 1 will use fixed-size chunking with overlap.

Characteristics

- configurable chunk size
- configurable overlap
- avoid splitting words
- preserve page information

Future versions

- semantic chunking
- sentence-aware chunking
- heading-aware chunking

## Why?

Provides a simple implementation while remaining extensible.

---

# AD-011 : Reprocessing Strategy

## Status
✅ Frozen

## Decision

Incorrectly processed documents are not repaired.

They are reprocessed from the original source.

```
Delete

↓

Extract

↓

Chunk

↓

Embed

↓

Index
```

## Why?

Reprocessing guarantees consistency throughout the pipeline.

Repairing downstream artifacts risks inconsistencies.

---

# AD-012 : Technology Independence

## Status
✅ Frozen

## Decision

Domain models never depend on:

- PdfPig
- OpenAI
- Qdrant
- AWS
- Azure

## Why?

The domain represents the business.

Infrastructure represents technology.

Technology should be replaceable.

```