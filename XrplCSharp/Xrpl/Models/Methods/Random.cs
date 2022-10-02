//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/random.ts
namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The random command provides a random number to be used as a source of  entropy for random number generation by clients.<br/>
    /// Expects a response in the  form of a RandomResponse.
    /// </summary>
    public class RandomRequest : RippleRequest
    {
        public RandomRequest()
        {
            Command = "random";
        }
    }
    //todo not found  RippleRequest extends BaseResponse
    //https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/random.ts#L19

}
