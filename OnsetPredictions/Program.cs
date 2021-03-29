using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using CSCore;
using CSCore.SoundIn;

namespace OnsetPredictions
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            var criticalBands = BarkScale.BarkScale.CriticalBands();
            var criticalBandFrequencies = criticalBands.Select(x => (double)x.CenterFrequencyHz).ToList();

            // SETUP DSP
            //create a new soundIn instance
            var wasapiCapture = new WasapiCapture();
            wasapiCapture.Initialize();
            var onsetWriterBuilder = new DrumPredictor(wasapiCapture);
            var canContinue = true;
            var chosenDrums = new List<int>();

            do
            {
                // Make sure it's clear before we start
                onsetWriterBuilder.Reset();
                onsetWriterBuilder.Build(criticalBandFrequencies);
                // BEGIN CAPTURE
                wasapiCapture.Start();
                // Start detecting on each writer
                onsetWriterBuilder.DetectAndPredict();

                // WAIT FOR INPUT TO STOP
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

                Console.WriteLine("Continue? YES:1 NO:2");
                var chosen = ReadInteger(new[] {1, 2});

                canContinue = chosen switch
                {
                    1 => true,
                    2 => false,
                    _ => canContinue
                };
            } while (canContinue);

            wasapiCapture.Stop();
            wasapiCapture.Dispose();
        }

        private static List<IEnumerable<Tuple<double, float>>> ParseDictionaries(ConcurrentDictionary<double, ConcurrentDictionary<string, float>> dictionaries)
        {
            var rows = dictionaries.SelectMany(frequencyDict => frequencyDict.Value.Select(timeOnsetDict =>
                Tuple.Create(frequencyDict.Key, timeOnsetDict.Key, timeOnsetDict.Value))).ToList();

            var rowGroups = rows.GroupBy(r => r.Item2).OrderBy(dt => DateTime.Parse(dt.Key)).ToList();

            return rowGroups.Select(x => x.Select(y => Tuple.Create(y.Item1, y.Item3))).ToList();
        }

        /// <summary>
        /// Appends each member of the passed in collection of tuples to sb
        /// Ordered by frequency low -> high, write the raw data for that frequency
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="theDataWeWant"></param>
        /// <param name="chosenDrum"></param>
        /// <param name="criticalBandFrequencies"></param>
        private static void AppendParsedDictToSb(StringBuilder sb, List<IEnumerable<Tuple<double, float>>> theDataWeWant, int chosenDrum, IEnumerable<double> criticalBandFrequencies)
        {
            foreach (var frequenciesForMs in theDataWeWant)
            {
                var appendum = chosenDrum + "\t";
                var allZeroes = true;
                    
                foreach (var criticalBandFrequency in criticalBandFrequencies)
                {
                    var theFrequency = frequenciesForMs.FirstOrDefault(f => f.Item1 == criticalBandFrequency);

                    var valueToWrite = theFrequency == null ? 0 : theFrequency.Item2;

                    if (valueToWrite > 0) allZeroes = false;

                    appendum += valueToWrite + "\t";
                }

                if (!allZeroes)
                    sb.AppendLine(appendum);
            }
        }

        private static string FilePath(string fileName)
        {
            if (!fileName.Contains(".wav")) fileName += ".wav";
            
            return Path.Combine("C:\\source\\ADT\\sound", $"{fileName}");
        }

        private static int ReadInteger(params int[] validValues)
        {
            int value;

            do
            {
                value = ReadInteger();
                if (validValues == null || validValues.Any(x => x == value))
                    return value;
                Console.WriteLine("Invalid value");
            } while (true);
        }

        private static int ReadInteger()
        {
            int value;
            while (!Int32.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Invalid value");
            }
            return value;
        }

        /// <summary>
        /// Write the DrumTable and reads input
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private static int GetDrumTypeInput()
        {
            // Create a table with all of the options of DrumSoundType, and ask read integer for chosen DrumSoundType
            var table = SetupTable(new[] { "Drum", "Id" }, typeof(DrumSoundType));
            table.Options.EnableCount = false;
            table.Write();

            Console.WriteLine("> Select which drum you are going to play");

            var drumTypeInts = Extensions.EnumExtensions.GetEnumValues(typeof(DrumSoundType));
            // Pass in the drum type enum to ensure it's a valid choice
           return ReadInteger(drumTypeInts.ToArray());
        }

        /// <summary>
        /// Creates the table from given enum and columns
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private static ConsoleTable SetupTable(string[] columns, Type enumType)
        {
            var table = new ConsoleTable(columns);
            
            var drumTypeInts = Extensions.EnumExtensions.GetEnumValues(enumType);

            foreach (var drumTypeInt in drumTypeInts)
            {
                table.AddRow($"{(DrumSoundType) drumTypeInt}", drumTypeInt);
            }

            return table;
        }

        /// <summary>
        /// Appends the headers to sb and ends with a new line
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="criticalBandFrequencies"></param>
        private static void SetupStringBuilder(StringBuilder sb, IEnumerable<double> criticalBandFrequencies)
        {
            //sb.Append("#Type\t");

            //foreach (var criticalBandFrequency in criticalBandFrequencies)
            //{
            //    sb.Append(criticalBandFrequency + "\t");
            //}

            //sb.AppendLine();
        }
    }
}