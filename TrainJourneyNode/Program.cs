using System;
using System.Diagnostics;
using System.Threading;

namespace TrainJourneyNode
{
    public class JourneyStatus
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public int SequencePosition { get; set; }
    }
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
