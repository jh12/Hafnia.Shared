namespace Hafnia.Shared.Messages.Social;

public record SocialEventMessage(
    long RequestId,
    string? Title,
    string Content,
    DateTime CreatedAt,
    DateTime? ModifiedAt
) : Message(RequestId);
