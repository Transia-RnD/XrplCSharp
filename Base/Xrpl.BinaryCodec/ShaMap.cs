using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/sha-map.ts

namespace Xrpl.BinaryCodec
{
    public abstract class ShaMapNode
    {
        public abstract byte[] HashPrefix();
        public abstract bool IsLeaf();
        public abstract bool IsInner();
        public abstract void ToBytesSink(BytesList list);
        public abstract Hash256 Hash();
    }

    public class ShaMapLeaf : ShaMapNode
    {
        public Hash256 Index { get; }
        public ShaMapNode Item { get; }

        public ShaMapLeaf(Hash256 index, ShaMapNode item)
        {
            Index = index;
            Item = item;
        }

        public override bool IsLeaf()
        {
            return true;
        }

        public override bool IsInner()
        {
            return false;
        }

        public override byte[] HashPrefix()
        {
            return Item == null ? new byte[0] : Item.HashPrefix();
        }

        public override Hash256 Hash()
        {
            var hash = Sha512Half.put(HashPrefix());
            ToBytesSink(hash);
            return hash.Finish();
        }

        public override void ToBytesSink(BytesList list)
        {
            if (Item != null)
            {
                Item.ToBytesSink(list);
            }
            Index.ToBytesSink(list);
        }
    }

    public class ShaMapInner : ShaMapNode
    {
        private int slotBits = 0;
        private ShaMapNode[] branches = new ShaMapNode[16];

        private int depth = 0;

        public ShaMapInner(int depth = 0)
        {
            this.depth = depth;
        }

        public override bool IsInner()
        {
            return true;
        }

        public override bool IsLeaf()
        {
            return false;
        }

        public override byte[] HashPrefix()
        {
            return HashPrefix.InnerNode;
        }

        public void SetBranch(int slot, ShaMapNode branch)
        {
            this.slotBits = this.slotBits | (1 << slot);
            this.branches[slot] = branch;
        }

        public bool Empty()
        {
            return this.slotBits == 0;
        }

        public override Hash256 Hash()
        {
            if (this.Empty())
            {
                return Hash256.Zero256;
            }

            var hash = Sha512Half.put(this.HashPrefix());
            this.ToBytesSink(hash);
            return hash.Finish();
        }

        public override void ToBytesSink(BytesList list)
        {
            for (int i = 0; i < this.branches.Length; i++)
            {
                var branch = this.branches[i];
                var hash = branch != null ? branch.Hash() : Hash256.Zero256;
                hash.ToBytesSink(list);
            }
        }

        public void AddItem(Hash256 index, ShaMapNode item, ShaMapLeaf leaf)
        {
            Debug.Assert(index != null);
            var nibble = index.Nibblet(this.depth);
            var existing = this.branches[nibble];

            if (existing == null)
            {
                this.SetBranch(nibble, leaf ?? new ShaMapLeaf(index, item));
            }
            else if (existing is ShaMapLeaf)
            {
                var newInner = new ShaMapInner(this.depth + 1);
                newInner.AddItem(((ShaMapLeaf)existing).Index, null, (ShaMapLeaf)existing);
                newInner.AddItem(index, item, leaf);
                this.SetBranch(nibble, newInner);
            }
            else if (existing is ShaMapInner)
            {
                ((ShaMapInner)existing).AddItem(index, item, leaf);
            }
            else
            {
                throw new Exception("invalid ShaMap.addItem call");
            }
        }
    }

    public class ShaMap : ShaMapInner
    {
        public ShaMap(int depth) : base(depth)
        {
        }
    }
}

