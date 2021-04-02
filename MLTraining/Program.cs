using System;

namespace MLTraining
{
    internal class Program
    {
        
        private static void Main(string[] args)
        {
            LowFrequencyTrainer lft = new LowFrequencyTrainer();
            MediumFrequencyTrainer mft = new MediumFrequencyTrainer();
            HighFrequencyTrainer hft = new HighFrequencyTrainer();

            AllBandTrainer abt = new AllBandTrainer();

            Console.WriteLine("===================================================================");
            Console.WriteLine("===================================================================");
            Console.WriteLine("===================================================================");


            //LowFrequencyFloorTrainer flft = new LowFrequencyFloorTrainer();
            //LowFrequencyCeilingTrainer clft = new LowFrequencyCeilingTrainer();

            //MediumFrequencyFloorTrainer fmft = new MediumFrequencyFloorTrainer();
            //MediumFrequencyCeilingTrainer cmft = new MediumFrequencyCeilingTrainer();

            //HighFrequencyFloorTrainer fhft = new HighFrequencyFloorTrainer();
            //HighFrequencyCeilingTrainer chft = new HighFrequencyCeilingTrainer();


            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}