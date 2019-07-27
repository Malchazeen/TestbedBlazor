using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COGSLib
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntryType : byte
    {
        Buy = 1,
        Sell = 2,
    }
}
