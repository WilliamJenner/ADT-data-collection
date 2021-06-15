using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public static class TrainingDataFactory
    {
        private const string All = "all.csv";


        /// <summary>
        /// Reads the directory and attempts to get an all.csv file, or combines any file with the format "results {0}.csv" into an all
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="directory">The directory the CSV files are contained in</param>
        /// <returns>A Train Test data split of testFraction 0.2</returns>
        public static DataOperationsCatalog.TrainTestData GetSplitTrainingData(MLContext mlContext, string directory)
        {
            try
            {
                var allPath = CombineResultsData(directory);

                var dataView =
                    mlContext.Data.LoadFromTextFile<DrumTypeData>(allPath, hasHeader: true, separatorChar: ',');
                return mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            }
            catch (Exception ex)
            {
                var shell = new Exception("Error trying to GetSplitTrainingData", ex);
                throw shell;
            }
        }

        private static string CombineResultsData(string directory )
        {
            try
            {
                var allPath = Path.Combine(directory, All);

               if (File.Exists(allPath)){File.Delete(allPath);}

                DirectoryInfo csvDirectory = new DirectoryInfo(directory);
                
                FileInfo[] csvFiles = csvDirectory.GetFiles("*.csv");

                if (csvFiles.Length < 2)
                {
                    throw new ArgumentOutOfRangeException(directory, "Less than 2 sets of CSV result files. Training needs at least 2 to differentiate between them.");
                }

                StringBuilder sb = new StringBuilder();
                for (var index = 0; index < csvFiles.Length; index++)
                {
                    FileInfo csvFile = csvFiles[index];
                    using (StreamReader sr = new StreamReader(csvFile.OpenRead()))
                    {
                        if (index != 0)
                            sr.ReadLine(); // Discard header line unless this is the first file, we want at least one header
                        while (!sr.EndOfStream)
                            sb.AppendLine(sr.ReadLine());
                    }
                }

                File.AppendAllText(allPath, sb.ToString());
                return allPath;
            }
            catch (Exception ex)
            {
                var shell = new Exception($"Error attempting to create CSV files for path {directory}", ex);
                throw shell;
            }
        }
    }
}
