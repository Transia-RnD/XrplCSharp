using System;
using Ripple.Core.Ledger;
using Ripple.Core.ShaMapTree;
using Ripple.Core.Types;

namespace Ripple.Core.Tests
{
    public class HistoryLoader
    {
        private AccountState _state;
        private TransactionTree _transactions;
        private LedgerHeader _ledger, _parentLedger;
        private readonly StReader _reader;

        public delegate bool OnLedger (LedgerHeader header, AccountState state, TransactionTree txns);

        public HistoryLoader(StReader reader)
        {
            _reader = reader;
        }

        public virtual void Parse(OnLedger onLedger)
        {
            ParseIt(onLedger, true);
        }

        public virtual void ParseFast(OnLedger onLedger)
        {
            ParseIt(onLedger, false);
        }

        private void ParseIt(OnLedger onLedger, bool copyMap)
        {
            while (!_reader.End())
            {
                ParseOneLedger();
                var continue_ = onLedger(_ledger, 
                    copyMap ? _state.Copy() : _state, 
                    _transactions);
                if (!continue_)
                {
                    break;
                }
            }
        }

        public virtual void ParseOneLedger()
        {
            _parentLedger = _ledger;
            _ledger = LedgerHeader.FromReader(_reader);

            if (NextFrame() == FrameType.AccountStateTree)
            {
                ParseAccountState();
            }

            ParseTransactions();
            CheckTransactionHash();
            ParseAndApplyAccountStateDiff();
            CheckStateHash();
            CheckHashChain();
        }

        private FrameType NextFrame()
        {
            return (FrameType) _reader.Parser().Read(1)[0];
        }

        private void CheckHashChain()
        {
            if (_parentLedger != null)
            {
                AssertHashesEqual(_parentLedger.Hash(), _ledger.ParentHash);
            }
        }

        private void CheckStateHash()
        {
            AssertHashesEqual(_state.Hash(), _ledger.StateHash);

        }

        private void CheckTransactionHash()
        {
            AssertHashesEqual(_transactions.Hash(), _ledger.TransactionHash);
        }

        private static void AssertHashesEqual(Hash256 h1, Hash h2)
        {
            if (!h1.Equals(h2))
            {
                throw new AssertionError(h1 + " != " + h2);
            }
        }

        private void ParseTransactions()
        {
            _transactions = new TransactionTree();

            while (NextFrame() == FrameType.IndexedTransaction)
            {
                var tr = _reader.ReadTransactionResult(_ledger.LedgerIndex);
                _transactions.Add(tr);
                _reader.ReadOneInt();
                ParseAndApplyAccountStateDiff();
            }
        }

        private void ParseAccountState()
        {
            _state = new AccountState();

            while (NextFrame() == FrameType.IndexedLedgerEntry)
            {
                _state.Add(_reader.ReadLedgerEntry());
            }
        }

        private void ParseAndApplyAccountStateDiff()
        {
            uint modded = _reader.ReadUint32();
            for (var i = 0; i < modded; i++)
            {
                var b = _state.Update(_reader.ReadLedgerEntry());
                if (!b)
                {
                    throw new AssertionError();
                }
            }

            uint deleted = _reader.ReadUint32();
            for (var i = 0; i < deleted; i++)
            {
                var b = _state.RemoveLeaf(_reader.ReadHash256());
                if (!b)
                {
                    throw new AssertionError();
                }
            }

            uint added = _reader.ReadUint32();
            for (var i = 0; i < added; i++)
            {
                var b = _state.Add(_reader.ReadLedgerEntry());
                if (!b)
                {
                    throw new AssertionError();
                }
            }
        }

        public enum FrameType : byte
        {
            AccountStateTree = 0,
            AccountStateTreeEnd = 1,
            AccountStateDelta = 2,
            IndexedLedgerEntry = 3,
            IndexedTransaction = 4
        }
    }

    internal class AssertionError : Exception
    {
        public AssertionError()
        {
        }

        public AssertionError(string s) : base(s)
        {
        }
    }
}