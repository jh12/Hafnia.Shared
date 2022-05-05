using System.Collections.Concurrent;
using Akka.Actor;
using Akka.Event;
using Hafnia.Shared.Messages;

namespace Hafnia.Shared.Actors;

public abstract class SupervisorBase : ReceiveActor
{
    private long _nextRequestId;
    private ConcurrentDictionary<long, AwaitingMessage> _awaitingMessages = new();

    protected ILoggingAdapter Log { get; } = Context.GetLogger();

    protected ISet<IActorRef> SupervisedActorRefs { get; } = new HashSet<IActorRef>();

    public SupervisorBase()
    {
        Receive<AggregatedReply<Message>>(OnReceiveAny);
    }

    private void OnReceiveAny(AggregatedReply<Message> msg)
    {
        if (_awaitingMessages.TryRemove(msg.RequestId, out AwaitingMessage? awaitingMsg))
        {
            AggregatedReply<Message> reply = msg with { RequestId = awaitingMsg.RequestId };

            awaitingMsg.Sender.Tell(reply);
            return;
        }

        Log.Error("Received unhandled message");
    }

    protected void TellAndReply(Message message)
    {
        long requestId = Interlocked.Increment(ref _nextRequestId);

        AwaitingMessage awaitingMessage = new AwaitingMessage(requestId, Sender, message);
        _awaitingMessages.AddOrUpdate(requestId, l => awaitingMessage, (_, existingMsg) => existingMsg);

        Message localScopedMessage = message with { RequestId = requestId };

        IActorRef actorRef = Context.ActorOf(AggregatorActor<Message>.Props(SupervisedActorRefs));
        actorRef.Tell(localScopedMessage);
    }

    // TODO: Necessary?
    private record AwaitingMessage(long RequestId, IActorRef Sender, Message Message);
}
