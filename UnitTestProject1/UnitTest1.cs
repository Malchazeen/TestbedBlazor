using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using COGSLib;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        readonly WeightedAverage weightedAverage;
        readonly FIFO fifo;
        readonly LIFO lifo;
        public UnitTest1()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<WeightedAverage>();
            services.AddSingleton<FIFO>();
            services.AddSingleton<LIFO>();
            var serviceProvider = services.BuildServiceProvider();
            weightedAverage = serviceProvider.GetRequiredService<WeightedAverage>();
            fifo = serviceProvider.GetRequiredService<FIFO>();
            lifo = serviceProvider.GetRequiredService<LIFO>();
        }

        List<Entry> GetEntries(int num)
        {
            string basePath = @"..\..\..\json\";
            string fileName = $"entries{num:D2}.json";
            return JsonConvert.DeserializeObject<List<Entry>>(File.ReadAllText(Path.Combine(basePath, fileName)));
        }

        [TestMethod]
        public void TestMethod1()
        {
            var entries = GetEntries(1);
            var output = weightedAverage.ComputeCOGS(entries);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var entries = GetEntries(1);
            var output = fifo.ComputeCOGS(entries);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var entries = GetEntries(1);
            var output = lifo.ComputeCOGS(entries);
        }
    }
}
