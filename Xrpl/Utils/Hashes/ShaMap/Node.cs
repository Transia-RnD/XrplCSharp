// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/SHAMap/node.ts

namespace Xrpl.Utils.Hashes.ShaMap
{
    /// <summary>
    /// Abstract base class for SHAMapNode.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Adds an item to the InnerNode.
        /// </summary>
        /// <param name="tag">Equates to a ledger entry `index`.</param>
        /// <param name="node">Node to add.</param>
        public abstract void AddItem(string tag, Node node);
        /// <summary>
        /// Get the hash of a LeafNode.
        /// </summary>
        /// <returns> Hash of the LeafNode.</returns>
        public abstract string Hash { get; }
    }
    public enum NodeType
    {
        INNER = 1,
        TRANSACTION_NO_METADATA = 2,
        TRANSACTION_METADATA = 3,
        ACCOUNT_STATE = 4,
    }
}

