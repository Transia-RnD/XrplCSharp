//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/ping.ts
namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The ping command returns an acknowledgement, so that clients can test the  connection status and latency.<br/>
    /// Expects a response in the form of a PingRequest.
    /// </summary>
    public class PingRequest : RippleRequest
    {
        public PingRequest()
        {
            Command = "ping";
        }
    }
    //todo not found  PingResponse extends BaseResponse
    //https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/ping.ts#L19
}
