using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using OnsetDataGeneration.BarkScale;

namespace OnsetDataGeneration
{
    public abstract class BaseDataGenerator
    {
        protected OnsetWriterBuilder OnsetWriterBuilder;
        private int ChosenDrum;
        protected List<CriticalBandModel> CriticalBands;
        protected List<double> CriticalBandFrequencies => CriticalBands.Select(x => (double)x.CenterFrequencyHz).ToList();

        private const string DataDirectory = "C:\\source\\ADT\\MLTraining\\data";

        protected BaseDataGenerator(int chosenDrum)
        {
            ChosenDrum = chosenDrum;
            CriticalBands = BarkScale.BarkScale.CriticalBands();
        }

        public abstract void Generate();

        public void StopAndSave()
        {
            // PARSE CAPTURED DATA FROM WRITERS INTO STRING BUILDER
            var dictionaries = new ConcurrentDictionary<double, ConcurrentDictionary<string, float>>();

            Parallel.ForEach(OnsetWriterBuilder.GetOnsetWriters(), (writer) =>
            {
                writer.Value.Detecting = false;
                dictionaries.AddOrUpdate(writer.Value.Frequency, writer.Value.ProcessedOnsetPeaks, (d, peaks) => peaks);
            });

            var sb = new StringBuilder();

            // Create rows from the dictionaries
            // Each index in the list contains a IEnumerable of tuples of <FREQUENCY, RAW FLOAT DATA>
            // Each index in the list is a 10ms window
            var dictionaryRows = ParseDictionaries(dictionaries);
            AppendParsedDictToSb(sb, dictionaryRows, ChosenDrum, CriticalBandFrequencies);

            
            var str = $"results {ChosenDrum}.txt";
            var path = Path.Combine(DataDirectory, str);

            File.WriteAllText(path, sb.ToString());

            var resultLines = File.ReadAllLines(path).ToList();
            resultLines.ForEach(l => Debug.WriteLine(l.Split("\t")));
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
    }
}
