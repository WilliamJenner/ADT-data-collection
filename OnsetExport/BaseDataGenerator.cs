using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using OnsetDetection;

namespace OnsetExport
{
    public abstract class BaseDataGenerator
    {
        private string ExportDirectory;
        private readonly ConcurrentBag<DrumDataStruct> ProcessedOnsets;
        private int ChosenDrum;

        private const string SEPARATOR = ",";
        private const string TYPE = "Type";
        private const string AVERAGE = "Avg";
        private const string L1NORM = "L1Norm";
        private const string MEAN = "Mean";

        protected BaseDataGenerator(int chosenDrum, string exportDirectory)
        {
            ChosenDrum = chosenDrum;
            ExportDirectory = exportDirectory;
            ProcessedOnsets = new ConcurrentBag<DrumDataStruct>();
        }

        public void OnPeakCalculated(OnsetPeakModel onsetPeakModel, double frequency)
        {
            try
            {
                var drumData = new DrumDataStruct(frequency, onsetPeakModel);
                // Adds the drum data to internal collection
                ProcessedOnsets.Add(drumData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public abstract void StartCollecting();

        /// <summary>
        /// Saves proccessed peaks to file
        /// </summary>
        /// <returns>Directory of the saved file</returns>
        public string StopAndSave()
        {
            // each row is a 10ms window
            // cols = frequencyValue, frequencyL1, frequencyMean repeated

            // we want highest peak value for each frequency for each 10ms window

            var groupedByTime = ProcessedOnsets
                .GroupBy(drumData => drumData.TimeOccured)
                .ToList();

            var validGroups = groupedByTime
                .Where(g => g.Any(drumData => drumData.OnsetPeakModel.PeakValue > 0.5));

            var sortedGroups = validGroups
                .Select(group => group
                    .OrderBy(x => x.Frequency)
                    .ToList())
                .ToList();

            var header = BuildCsvHeaderString();
            var body = BuildCsvBodyString(sortedGroups);

            var pathName = $"results {ChosenDrum}.csv";
            var path = Path.Combine(ExportDirectory, pathName);

            // Append the body if there are already results in the directory,
            // else write a new file with header
            if (File.Exists(path))
            {
                File.AppendAllText(path, body);
            }
            else
            {
                File.WriteAllText(path, header + body);
            }

            return path;
        }

        private string BuildCsvHeaderString()
        {
            var criticalBands = CriticalBandFactory.Get();
            var last = criticalBands.Last();
            var sb = new StringBuilder();
            // Order of each column
            // xValue, xAvg, xL1Norm, xMean, yValue, yAvg, yL1Norm, yMean ....

            sb.Append(TYPE);
            sb.Append(SEPARATOR);

            foreach (var criticalBand in criticalBands)
            {
                sb.Append(criticalBand);
                sb.Append(SEPARATOR);

                sb.Append($"{criticalBand}{AVERAGE}");
                sb.Append(SEPARATOR);

                sb.Append($"{criticalBand}{L1NORM}");
                sb.Append(SEPARATOR);

                sb.Append($"{criticalBand}{MEAN}");

                // If not last, add comma
                if (!criticalBand.Equals(last))
                {
                    sb.Append(SEPARATOR);
                }
            }

            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        /// <summary>
        /// Creates the body of the CSV file, not the headers for columns
        /// </summary>
        /// <param name="drumDataRows"></param>
        private string BuildCsvBodyString(List<List<DrumDataStruct>> drumDataRows)
        {
            var sb = new StringBuilder();

            foreach (var drumDataRow in drumDataRows)
            {
                sb.Append(ChosenDrum);
                sb.Append(SEPARATOR);

                var lastColumn = drumDataRow.Last();

                drumDataRow.ForEach(col =>
                {
                    var onsetPeakModel = col.OnsetPeakModel;
                    sb.Append(onsetPeakModel.PeakValue);
                    sb.Append(SEPARATOR);

                    sb.Append(onsetPeakModel.Average());
                    sb.Append(SEPARATOR);

                    sb.Append(onsetPeakModel.L1Norm());
                    sb.Append(SEPARATOR);

                    sb.Append(onsetPeakModel.Mean());
                    // DON'T append a separator here if it this is the last column in the row
                    if (col != lastColumn)
                    {
                        sb.Append(SEPARATOR);
                    }
                });

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}