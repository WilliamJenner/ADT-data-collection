using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CSCore.Codecs;
using CSCore.SoundIn;

namespace OnsetDataGeneration
{
    class Mp3DataGenerator : BaseDataGenerator
    {
        public Mp3DataGenerator(int chosenDrum) : base(chosenDrum)
        {
        }

        public override void Generate()
        {
            var directory = "C:\\source\\ADT\\MLTraining\\data\\cymbal-test";
            var fileName = "signature_traditionals_22_medium_light_swish_swell.mp3";

            var wavInput = CodecFactory.Instance.GetCodec(Path.Combine(directory, fileName));
            OnsetWriterBuilder = new OnsetWriterBuilder(wavInput);

            OnsetWriterBuilder.Detect(CriticalBandFrequencies);


            // WAIT FOR INPUT TO STOP
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            StopAndSave();
        }
    }
}
