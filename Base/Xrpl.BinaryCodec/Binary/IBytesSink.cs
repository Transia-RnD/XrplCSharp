namespace Xrpl.BinaryCodec.Binary
{
    /// <summary> base Bytes Sink </summary>
    public interface IBytesSink
    {
        /// <summary> Write bytes to this BinarySerializer </summary>
        /// <param name="aByte">byte to write</param>
        void Put(byte aByte);
        /// <summary>
        /// Write bytes to this BinarySerializer
        /// </summary>
        /// <param name="bytes">the bytes to write</param>
        void Put(byte[] bytes);
    }
}