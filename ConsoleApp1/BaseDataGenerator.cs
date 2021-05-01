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
        protected int ChosenDrum;
        protected List<CriticalBandModel> CriticalBands;
        //protected List<double> CriticalBandFrequencies => CriticalBands.Select(x => (double)x.CenterFrequencyHz).ToList();

        // Create a list of every 1000 between 1 and 20,001 - not sure if inclusive or exclusive so
        protected List<double> CriticalBandFrequencies =>
            CriticalBands.Select(x => (double)x.CenterFrequencyHz).Union(Enumerable.Range(1, 20001)
                    .Where(integer => integer % 1000 == 0)
                    .Select(Convert.ToDouble))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

        private const string DataDirectory = "C:\\source\\ADT\\MLTraining\\data";

        protected BaseDataGenerator(int chosenDrum)
        {
            ChosenDrum = chosenDrum;
            CriticalBands = BarkScale.BarkScale.CriticalBands();

            CriticalBandFrequencies.ForEach(cbf => Debug.Write(string.Join(",", new string[] { cbf.ToString(), $"{cbf}Mean", $"{cbf}Avg", $"{cbf}L1," })));
        }

        public abstract void Generate();

        public void StopAndSave()
        {
            // PARSE CAPTURED DATA FROM WRITERS INTO STRING BUILDER
            var dictionaries = new ConcurrentDictionary<double, ConcurrentDictionary<string, OnsetPeakModel>>();

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

            
            var str = $"results {ChosenDrum}.csv";
            var path = Path.Combine(DataDirectory, str);

            File.AppendAllText(path, sb.ToString());

            var resultLines = File.ReadAllLines(path).ToList();
        }

        private static List<IEnumerable<Tuple<double, OnsetPeakModel>>> ParseDictionaries(ConcurrentDictionary<double, ConcurrentDictionary<string, OnsetPeakModel>> dictionaries)
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
        private static void AppendParsedDictToSb(StringBuilder sb, List<IEnumerable<Tuple<double, OnsetPeakModel>>> theDataWeWant, int chosenDrum, IEnumerable<double> criticalBandFrequencies)
        {
            foreach (var frequenciesForMs in theDataWeWant)
            {
                var appendum = chosenDrum + ",";
                var allZeroes = true;
                var lastItem = criticalBandFrequencies.Last();

                foreach (var criticalBandFrequency in criticalBandFrequencies)
                {
                    var theFrequency = frequenciesForMs.FirstOrDefault(f => f.Item1 == criticalBandFrequency);

                    var valueToWrite = theFrequency == null ? new OnsetPeakModel(0, new float[0]) : theFrequency.Item2;

                    if (valueToWrite.PeakValue > 0) allZeroes = false;

                    var isLastItem = lastItem == criticalBandFrequency;
                    appendum += valueToWrite.ToString(leadingComma: !isLastItem);
                }

                if (!allZeroes)
                    sb.AppendLine(appendum);
            }
        }
    }
}
