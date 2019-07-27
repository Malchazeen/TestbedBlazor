using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestbedBlazor2
{
    public static class Extension
    {
        public static bool Read(this StringReader value, out char c)
        {
            c = default;

            int read = value.Read();
            if (read == -1)
                return false;

            c = (char)read;
            return true;
        }
        public static bool ReadTo(this StringReader value, char c, out string read)
        {
            StringBuilder sb = new StringBuilder();
            bool found = false;
            while (value.Read(out char r))
            {
                if (r == c)
                {
                    found = true;
                    break;
                }
                sb.Append(r);
            }
            read = sb.ToString();
            return found;
        }
    }
}
