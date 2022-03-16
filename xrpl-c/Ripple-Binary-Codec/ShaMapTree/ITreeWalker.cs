namespace Ripple.Core.ShaMapTree
{
    public interface ILeafWalker
    {
        void OnLeaf(ShaMapLeaf leaf);
    }
    public interface ITreeWalker : ILeafWalker
    {
        void OnInner(ShaMapInner inner);
    }

    public class TreeWalker : ITreeWalker
    {
        public void OnLeaf(ShaMapLeaf leaf)
        {

        }
        public void OnInner(ShaMapInner inner)
        {

        }
    }
}