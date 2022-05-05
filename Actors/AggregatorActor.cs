using System.Collections.Immutable;
using Akka.Actor;
using Akka.Util.Internal;
using Hafnia.Shared.Messages;

namespace Hafnia.Shared.Actors;

public class AggregatorActor<T> : ReceiveActor where T : Message
{
    private T? _originalMessage;
    private IActorRef? _originalSender;
    private readonly ISet<IActorRef> _refs;

    public AggregatorActor(ISet<IActorRef> refs)
    {
        _refs = refs;

        Context.SetReceiveTimeout(TimeSpan.FromSeconds(30));
        Receive<T>(x =>
        {
            _originalMessage = x;
            _originalSender = Sender;
            refs.ForEach(r => r.Tell(x));
            Become(Aggregating);
        });
    }

    private void Aggregating()
    {
        List<T> replies = new();
        Receive<ReceiveTimeout>(_ => ReplyAndStop(_originalMessage!.RequestId, replies));
        Receive<T>(msg =>
        {
            if (_refs.Remove(Sender))
                replies.Add(msg);

            if (!_refs.Any())
                ReplyAndStop(msg.RequestId, replies);
        });
        Receive<AggregatedReply<T>>(msg =>
        {
            if (_refs.Remove(Sender))
                replies.AddRange(msg.Replies);

            if (!_refs.Any())
                ReplyAndStop(msg.RequestId, replies);
        });
    }

    private void ReplyAndStop(long requestId, List<T> replies)
    {
        _originalSender.Tell(new AggregatedReply<T>(requestId, replies.ToImmutableList()));
        Context.Stop(Self);
    }

    public static Props Props(ISet<IActorRef> actors) => Akka.Actor.Props.Create<AggregatorActor<T>>(actors);
}
