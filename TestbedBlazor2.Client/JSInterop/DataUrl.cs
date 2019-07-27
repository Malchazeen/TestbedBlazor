using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestbedBlazor2.Client.JSInterop
{
    public class DataUrl
    {
        public string MimeType { get; }
        public bool IsBase64 { get; }
        public string Data { get; }

        public DataUrl(string dataUrl)
        {
            if (string.IsNullOrEmpty(dataUrl))
                throw new ArgumentNullException(nameof(dataUrl));

            const string magic = "data:";
            using (var mainReader = new StringReader(dataUrl))
            {
                char[] temp = new char[magic.Length];
                mainReader.ReadBlock(temp, 0, temp.Length);
                if (magic != new string(temp))
                    throw new FormatException(nameof(dataUrl));

                string midPart;
                if (!mainReader.ReadTo(',', out midPart))
                    throw new FormatException(nameof(dataUrl));
                
                using (var subReader = new StringReader(midPart))
                {
                    string read;
                    IsBase64 = subReader.ReadTo(';', out read) && subReader.ReadToEnd() == "base64";
                    MimeType = string.IsNullOrEmpty(read) ? "text/plain;charset=US-ASCII" : read;
                }

                Data = mainReader.ReadToEnd();
            }
        }
    }
}
