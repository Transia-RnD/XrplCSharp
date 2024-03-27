

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/SHAMap/LeafNode.ts

using Xrpl.Client.Exceptions;

namespace Xrpl.Utils.Hashes.ShaMap
{
    public class LeafNode : Node
    {
        public string Tag { get; set; }
        public NodeType Type { get; set; }
        public string Data { get; set; }

        /// <summary>
        /// Leaf node in a SHAMap tree.
        /// </summary>
        /// <param name="tag">Equates to a ledger entry `index`.</param>
        /// <param name="data">Hex of account state, transaction etc.</param>
        /// <param name="type"> One of TYPE_ACCOUNT_STATE, TYPE_TRANSACTION_MD etc.</param>
        public LeafNode(string tag, string data, NodeType type)
        {
            Tag = tag;
            Data = data;
            Type = type;
        }

        /// <inheritdoc />
        public override void AddItem(string tag, Node node)
        {
            throw new XrplException("Cannot call addItem on a LeafNode");
            //AddItem(tag, node);
        }

        /// <inheritdoc />
        public override string Hash
        {
            get
            {
                switch (Type)
                {
                    case NodeType.ACCOUNT_STATE:
                    {
                        var leafPrefix = HashPrefix.LEAF_NODE.ToString("X");
                        return Sha512HalfUtil.Sha512Half(leafPrefix + Data + Tag);
                    }
                    case NodeType.TRANSACTION_NO_METADATA:
                    {
                        var txIDPrefix = HashPrefix.TRANSACTION_ID.ToString("X");
                        return Sha512HalfUtil.Sha512Half(txIDPrefix + Data);
                    }
                    case NodeType.TRANSACTION_METADATA:
                    {
                        var txNodePrefix = HashPrefix.TRANSACTION_NODE.ToString("X");
                        return Sha512HalfUtil.Sha512Half(txNodePrefix + Data + Tag);
                    }
                    default:
                        throw new XrplException("Tried to hash a SHAMap node of unknown type.");
                }
            }
        }
    }
}

