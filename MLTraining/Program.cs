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

            //FloorLowFrequencyTrainer flft = new FloorLowFrequencyTrainer();
            //CeilingLowFrequencyTrainer clft = new CeilingLowFrequencyTrainer();

            //FloorMediumFrequencyTrainer fmft = new FloorMediumFrequencyTrainer();
            //CeilingMediumFrequencyTrainer cmft = new CeilingMediumFrequencyTrainer();

            //FloorHighFrequencyTrainer fhft = new FloorHighFrequencyTrainer();
            //CeilingHighFrequencyTrainer chft = new CeilingHighFrequencyTrainer();


            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}