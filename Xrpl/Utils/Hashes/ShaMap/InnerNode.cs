

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/SHAMap/InnerNode.ts

using System.Collections.Generic;

using Xrpl.Client.Exceptions;

namespace Xrpl.Utils.Hashes.ShaMap
{
    public class InnerNode : Node
    {
        public readonly string HEX_ZERO = "0000000000000000000000000000000000000000000000000000000000000000";

        public const int SLOT_MAX = 15;
        public const int HEX = 16;

        public Dictionary<int, Node> Leaves { get; set; }
        public NodeType Type { get; set; }
        public int Depth { get; set; }
        public bool Empty { get; set; }

        public InnerNode(int depth = 0)
        {
            Leaves = new Dictionary<int, Node>();
            Type = NodeType.INNER;
            Depth = depth;
            Empty = true;
        }

        /// <inheritdoc />
        public override void AddItem(string tag, Node node)
        {
            var existingNode = GetNode(int.Parse($"{tag[Depth]}", System.Globalization.NumberStyles.HexNumber));

            if (existingNode == null)
            {
                SetNode(int.Parse($"{tag[Depth]}", System.Globalization.NumberStyles.HexNumber), node);
                return;
            }

            if (existingNode is InnerNode)
            {
                existingNode.AddItem(tag, node);
            }
            else if (existingNode is LeafNode leaf)
            {
                if (leaf.Tag == tag)
                {
                    throw new XrplException("Tried to add a node to a SHAMap that was already in there.");
                }
                else
                {
                    var newInnerNode = new InnerNode(Depth + 1);
                    newInnerNode.AddItem(leaf.Tag, leaf);
                    newInnerNode.AddItem(tag, node);
                    SetNode(int.Parse($"{tag[Depth]}", System.Globalization.NumberStyles.HexNumber), newInnerNode);
                }
            }
        }
        /// <summary>
        /// Overwrite the node that is currently in a given slot.
        /// </summary>
        /// <param name="slot">A number 0-15.</param>
        /// <param name="node">To place.</param>
        /// <exception cref="XrplException">If slot is out of range.</exception>
        public void SetNode(int slot, Node node)
        {
            if (slot is < 0 or > SLOT_MAX)
            {
                throw new XrplException("Invalid slot: slot must be between 0-15.");
            }
            Leaves[slot] = node;
            Empty = false;
        }
        /// <summary>
        /// Get the node that is currently in a given slot.
        /// </summary>
        /// <param name="slot">A number 0-15.</param>
        /// <returns>Node currently in a slot.</returns>
        /// <exception cref="XrplException">If slot is out of range.</exception>
        public Node GetNode(int slot)
        {
            if (slot is < 0 or > SLOT_MAX)
            {
                throw new XrplException("Invalid slot: slot must be between 0-15.");
            }
            return Leaves[slot];
        }

        /// <inheritdoc />
        public override string Hash
        {
            get
            {
                if (Empty)
                {
                    return HEX_ZERO;
                }

                string hex = "";
                for (var iter = 0; iter <= SLOT_MAX; iter++)
                {
                    Node child = Leaves[iter];
                    string hash = child == null ? HEX_ZERO : child.Hash;
                    hex += hash;
                }

                string prefix = HashPrefix.INNER_NODE.ToString("X");
                return Sha512HalfUtil.Sha512Half(prefix + hex);
            }
        }
    }
}

