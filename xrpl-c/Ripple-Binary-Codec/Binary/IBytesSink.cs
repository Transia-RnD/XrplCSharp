namespace Ripple.Core.Binary
{
    public interface IBytesSink
    {
        void Put(byte aByte);
        void Put(byte[] bytes);
    }
}