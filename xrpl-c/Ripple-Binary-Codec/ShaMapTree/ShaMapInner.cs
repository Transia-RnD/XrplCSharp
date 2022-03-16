using System;
using System.Diagnostics;
using System.Linq;
using Ripple.Core.Binary;
using Ripple.Core.Hashing;
using Ripple.Core.Types;

namespace Ripple.Core.ShaMapTree
{
    public class ShaMapInner : ShaMapNode
    {
        public int Depth;
        internal int SlotBits;
        internal int Version;
        internal bool DoCoW;
        protected internal ShaMapNode[] Branches = new ShaMapNode[16];

        public ShaMapInner(int depth) : this(false, depth, 0)
        {
        }

        public ShaMapInner(bool isCopy, int depth, int version) {
            DoCoW = isCopy;
            Depth = depth;
            Version = version;
        }

        protected internal ShaMapInner Copy(int version)
        {
            var copy = MakeInnerOfSameClass(Depth);
            Array.Copy(Branches, 0, copy.Branches, 0, Branches.Length);
            copy.SlotBits = SlotBits;
            copy.CachedHash = CachedHash;
            copy.Version = version;
            DoCoW = true;

            return copy;
        }

        protected internal virtual ShaMapInner MakeInnerOfSameClass(int depth)
        {
            return new ShaMapInner(true, depth, Version);
        }

        protected internal ShaMapInner MakeInnerChild()
        {
            var childDepth = Depth + 1;
            if (childDepth >= 64)
            {
                throw new InvalidOperationException();
            }
            return new ShaMapInner(DoCoW, childDepth, Version);
        }

        // Descend into the tree, find the leaf matching this index
        // and if the tree has it.
        protected internal void SetLeaf(ShaMapLeaf leaf)
        {
            if (leaf.Version == -1)
            {
                leaf.Version = Version;
            }
            SetBranch(leaf.Index, leaf);
        }

        private void RemoveBranch(Hash256 index)
        {
            RemoveBranch(SelectBranch(index));
        }

        public void WalkLeaves(OnLeaf leafWalker)
        {
            foreach (var branch in Branches.Where(branch => branch != null))
            {
                if (branch.IsInner)
                {
                    branch.AsInner().WalkLeaves(leafWalker);
                }
                else if (branch.IsLeaf)
                {
                    leafWalker(branch.AsLeaf());
                }
            }
        }

        public virtual void WalkTree(ITreeWalker treeWalker)
        {
            treeWalker.OnInner(this);
            foreach (var branch in Branches.Where(branch => branch != null))
            {
                if (branch.IsLeaf)
                {
                    treeWalker.OnLeaf(branch.AsLeaf());
                }
                else if (branch.IsInner)
                {
                    branch.AsInner().WalkTree(treeWalker);
                }
            }
        }

        /// <returns> the `only child` leaf or null if other children </returns>
        public ShaMapLeaf OnlyChildLeaf()
        {
            ShaMapLeaf leaf = null;
            var leaves = 0;

            foreach (var branch in Branches.Where(branch => branch != null))
            {
                if (branch.IsInner)
                {
                    leaf = null;
                    break;
                }
                if (++leaves == 1)
                {
                    leaf = branch.AsLeaf();
                }
                else
                {
                    leaf = null;
                    break;
                }
            }
            return leaf;
        }

        public bool RemoveLeaf(Hash256 index)
        {
            var path = PathToIndex(index);
            if (!path.HasMatchedLeaf()) return false;
            var top = path.DirtyOrCopyInners();
            top.RemoveBranch(index);
            path.CollapseOnlyLeafChildInners();
            return true;
        }

        public IShaMapItem<object> GetItem(Hash256 index)
        {
            return GetLeaf(index)?.Item;
        }

        public bool AddItem(Hash256 index, IShaMapItem<object> item)
        {
            return AddLeaf(new ShaMapLeaf(index, item));
        }

        public bool UpdateItem(Hash256 index, IShaMapItem<object> item)
        {
            return UpdateLeaf(new ShaMapLeaf(index, item));
        }

