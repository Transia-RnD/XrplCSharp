namespace xrpl_c.Xrpl.Client.Models.Subscriptions
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
