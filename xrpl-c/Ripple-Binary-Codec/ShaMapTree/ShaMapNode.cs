using Ripple.Core.Binary;
using Ripple.Core.Hashing;
using Ripple.Core.Types;

namespace Ripple.Core.ShaMapTree
{
    public abstract class ShaMapNode
    {
        protected Hash256 CachedHash;

        public abstract bool IsLeaf { get; }
        public abstract bool IsInner { get; }

        public ShaMapLeaf AsLeaf()
        {
            return (ShaMapLeaf)this;
        }

        public ShaMapInner AsInner()
        {
            return (ShaMapInner)this;
        }

        internal abstract HashPrefix Prefix();
        public abstract void ToBytesSink(IBytesSink sink);

        public void Invalidate()
        {
            CachedHash = null;
        }
        public virtual Hash256 Hash()
        {
            return CachedHash ?? (CachedHash = CreateHash());
        }

        public Hash256 CreateHash()
        {
            var half = new Sha512(Prefix().Bytes());
            ToBytesSink(half);
            return new Hash256(half.Finish256());
        }

        /// <summary>
        /// Walk any leaves, possibly this node itself, if it's terminal.
        /// </summary>
        public void WalkAnyLeaves(OnLeaf leafWalker)
        {
            if (IsLeaf)
            {
                leafWalker(AsLeaf());
            }
            else
            {
                AsInner().WalkLeaves(leafWalker);
            }
        }
    }
}

