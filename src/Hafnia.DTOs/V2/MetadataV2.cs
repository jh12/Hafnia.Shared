namespace Hafnia.DTOs.V2;

public record MetadataV2
(
    string Id,
    string Title,
    string[] Tags
);

public record MetadataSourceV2
(
    string Id,
    string Uri,
    string Title,
    string? CreatorId,
    string[] Tags
);

public record MetadataWithSourceV2
(
    string Id,
    string Title,
    string[] Tags,
    MetadataSourceV2 Source
) : MetadataV2(Id, Title, Tags);
