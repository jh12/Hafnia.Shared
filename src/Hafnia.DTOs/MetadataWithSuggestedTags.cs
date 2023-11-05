namespace Hafnia.DTOs;

public record MetadataWithSuggestedTags
(
    string Id,
    string? OriginalId,
    Uri Uri,
    string Title,
    string[] Tags,
    string[] SuggestedTags
) : 
Metadata
(
    Id,
    OriginalId,
    Uri,
    Title,
    Tags
);
