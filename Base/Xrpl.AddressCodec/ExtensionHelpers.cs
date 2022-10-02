using Org.BouncyCastle.Utilities.Encoders;

public static class ExtensionHelpers
{
    public static string ToHex(this byte[] input)
    {
        return Hex.ToHexString(input).ToUpper();
    }

    public static byte[] FromHex(this string input)
    {
        return Hex.Decode(input);
    }
}