        public bool HasLeaf(Hash256 index)
        {
            return PathToIndex(index).HasMatchedLeaf();
        }

        public ShaMapLeaf GetLeaf(Hash256 index)
        {
            var stack = PathToIndex(index);
            return stack.HasMatchedLeaf() ? stack.Leaf : null;
        }

        public bool AddLeaf(ShaMapLeaf leaf)
        {
            var stack = PathToIndex(leaf.Index);
            if (stack.HasMatchedLeaf())
            {
                return false;
            }
            var top = stack.DirtyOrCopyInners();
            top.AddLeafToTerminalInner(leaf);
            return true;
        }

        public bool UpdateLeaf(ShaMapLeaf leaf)
        {
            var stack = PathToIndex(leaf.Index);
            if (!stack.HasMatchedLeaf()) return false;
            var top = stack.DirtyOrCopyInners();
            // Why not update in place? Because of structural sharing
            top.SetLeaf(leaf);
            return true;
        }

        public PathToIndex PathToIndex(Hash256 index)
        {
            return new PathToIndex(this, index);
        }

        /// <summary>
        /// This should only be called on the deepest inners, as it
        /// does not do any dirtying. </summary>
        /// <param name="leaf"> to add to inner </param>
        internal void AddLeafToTerminalInner(ShaMapLeaf leaf)
        {
            var branch = GetBranch(leaf.Index);
            if (branch == null)
            {
                SetLeaf(leaf);
            }
            else if (branch.IsInner)
            {
                throw new InvalidOperationException();
            }
            else if (branch.IsLeaf)
            {
                var inner = MakeInnerChild();
                SetBranch(leaf.Index, inner);
                inner.AddLeafToTerminalInner(leaf);
                inner.AddLeafToTerminalInner(branch.AsLeaf());
            }
        }

        protected internal void SetBranch(Hash256 index, ShaMapNode node)
        {
            SetBranch(SelectBranch(index), node);
        }

        protected internal ShaMapNode GetBranch(Hash256 index)
        {
            return GetBranch(index.Nibblet(Depth));
        }

        public ShaMapNode GetBranch(int i)
        {
            return Branches[i];
        }

        public ShaMapNode Branch(int i)
        {
            return Branches[i];
        }

        protected internal int SelectBranch(Hash256 index)
        {
            return index.Nibblet(Depth);
        }

        public bool HasLeaf(int i)
        {
            return Branches[i].IsLeaf;
        }
        public bool HasInner(int i)
        {
            return Branches[i].IsInner;
        }
        public bool HasNone(int i)
        {
            return Branches[i] == null;
        }

        private void SetBranch(int slot, ShaMapNode node)
        {
            SlotBits = SlotBits | (1 << slot);
            Branches[slot] = node;
            Invalidate();
        }

        private void RemoveBranch(int slot)
        {
            Branches[slot] = null;
            SlotBits = SlotBits & ~(1 << slot);
        }
        public bool Empty()
        {
            return SlotBits == 0;
        }

        public override bool IsInner => true;
        public override bool IsLeaf => false;

        internal override HashPrefix Prefix()
        {
            return HashPrefix.InnerNode;
        }

        public override void ToBytesSink(IBytesSink sink)
        {
            foreach (var branch in Branches)
            {
                if (branch != null)
                {
                    branch.Hash().ToBytes(sink);
                }
                else
                {
                    Hash256.Zero.ToBytes(sink);
                }
            }
        }

        public override Hash256 Hash()
        {
            if (Empty())
            {
                // empty inners have a hash of all Zero
                // it's only valid for a root node to be empty
                // any other inner node, must contain at least a
                // single leaf
                Debug.Assert(Depth == 0);
                return Hash256.Zero;
            }
            // hash the hashPrefix() and toBytesSink
            return base.Hash();
        }

        public ShaMapLeaf GetLeafForUpdating(Hash256 leaf)
        {
            var path = PathToIndex(leaf);
            if (path.HasMatchedLeaf())
            {
                return path.InvalidatedPossiblyCopiedLeafForUpdating();
            }
            return null;
        }

        public int BranchCount()
        {
            return Branches.Count(branch => branch != null);
        }
    }
}