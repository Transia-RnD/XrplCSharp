using Xrpl.AddressCodec;
using Xrpl.Client.Exceptions;

namespace Xrpl.Sugar
{
    public class Utils
    {
        /// <summary>
        /// If an address is an X-Address, converts it to a classic address.
        /// </summary>
        /// <param name="account">A classic address or X-address.</param>
        /// <returns>The account's classic address.</returns>
        public static string EnsureClassicAddress(string account)
        {
            if (XrplAddressCodec.IsValidXAddress(account))
            {
                var codec = XrplAddressCodec.XAddressToClassicAddress(account);
                if (codec.Tag is not null)
                    throw new RippleException("This command does not support the use of a tag. Use an address without a tag."); 
                
                // For rippled requests that use an account, always use a classic address.
                return codec.ClassicAddress;
            }
            return account;
        }
    }
}
