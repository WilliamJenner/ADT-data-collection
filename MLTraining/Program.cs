using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.ML;
using Microsoft.ML.Data;
using MLTraining.DataStructures;

namespace MLTraining
{
    internal class Program
    {
        
        private static void Main(string[] args)
        {
            LowFrequencyTrainer lft = new LowFrequencyTrainer();
            MediumFrequencyTrainer mft = new MediumFrequencyTrainer();
            HighFrequencyTrainer hft = new HighFrequencyTrainer();

            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}