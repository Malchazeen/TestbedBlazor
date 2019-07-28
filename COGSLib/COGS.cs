using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COGSLib
{
    public interface ICOGS
    {
        COGSResult ComputeCOGS(List<Entry> entries);
    }

    public class COGSResult
    {
        public Performance Performance { get; }
        public List<ComputedEntry> Entries { get; }

        public COGSResult(Performance performance, List<ComputedEntry> computedEntries)
        {
            Performance = performance;
            Entries = computedEntries;
        }
    }

    public class WeightedAverage : ICOGS
    {
        public COGSResult ComputeCOGS(List<Entry> entries)
        {
            int buyQuantity = 0;
            decimal buyGrossValue = 0;
            decimal buyNetValue = 0;
            DateTime buyDateTime = default;
            int sellQuantity = 0;
            decimal sellGrossValue = 0;
            decimal sellNetValue = 0;
            DateTime sellDateTime = default;

            foreach (var entry in entries)
            {
                if (entry.EntryType == EntryType.Buy)
                {
                    buyQuantity += entry.Quantity;
                    buyGrossValue += entry.GrossValue;
                    buyNetValue += entry.NetValue;
                    buyDateTime = entry.EntryDateTime;
                }
                else
                {
                    sellQuantity += entry.Quantity;
                    sellGrossValue += entry.GrossValue;
                    sellNetValue += entry.NetValue;
                    sellDateTime = entry.EntryDateTime;
                }
            }

            int outstandingQuantity = buyQuantity - sellQuantity;
            decimal inputValue = buyNetValue;
            decimal unallocatedValue = sellNetValue;
            decimal outstandingValue = buyNetValue * (outstandingQuantity / (decimal)buyQuantity);

            List<ComputedEntry> computedEntries = new List<ComputedEntry>
            {
                new ComputedEntry(1, buyDateTime, EntryType.Buy, buyQuantity, buyGrossValue, buyNetValue, new List<EntryRelationship>()) { QuantityConsumed = sellQuantity, },
                new ComputedEntry(2, sellDateTime, EntryType.Sell, sellQuantity, sellGrossValue, sellNetValue, new List<EntryRelationship> {
                    new EntryRelationship(2, 1, 1, sellQuantity),
                }),
            };
            Performance performance = new Performance(outstandingQuantity, inputValue, unallocatedValue, outstandingValue);
            return new COGSResult(performance, computedEntries);
        }
    }

    public class FIFO : ICOGS
    {
        public COGSResult ComputeCOGS(List<Entry> entries)
        {
            List<ComputedEntry> computedEntries = new List<ComputedEntry>();
            Queue<ComputedEntry> inventoryQueue = new Queue<ComputedEntry>();

            int outstandingQuantity = 0;
            decimal inputValue = 0;
            decimal unallocatedValue = 0;
            decimal outstandingValue = 0;
            foreach (var entry in entries)
            {
                List<EntryRelationship> relatedEntries = new List<EntryRelationship>();
                var computedEntry = new ComputedEntry(entry, relatedEntries);
                int detailNoCounter = 0;
                if (entry.EntryType == EntryType.Buy)
                {
                    if (entry.NetValue > unallocatedValue)
                    {
                        inputValue += entry.NetValue - unallocatedValue;
                        unallocatedValue = 0;
                    }
                    else
                    {
                        unallocatedValue -= entry.NetValue;
                    }
                    outstandingQuantity += entry.Quantity;
                    outstandingValue += entry.NetValue;
                    inventoryQueue.Enqueue(computedEntry);
                }
                else
                {
                    int quantity = entry.Quantity;
                    decimal valueConsumed = 0;
                    while(quantity > 0)
                    {
                        var currentInventoryEntry = inventoryQueue.Peek();

                        int matchableQuantity = Math.Min(quantity, currentInventoryEntry.RemainingQuantity);
                        currentInventoryEntry.QuantityConsumed += matchableQuantity;
                        quantity -= matchableQuantity;
                        valueConsumed += currentInventoryEntry.NetValue * (matchableQuantity / (decimal)currentInventoryEntry.Quantity);
                        if (currentInventoryEntry.RemainingQuantity == 0)
                            inventoryQueue.Dequeue();

                        relatedEntries.Add(new EntryRelationship(entry.EntryNo, ++detailNoCounter, currentInventoryEntry.EntryNo, matchableQuantity));
                    }

                    outstandingValue -= valueConsumed;
                    outstandingQuantity -= entry.Quantity;
                    unallocatedValue += entry.NetValue;
                }
                computedEntries.Add(computedEntry);
            }

            Performance performance = new Performance(outstandingQuantity, inputValue, unallocatedValue, outstandingValue);
            return new COGSResult(performance, computedEntries);
        }
    }

    public class LIFO : ICOGS
    {
        public COGSResult ComputeCOGS(List<Entry> entries)
        {
            List<ComputedEntry> computedEntries = new List<ComputedEntry>();
            Stack<ComputedEntry> inventoryQueue = new Stack<ComputedEntry>();

            int outstandingQuantity = 0;
            decimal inputValue = 0;
            decimal unallocatedValue = 0;
            decimal outstandingValue = 0;
            foreach (var entry in entries)
            {
                List<EntryRelationship> relatedEntries = new List<EntryRelationship>();
                var computedEntry = new ComputedEntry(entry, relatedEntries);
                int detailNoCounter = 0;
                if (entry.EntryType == EntryType.Buy)
                {
                    if (entry.NetValue > unallocatedValue)
                    {
                        inputValue += entry.NetValue - unallocatedValue;
                        unallocatedValue = 0;
                    }
                    else
                    {
                        unallocatedValue -= entry.NetValue;
                    }
                    outstandingQuantity += entry.Quantity;
                    outstandingValue += entry.NetValue;
                    inventoryQueue.Push(computedEntry);
                }
                else
                {
                    int quantity = entry.Quantity;
                    decimal valueConsumed = 0;
                    while (quantity > 0)
                    {
                        var currentInventoryEntry = inventoryQueue.Peek();

                        int matchableQuantity = Math.Min(quantity, currentInventoryEntry.RemainingQuantity);
                        currentInventoryEntry.QuantityConsumed += matchableQuantity;
                        quantity -= matchableQuantity;
                        valueConsumed += currentInventoryEntry.NetValue * (matchableQuantity / (decimal)currentInventoryEntry.Quantity);
                        if (currentInventoryEntry.RemainingQuantity == 0)
                            inventoryQueue.Pop();

                        relatedEntries.Add(new EntryRelationship(entry.EntryNo, ++detailNoCounter, currentInventoryEntry.EntryNo, matchableQuantity));
                    }

                    outstandingValue -= valueConsumed;
                    outstandingQuantity -= entry.Quantity;
                    unallocatedValue += entry.NetValue;
                }
                computedEntries.Add(computedEntry);
            }

            Performance performance = new Performance(outstandingQuantity, inputValue, unallocatedValue, outstandingValue);
            return new COGSResult(performance, computedEntries);
        }
    }
}
