using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COGSLib
{
    public class Performance
    {
        public int OutstandingQuantity { get; }
        public decimal InputValue { get; }
        public decimal UnallocatedValue { get; }
        public decimal OutstandingValue { get; }

        public decimal AllocatedValue => InputValue - UnallocatedValue;

        public decimal AllocatedUnitPrice => OutstandingQuantity > 0 ? AllocatedValue / OutstandingQuantity : 0;

        public decimal OutstandingUnitPrice => OutstandingQuantity > 0 ? OutstandingValue / OutstandingQuantity : 0;

        public decimal RealizedProfit => UnallocatedValue + OutstandingValue - InputValue;

        public Performance(int outstandingQuantity
            , decimal inputValue
            , decimal unallocatedValue
            , decimal outstandingValue)
        {
            OutstandingQuantity = outstandingQuantity;
            InputValue = inputValue;
            UnallocatedValue = unallocatedValue;
            OutstandingValue = outstandingValue;
        }
    }
}
