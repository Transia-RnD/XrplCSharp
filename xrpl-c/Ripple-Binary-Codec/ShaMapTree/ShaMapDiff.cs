using System;
using System.Collections.Generic;
using System.Linq;
using Ripple.Core.Types;

namespace Ripple.Core.ShaMapTree
{
    public class ShaMapDiff
    {
        public readonly ShaMap A, B;
        public readonly SortedSet<Hash256> Modified;
        public readonly SortedSet<Hash256> Deleted;
        public readonly SortedSet<Hash256> Added;

        private ShaMapDiff(ShaMap a,
                           ShaMap b,
                           SortedSet<Hash256> modified = null,
                           SortedSet<Hash256> deleted = null,
                           SortedSet<Hash256> added = null)
        {
            A = a;
            B = b;
            Modified = modified ?? new SortedSet<Hash256>();
            Deleted = deleted ?? new SortedSet<Hash256>();
            Added = added ?? new SortedSet<Hash256>();
        }

        public static ShaMapDiff Find(ShaMap a, ShaMap b)
        {
            var diff = new ShaMapDiff(a, b);
            diff.Find();
            return diff;
        }

        // Find what's added, modified and deleted in `B`
        private void Find()
        {
            A.Hash();
            B.Hash();
            Compare(A, B);
        }

        public ShaMapDiff Inverted()
        {
            return new ShaMapDiff(B, A, added: Deleted, deleted: Added, modified: Modified);
        }

        public void Apply(ShaMap sa)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var mod in Modified)
            {
                var modded = sa.UpdateItem(mod, B.GetItem(mod).Copy());
                if (!modded)
                {
                    throw new InvalidOperationException();
                }
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var add in Added)
            {
                var added = sa.AddItem(add, B.GetItem(add).Copy());
                if (!added)
                {
                    throw new InvalidOperationException();
                }
            }
            if (Deleted.Select(sa.RemoveLeaf).Any(removed => !removed))
            {
                throw new InvalidOperationException();
            }
        }
        private void Compare(ShaMapInner a, ShaMapInner b)
        {
            for (var i = 0; i < 16; i++)
            {
                var aChild = a.GetBranch(i);
                var bChild = b.GetBranch(i);

                if (aChild == null && bChild != null)
                {
                    TrackAdded(bChild);
                    // added in B
                }
                else if (aChild != null && bChild == null)
                {
                    TrackRemoved(aChild);
                    // removed from B
                }
                else if (aChild != null && !aChild.Hash().Equals(bChild.Hash()))
                {
                    bool aleaf = aChild.IsLeaf,
                         bLeaf = bChild.IsLeaf;

                    if (aleaf && bLeaf)
                    {
                        var la = aChild.AsLeaf();
                        var lb = bChild.AsLeaf();
                        if (la.Index.Equals(lb.Index))
                        {
                            Modified.Add(la.Index);
                        }
                        else
                        {
                            Deleted.Add(la.Index);
                            Added.Add(lb.Index);
                        }
                    }
                    else if (aleaf)
                    { //&& bInner
                        var la = aChild.AsLeaf();
                        var ib = bChild.AsInner();
                        TrackAdded(ib);

                        if (ib.HasLeaf(la.Index))
                        {
                            // because trackAdded would have added it
                            Added.Remove(la.Index);
                            var leaf = ib.GetLeaf(la.Index);
                            if (!leaf.Hash().Equals(la.Hash()))
                            {
                                Modified.Add(la.Index);
                            }
                        }
                        else
                        {
                            Deleted.Add(la.Index);
                        }
                    }
                    else if (bLeaf)
                    { //&& aInner
                        var lb = bChild.AsLeaf();
                        var ia = aChild.AsInner();
                        TrackRemoved(ia);

                        if (ia.HasLeaf(lb.Index))
                        {
                            // because trackRemoved would have deleted it
                            Deleted.Remove(lb.Index);
                            var leaf = ia.GetLeaf(lb.Index);
                            if (!leaf.Hash().Equals(lb.Hash()))
                            {
                                Modified.Add(lb.Index);
                            }
                        }
                        else
                        {
                            Added.Add(lb.Index);
                        }
                    }
                    else //if (aInner && bInner)
                    {
                        Compare(aChild.AsInner(), bChild.AsInner());
                    }
                }
            }
        }
        private void TrackRemoved(ShaMapNode child)
        {
            child.WalkAnyLeaves(leaf => Deleted.Add(leaf.Index));
        }

        private void TrackAdded(ShaMapNode child)
        {
            child.WalkAnyLeaves(leaf => Added.Add(leaf.Index));
        }
    }

}
