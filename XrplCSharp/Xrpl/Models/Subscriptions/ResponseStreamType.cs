namespace Xrpl.Models.Subscriptions
{
    public enum ResponseStreamType
    {
        UNKNOWN,
        response,
        connected,
        disconnected,
        ledgerClosed,
        validationReceived,
        transaction,
        peerStatusChange,
        consensusPhase,
        path_find,
        error
    }
}
