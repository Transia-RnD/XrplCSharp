

using System;
using System.Text.Json.Nodes;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/ledger-hashes.ts

namespace Xrpl.BinaryCodec
{
  public static class LedgerHashes {
    
    public static Hash256 ComputeHash(
        Func<JsonObject, (Hash256?, ShaMapNode?, ShaMapLeaf?)> itemizer,
        JsonObject[] itemsJson)
    {
        var map = new ShaMap();
        foreach (var item in itemsJson)
        {
            map.AddItem(itemizer(item));
        }
        return map.Hash();
    }

    public static Hash256 TransactionTreeHash(JsonObject[] param)
    {
        return ComputeHash(TransactionItemizer, param);
    }

    public static Hash256 AccountStateHash(JsonObject[] param)
    {
        return ComputeHash(EntryItemizer, param);
    }

    public static Hash256 LedgerHash(JsonObject header)
    {
        var hash = new Sha512Half();
        hash.Put(HashPrefix.LedgerHeader);
        if (header["parent_close_time"] == null)
        {
            throw new Exception("parent_close_time must be defined");
        }
        if (header["close_flags"] == null)
        {
            throw new Exception("close_flags must be defined");
        }
        UInt32.From((int)header["ledger_index"]).ToBytesSink(hash);
        UInt64.From(BigInteger.Parse(header["total_coins"].ToString())).ToBytesSink(hash);
        Hash256.From((string)header["parent_hash"]).ToBytesSink(hash);
        Hash256.From((string)header["transaction_hash"]).ToBytesSink(hash);
        Hash256.From((string)header["account_hash"]).ToBytesSink(hash);
        UInt32.From((int)header["parent_close_time"]).ToBytesSink(hash);
        UInt32.From((int)header["close_time"]).ToBytesSink(hash);
        UInt8.From((byte)header["close_time_resolution"]).ToBytesSink(hash);
        UInt8.From((byte)header["close_flags"]).ToBytesSink(hash);
        return hash.Finish();
    }

    public static JsonObject DecodeLedgerData(string binary)
    {
        if (typeof(binary) != typeof(string))
        {
            throw new Exception("binary must be a hex string");
        }
        var parser = new BinaryParser(binary);
        return new JsonObject
        {
            ["ledger_index"] = parser.ReadUInt32(),
            ["total_coins"] = parser.ReadType(UInt64).ValueOf().ToString(),
            ["parent_hash"] = parser.ReadType(Hash256).ToHex(),
            ["transaction_hash"] = parser.ReadType(Hash256).ToHex(),
            ["account_hash"] = parser.ReadType(Hash256).ToHex(),
            ["parent_close_time"] = parser.ReadUInt32(),
            ["close_time"] = parser.ReadUInt32(),
            ["close_time_resolution"] = parser.ReadUInt8(),
            ["close_flags"] = parser.ReadUInt8(),
        };
    }

    private static (Hash256?, ShaMapNode?, ShaMapLeaf?) TransactionItemizer(JsonObject json)
    {
        if (json["hash"] == null)
        {
            throw new Exception("hash must be defined");
        }
        var index = Hash256.From((string)json["hash"]);
        var item = new ShaMapNode
        {
            HashPrefix = () => HashPrefix.Transaction,
            ToBytesSink = sink =>
            {
                var serializer = new BinarySerializer(sink);
                serializer.WriteLengthEncoded(STObject.From(json));
                serializer.WriteLengthEncoded(STObject.From((JsonObject)json["metaData"]));
            }
        };
        return (index, item, null);
    }

    private static (Hash256?, ShaMapNode?, ShaMapLeaf?) EntryItemizer(JsonObject json)
    {
        var index = Hash256.From((string)json["index"]);
        var bytes = SerializeObject(json);
        var item = new ShaMapNode
        {
            HashPrefix = () => HashPrefix.AccountStateEntry,
            ToBytesSink = sink => sink.Put(bytes)
        };
        return (index, item, null);
    }
  }
}