namespace Ripple.Binary.Codec.Binary
{
    public interface IBytesSink
    {
        void Put(byte aByte);
        void Put(byte[] bytes);
    }
}