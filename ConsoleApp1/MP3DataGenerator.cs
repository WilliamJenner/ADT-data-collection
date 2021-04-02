using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CSCore.Codecs;
using CSCore.SoundIn;
using Extensions;

namespace OnsetDataGeneration
{
    class Mp3DataGenerator : BaseDataGenerator
    {
        private readonly int _chosenDrum;

        public Mp3DataGenerator(int chosenDrum) : base(chosenDrum)
        {
            _chosenDrum = chosenDrum;
        }

        public override void Generate()
        {
            // Get all mp3 files from the dir
            string[] directories = new[]
            {
                "D:\\Google Drive\\Final Year\\Project\\kick samples",
                "D:\\Google Drive\\Final Year\\Project\\snare samples",
                "D:\\Google Drive\\Final Year\\Project\\cymbal samples"
            };


            for (var index = 0; index < directories.Length; index++)
            {
                var directory = directories[index];
                var files =
                    Directory.GetFiles(directory).Where(f => f.Contains(".mp3") || f.Contains(".wav")).ToList();

                var chunkedFiles = files.SplitList(nSize: 15);

                foreach (var chunk in chunkedFiles.Select((value, i) => (value, i)))
                {
                    foreach (var file in chunk.value)
                    {
                        var wavInput = CodecFactory.Instance.GetCodec(Path.Combine(directory, file));
                        OnsetWriterBuilder = new OnsetWriterBuilder(wavInput);
                        OnsetWriterBuilder.Reset();
                        OnsetWriterBuilder.Detect(CriticalBandFrequencies);
                        ChosenDrum = index; // order of dirs is equal to types of drums
                        StopAndSave();
                        // Add a little sleep so all the onset writers and threads can dispose
                    }

                    Console.WriteLine($"Completed chunk {chunk.i}/{chunkedFiles.Count()}");
                    Thread.Sleep(1000);
                }
            }

            // below is for reading via subdirs
            //var subDirs = Directory.GetDirectories(directory);

            //foreach (var subDirectory in subDirs)
            //{
            //    var lowerSubDirectories = Directory.GetDirectories(subDirectory);
            //    var files = lowerSubDirectories.SelectMany(lowerSubDirectory =>
            //        Directory.GetFiles(lowerSubDirectory).Where(f => f.Contains(".mp3"))).ToList();

            //    var chunkedFiles = files.SplitList(nSize: 15);

            //    foreach (var chunk in chunkedFiles.Select((value, i) => (value, i)))
            //    {
            //        foreach (var file in chunk.value)
            //        {
            //            var wavInput = CodecFactory.Instance.GetCodec(Path.Combine(directory, file));
            //            OnsetWriterBuilder = new OnsetWriterBuilder(wavInput);
            //            OnsetWriterBuilder.Reset();
            //            OnsetWriterBuilder.Detect(CriticalBandFrequencies);
            //            StopAndSave();
            //            // Add a little sleep so all the onset writers and threads can dispose
            //        }

            //        Console.WriteLine($"Completed chunk {chunk.i}/{chunkedFiles.Count()}");
            //        Thread.Sleep(100);
            //    }
            //    Console.WriteLine("-------------------------------");
            //    Console.WriteLine($"COMPLETED {subDirectory}");
            //    Console.WriteLine("-------------------------------");
            //}

            // Generate the data for each file
            //foreach (var file in enumerable.Select((value, i) => (value, i)))
            //{
            //    var wavInput = CodecFactory.Instance.GetCodec(Path.Combine(directory, file.value));
            //    OnsetWriterBuilder = new OnsetWriterBuilder(wavInput);
            //    OnsetWriterBuilder.Reset();
            //    OnsetWriterBuilder.Detect(CriticalBandFrequencies);
            //    StopAndSave();

            //    Console.WriteLine($"File {file.i}/{enumerable.Count()} complete");

            //    // Add a little sleep so all the onset writers and threads can dispose
            //    Thread.Sleep(500);
            //}
        }
    }
}
