using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COGSLib
{
    public class EntryRelationship
    {
        public int EntryNo { get; }
        public int DetailNo { get; }
        public int RelatedEntryNo { get; }
        public int Quantity { get; }

        public EntryRelationship(int entryNo, int detailNo, int relatedEntryNo, int quantity)
        {
            EntryNo = entryNo;
            DetailNo = detailNo;
            RelatedEntryNo = relatedEntryNo;
            Quantity = quantity;
        }
    }
}
