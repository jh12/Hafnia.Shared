using System.Collections.Immutable;

namespace Hafnia.Shared.Messages;

public record AggregatedReply<T>(long RequestId, IImmutableList<T> Replies) where T : Message
{
    public void Deconstruct(out long RequestId, out IImmutableList<T> Replies)
    {
        RequestId = this.RequestId;
        Replies = this.Replies;
    }

    public AggregatedReply<T> Combine(AggregatedReply<T> toCombine)
    {
        if (RequestId != toCombine.RequestId)
            throw new InvalidOperationException($"{nameof(RequestId)} must be identical");

        return this with
        {
            Replies = Replies.Union(toCombine.Replies).ToImmutableList()
        };
    }
}
