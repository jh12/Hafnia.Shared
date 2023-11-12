namespace Hafnia.DTOs;

public record Collection
(
    string Id,
    string Name,
    string ThumbnailId,
    string[] IncludedTags,
    string[] ExcludedTags,
    string[] Children,
    bool IsRoot
);
