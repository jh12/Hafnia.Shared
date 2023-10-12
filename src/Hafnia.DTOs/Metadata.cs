namespace Hafnia.DTOs;

public record Metadata
(
    string Id,
    string? OriginalId,
    Uri Uri,
    string Title,
    string[] Tags
);
