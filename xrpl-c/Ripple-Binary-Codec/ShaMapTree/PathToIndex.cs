using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ripple.Core.Types;

namespace Ripple.Core.ShaMapTree
{
    public class PathToIndex
    {
        public Hash256 Index;
        public ShaMapLeaf Leaf;

        private LinkedList<ShaMapInner> _inners;
        private ShaMapInner[] _dirtied;
        private bool _matched;

        public bool HasLeaf()
        {
            return Leaf != null;
        }
        public bool LeafMatchedIndex()
        {
            return _matched;
        }
        public bool CopyLeafOnUpdate()
        {
            return Leaf.Version != _dirtied[0].Version;
        }

        internal int Size()
        {
            return _inners.Count;
        }

        public ShaMapInner Top()
        {
            return _dirtied[_dirtied.Length - 1];
        }

        // returns the
        public ShaMapInner DirtyOrCopyInners()
        {
            if (MaybeCopyOnWrite())
            {
                var ix = 0;
                // We want to make a uniformly accessed array of the inners
                _dirtied = new ShaMapInner[_inners.Count];
                IEnumerator<ShaMapInner> it = _inners.GetEnumerator();
                var top = it.Current;
                _dirtied[ix++] = top;
                top.Invalidate();

                while (it.MoveNext())
                {
                    var next = it.Current;
                    var doCopies = next.Version != top.Version;

                    if (doCopies)
                    {
                        var copy = next.Copy(top.Version);
                        copy.Invalidate();
                        top.SetBranch(Index, copy);
                        next = copy;
                    }
                    else
                    {
                        next.Invalidate();
                    }
                    top = next;
                    _dirtied[ix++] = top;
                }
                return top;
            }
            CopyInnersToDirtiedArray();
            return _inners.Last.Value;
        }

        public bool HasMatchedLeaf()
        {
            return HasLeaf() && LeafMatchedIndex();
        }

        public void CollapseOnlyLeafChildInners()
        {
            Debug.Assert(_dirtied != null);
            ShaMapLeaf onlyChild = null;

            for (var i = _dirtied.Length - 1; i >= 0; i--)
            {
                var next = _dirtied[i];
                if (onlyChild != null)
                {
                    next.SetLeaf(onlyChild);
                }
                onlyChild = next.OnlyChildLeaf();
                if (onlyChild == null)
                {
                    break;
                }
            }
        }

        private void CopyInnersToDirtiedArray()
        {
            var ix = 0;
            _dirtied = new ShaMapInner[_inners.Count];
            IEnumerator<ShaMapInner> descending = _inners.GetEnumerator();
            while (descending.MoveNext())
            {
                ShaMapInner next = descending.Current;
                _dirtied[ix++] = next;
                next.Invalidate();
            }
        }

        private bool MaybeCopyOnWrite()
        {
            return _inners.Last().DoCoW;
        }

        public PathToIndex(ShaMapInner root, Hash256 index)
        {
            Index = index;
            MakeStack(root, index);
        }

        private void MakeStack(ShaMapInner root, Hash256 index)
        {
            _inners = new LinkedList<ShaMapInner>();
            var top = root;

            while (true)
            {
                _inners.AddLast(top);
                var existing = top.GetBranch(index);
                if (existing == null)
                {
                    break;
                }
                if (existing.IsLeaf)
                {
                    Leaf = existing.AsLeaf();
                    _matched = Leaf.Index.Equals(index);
                    break;
                }
                if (existing.IsInner)
                {
                    top = existing.AsInner();
                }
            }
        }

        public ShaMapLeaf InvalidatedPossiblyCopiedLeafForUpdating()
        {
            Debug.Assert(_matched);
            if (_dirtied == null)
            {
                DirtyOrCopyInners();
            }
            var theLeaf = Leaf;

            if (CopyLeafOnUpdate())
            {
                theLeaf = Leaf.Copy();
                Top().SetLeaf(theLeaf);
            }
            theLeaf.Invalidate();
            return theLeaf;
        }
    }
}