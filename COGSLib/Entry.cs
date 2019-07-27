using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COGSLib
{
    public class Entry
    {
        public int EntryNo { get; }
        public DateTime EntryDateTime { get; }
        public EntryType EntryType { get; }
        public int Quantity { get; }
        public decimal GrossValue { get; }
        public decimal NetValue { get; }

        public decimal GrossUnitPrice => GrossValue / Quantity;

        public decimal NetUnitPrice => NetValue / Quantity;

        public Entry(int entryNo, DateTime entryDateTime, EntryType entryType, int quantity, decimal grossValue, decimal netValue)
        {
            EntryNo = entryNo;
            EntryDateTime = entryDateTime;
            EntryType = entryType;
            Quantity = quantity;
            GrossValue = grossValue;
            NetValue = netValue;
        }
    }

    public class ComputedEntry : Entry
    {
        public List<EntryRelationship> RelatedEntries { get; }
        public int QuantityConsumed { get; set; }
        public int RemainingQuantity => Quantity - QuantityConsumed;

        public ComputedEntry(Entry entry, List<EntryRelationship> relatedEntries)
            : this(entry.EntryNo, entry.EntryDateTime, entry.EntryType, entry.Quantity, entry.GrossValue, entry.NetValue, relatedEntries) { }
        public ComputedEntry(int entryNo, DateTime entryDateTime, EntryType entryType, int quantity, decimal grossValue, decimal netValue, List<EntryRelationship> relatedEntries)
            : base(entryNo, entryDateTime, entryType, quantity, grossValue, netValue)
        {
            RelatedEntries = relatedEntries;
        }
    }
}
