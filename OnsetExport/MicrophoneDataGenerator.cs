using System;
using OnsetDetection;

namespace OnsetExport
{
    public class MicrophoneDataGenerator : BaseDataGenerator 
    {
        public MicrophoneDataGenerator(int chosenDrum, string exportDirectory) : base(chosenDrum, exportDirectory)
        {
        }

        public override void StartCollecting()
        {
            // SETUP DSP

            var onsetWriter = new OnsetWriter(OnPeakCalculated);
            onsetWriter.DetectAndBroadcast();

            // WAIT FOR INPUT TO STOP
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            onsetWriter.Reset();
            var filePath = StopAndSave();
            Console.WriteLine($"Data saved to {filePath}");
        }
    }
}

