using EnterpriseKnowledgeAssistant.DocumentProcessing.Models.Document;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.Models;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chat.PromptBuilder;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Chunking;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.Contracts;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.Ollama;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Embedding.OpenAI;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Extraction;
using EnterpriseKnowledgeAssistant.DocumentProcessing.Pipeline.Stages.Retrieval;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "production";

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Diagnostic output: print environment variables so it's clear what Visual Studio provided
Console.WriteLine($"DOTNET_ENVIRONMENT (from Env): {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}");
Console.WriteLine($"ASPNETCORE_ENVIRONMENT (from Env): {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
Console.WriteLine($"Resolved env variable used for configuration: {env}");

var embeddingOptions = new EmbeddingOptions
{
    ApiKey = configuration["OpenAI:ApiKey"]!,
    Model = configuration["OpenAI:Model"]!
};


Console.WriteLine("======================================");
Console.WriteLine(" Enterprise Knowledge Assistant");
Console.WriteLine(" PDF Text Extraction Playground");
Console.WriteLine("======================================");
Console.WriteLine();

Console.Write("Enter PDF file path: ");
string? pdfPath = Console.ReadLine();

if (string.IsNullOrWhiteSpace(pdfPath))
{
    Console.WriteLine("No file path entered.");
    return;
}

if (!File.Exists(pdfPath))
{
    Console.WriteLine($"File not found: {pdfPath}");
    return;
}

using var fs = File.OpenRead(pdfPath);

ITextExtractor extractor = new PdfTextExtractor();

if (!extractor.CanHandle("application/pdf"))
{
    Console.WriteLine("Extractor cannot handle the specified document.");
    return;
}

Console.WriteLine();
Console.WriteLine("Extracting text...");
Console.WriteLine();

var extractResult = await extractor.ExtractTextAsync(fs);

if (!extractResult.Success)
{
    Console.WriteLine($"Extraction failed: {extractResult.ErrorMessage}");
    return;
}

Console.WriteLine($"Pages Extracted : {extractResult.Pages.Count}");

int totalCharacters = extractResult.Pages.Sum(p => p.Text.Length);
Console.WriteLine($"Characters      : {totalCharacters}");

Console.WriteLine();
Console.WriteLine("======================================");
Console.WriteLine("First Page Preview");
Console.WriteLine("======================================");
Console.WriteLine();

if (extractResult.Pages.Any())
{
    Console.WriteLine(extractResult.Pages.First().Text);
}

Console.WriteLine();
Console.WriteLine("Extraction completed successfully.");

var document = new Document
{
    Id = Guid.NewGuid(),
    Name = Path.GetFileName(pdfPath),
    Type = DocumentType.Pdf,
    Status = DocumentStatus.Extracted,
    SourceIdentifier = pdfPath,
    Pages = extractResult.Pages
        .Select(p => new DocumentPage
        {
            PageNumber = p.PageNumber,
            Text = p.Text
        })
        .ToList()
};

IChunker chunker = new FixedSizeChunker(
    chunkSize: 1000,
    overlapSize: 100);

var chunkingResult = await chunker.ChunkAsync(document);

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("Chunking Result");
Console.WriteLine("========================================");

foreach (var chunk in chunkingResult.Chunks)
{
    Console.WriteLine($"Chunk : {chunk.ChunkSequence}");
    Console.WriteLine($"Document : {chunk.DocumentId}");
    Console.WriteLine($"Page : {chunk.StartPage}");
    Console.WriteLine($"Offset : {chunk.StartOffset} - {chunk.EndOffset}");
    Console.WriteLine($"Length : {chunk.Text.Length}");

    Console.WriteLine("----------------------------------------");
    Console.WriteLine(chunk.Text);
    Console.WriteLine("----------------------------------------");
    Console.WriteLine();
}

//OpenAi Embedding
/*
IEmbeddingService embeddingService =
    new OpenAIEmbeddingService(embeddingOptions);

var firstChunk = chunkingResult.Chunks.Take(1);
var embeddingResults =
    await embeddingService.GenerateEmbeddingsAsync(firstChunk, CancellationToken.None);

*/

//Ollama Embedding
var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:11434/")
};

//var ollamaOptions = new OllamaEmbeddingOptions
//{
//    BaseUrl = "http://localhost:11434/",
//    Model = "nomic-embed-text"
//};
var ollamaOptions = configuration
    .GetSection("Ollama").Get<OllamaEmbeddingOptions>()!;


IEmbeddingService embeddingService =
    new OllamaEmbeddingService(httpClient, ollamaOptions);

var chunksToIndex = chunkingResult.Chunks.Take(100).ToList();

//var embeddingResults = await embeddingService.GenerateEmbeddingsAsync(chunkingResult.Chunks.Take(100), CancellationToken.None);
var embeddingResults = await embeddingService.GenerateEmbeddingsAsync(chunksToIndex, CancellationToken.None);

Console.WriteLine();
Console.WriteLine("========== EMBEDDING RESULTS ==========");
Console.WriteLine();

foreach (var result in embeddingResults)
{
    Console.WriteLine($"Success        : {result.Success}");

    if (!result.Success)
    {
        Console.WriteLine($"Error          : {result.ErrorMessage}");
        Console.WriteLine();
        continue;
    }

    Console.WriteLine($"Relative Id    : {result.Chunk.RelativeId}");
    Console.WriteLine($"Document Id    : {result.Chunk.DocumentId}");
    Console.WriteLine($"Chunk Sequence : {result.Chunk.ChunkSequence}");

    Console.WriteLine($"Start Page     : {result.Chunk.StartPage}");
    Console.WriteLine($"End Page       : {result.Chunk.EndPage}");

    Console.WriteLine($"Start Offset   : {result.Chunk.StartOffset}");
    Console.WriteLine($"End Offset     : {result.Chunk.EndOffset}");

    Console.WriteLine($"Model          : {result.Chunk.Embedding?.Model}");
    Console.WriteLine($"Dimensions     : {result.Chunk.Embedding?.Dimensions}");

    Console.WriteLine();
    Console.WriteLine("First 10 Vector Values");

    if (result.Chunk.Embedding != null)
    {
        foreach (var value in result.Chunk.Embedding.Values.Take(10))
        {
            Console.WriteLine(value);
        }
    }
    Console.WriteLine("--------------------------------------------");

    
}

// Asking a question
string question = "What algorithm?";
EmbeddingVector embeddingVector = await embeddingService.GenerateEmbeddingAsync(question, CancellationToken.None);

IVectorStore vectorStore = new InMemoryVectorStore();
await vectorStore.AddRangeAsync(chunksToIndex, CancellationToken.None);
var searchResult = await vectorStore.SearchAsync(embeddingVector.Values, 3, CancellationToken.None);

foreach (var search in searchResult)
{
    Console.WriteLine(new string('-', 80));

    Console.WriteLine($"Score : {search.SimilarityScope:F4}");

    Console.WriteLine($"Chunk : {search.Chunk.RelativeId}");

    Console.WriteLine();

    Console.WriteLine(search.Chunk.Text);

    Console.WriteLine();
}

var promptBuilder = new DefaultPromptBuilder();

var prompt = promptBuilder.BuildPrompt(
    new PromptContext
    {
        Question = question,
        SearchResults = searchResult
    });

Console.WriteLine(prompt);

Console.ReadLine();