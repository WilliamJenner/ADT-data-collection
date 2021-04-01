using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.Codecs;
using CSCore.SoundIn;

namespace OnsetDataGeneration
{
    class MicrophoneDataGenerator : BaseDataGenerator
    {
        public MicrophoneDataGenerator(int chosenDrum) : base(chosenDrum)
        {
        }

        public override void Generate()
        {
            // SETUP DSP
            //create a new soundIn instance
            var wasapiCapture = new WasapiCapture();
            wasapiCapture.Initialize();

            OnsetWriterBuilder = new OnsetWriterBuilder(wasapiCapture);
            OnsetWriterBuilder.Detect(CriticalBandFrequencies);
            wasapiCapture.Start();

            // WAIT FOR INPUT TO STOP
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            StopAndSave();
        }
    }
}

