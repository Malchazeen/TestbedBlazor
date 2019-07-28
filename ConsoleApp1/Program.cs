using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using COGSLib;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = @"21-11-2559 00:39:44	Buy	5บ.	20450	102250
23-11-2559 21:32:59	Buy	5บ.	20190	100950
30-11-2559 22:17:55	Buy	5บ.	19900	99500
11-04-2560 21:27:16	Sell	5บ.	20700	103500
29-06-2560 01:18:52	Buy	10บ.	20000	200000
03-07-2560 07:15:10	Buy	5บ.	19900	99500
29-08-2560 21:42:58	Sell	10บ.	20670	206700
16-11-2560 20:42:44	Buy	5บ.	19970	99850
21-11-2560 20:32:02	Buy	5บ.	19850	99250
05-12-2560 21:35:32	Buy	5บ.	19630	98150
15-01-2561 20:14:43	Sell	5บ.	20230	101150
31-01-2561 10:11:17	Buy	5บ.	19900	99500
13-08-2561 18:51:32	Buy	5บ.	18990	94950
18-06-2562 20:32:38	Sell	15บ.	19990	299850
21-06-2562 00:26:55	Sell	5บ.	20250	101250";

            var entries = GetEntries(text);

            string json = JsonConvert.SerializeObject(entries);

            File.WriteAllText("input.json", json);
        }

        static List<Entry> GetEntries(string text)
        {
            List<Entry> entries = new List<Entry>();
            using (var reader = new StringReader(text))
            {
                string line;
                int counter = 0;
                while ((line = reader.ReadLine()) != null)
                    entries.Add(GetEntry(line, ++counter));
            }
            return entries;
        }

        static System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("th-Th");
        static Entry GetEntry(string text, int entryNo)
        {
            string[] split = text.Split('\t');
            DateTime date = DateTime.ParseExact(split[0], "dd-MM-yyyy HH:mm:ss", culture);
            EntryType entryType = Enum.Parse<EntryType>(split[1]);
            int quantity = int.Parse(split[2].Substring(0, split[2].Length - 2));
            decimal value = decimal.Parse(split[4]);

            return new Entry(entryNo, date, entryType, quantity, value, value);
        }
    }
}
