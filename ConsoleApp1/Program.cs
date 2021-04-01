using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using CSCore.Codecs;
using CSCore.SoundIn;

namespace OnsetDataGeneration
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputType = GetInputType();
            var chosenDrum = GetDrumTypeInput();

            var generator = (DSPInputType) inputType == DSPInputType.Microphone
                ? new MicrophoneDataGenerator(chosenDrum) as BaseDataGenerator
                : new Mp3DataGenerator(chosenDrum) as BaseDataGenerator;

            generator.Generate();
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
            var table = SetupTable<DrumSoundType>(new[] { "Drum", "Id" });
            table.Options.EnableCount = false;
            table.Write();

            Console.WriteLine("> Select which drum you are going to play");

            var drumTypeInts = Extensions.EnumExtensions.GetEnumValues(typeof(DrumSoundType));
            // Pass in the drum type enum to ensure it's a valid choice
           return ReadInteger(drumTypeInts.ToArray());
        }

        private static int GetInputType()
        {
            // Create a table with all of the options of DrumSoundType, and ask read integer for chosen DrumSoundType
            var table = SetupTable<DSPInputType>(new[] { "Input", "Id" });
            table.Options.EnableCount = false;
            table.Write();

            Console.WriteLine("> Select which input you are using");

            var inputTypes = Extensions.EnumExtensions.GetEnumValues(typeof(DSPInputType));
            // Pass in the drum type enum to ensure it's a valid choice
            return ReadInteger(inputTypes.ToArray());
        }

        /// <summary>
        /// Creates the table from given enum and columns
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private static ConsoleTable SetupTable<T>(string[] columns) where T : Enum
        {
            var table = new ConsoleTable(columns);
           
            foreach (var enumValue in Enum.GetValues(typeof(T)))
            {
                table.AddRow($"{enumValue}", (int)enumValue);
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