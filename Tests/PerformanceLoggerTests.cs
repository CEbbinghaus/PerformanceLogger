using System;
using Xunit;
using PerformanceLogger.Timer;
using System.IO;
using System.Text;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CanConstructTimer(){
            var timer = new PerfTimer();

            Assert.IsType<PerfTimer>(timer);
        }

        [Fact]
        public void CanRunBasicTiming()
        {
            StringWriter writer = new StringWriter();

            Console.SetOut(writer);

            var timer = new PerfTimer();

            timer.StartTimer("Test");
            timer.StopTimer();

            string output = writer.ToString();
            
            Assert.Contains("Test", output);
        }
    }
}
