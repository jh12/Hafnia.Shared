namespace Hafnia.DTOs;

public record MetadataWork
(
    string Id,
    string MetadataId,
    string Origin,
    bool Complete,
    DateTime UpdatedAt,
    string JsonData
);
