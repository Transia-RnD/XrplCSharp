

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/SHAMap/index.ts

namespace Xrpl.Utils.Hashes.ShaMap
{
    public class SHAMap
    {
        public InnerNode Root { get; set; }

        /// SHAMap tree constructor.
        public SHAMap()
        {
            Root = new InnerNode(0);
        }

        /// Add an item to the SHAMap.
        /// @param tag - Index of the Node to add.
        /// @param data - Data to insert into the tree.
        /// @param type - Type of the node to add.
        public void AddItem(string tag, string data, NodeType type)
        {
            Root.AddItem(tag, new LeafNode(tag, data, type));
        }

        /// Get the hash of the SHAMap.
        /// @returns The hash of the root of the SHAMap.
        public string Hash => this.Root.Hash;
    }
}

