using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ConsoleTables;
using Extensions;
using M;
using Microsoft.ML.Data;
using MLTraining;
using MLTraining.DataStructures;
using OnsetExport;
using OnsetPredictions;

namespace ADT
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // No arguments, continue with default behaviour
                if (args.Length == 0)
                {
                    Transcribe(MidiDevice.Outputs.First(), "C:\\source\\ADT\\ExportedData");
                }
                else
                {
                    ProcessArgs(args);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oops! An exception occurred.");
                PrettyPrintExceptionRecursive(ex);
                Console.WriteLine("Try --help");
            }
        }

        static void PrettyPrintExceptionRecursive(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            var innerExceptions = GetInnerExceptions(ex);
            var indent = "\t";

            foreach (var innerException in innerExceptions)
            {
                Console.WriteLine($"{indent} An exception of type {innerException.GetType()} occured");
                Console.WriteLine($"{indent} {innerException.Message}");
                Console.WriteLine($"{indent}");
                indent += "\t"; // make the indent larger
            }
        }

        static void PrettyPrintTrainingMetrics(IEnumerable<ModelMetrics> metrics)
        {
            var columns = new string []{"Trainer", "MacroAccuracy", "MicroAccuracy", "LogLoss", "LogLossReduction"};

            var table = new ConsoleTable(columns);
            table.AddRow(string.Empty, "Closer to 1, the better", "Closer to 1, the better", "Closer to 0, the better", "Closer to 0, the better");


            var perClassAccuracies = new List<string>();

            foreach (var metric in metrics)
            {
                table.AddRow(metric.Name.Replace("ClassificationModel", string.Empty), metric.Metrics.MacroAccuracy, metric.Metrics.MicroAccuracy, metric.Metrics.LogLoss, metric.Metrics.LogLossReduction);

                var perClassLogLoss = metric.Metrics.PerClassLogLoss;

                var sb = new StringBuilder();

                sb.AppendLine($"Per class log loss for {metric.Name}");

                for (var index = 0; index < perClassLogLoss.Count; index++)
                {
                    var logLoss = perClassLogLoss[index];
                    sb.AppendLine($"LogLoss for class {index} = {logLoss:0.####}");
                }

                sb.AppendLine();
                perClassAccuracies.Add(sb.ToString());
            }

            table.Options.EnableCount = false;
            Console.WriteLine("All values for accuracy and log loss are in a normalized range between [0, ~1]");
            table.Write();

            perClassAccuracies.ForEach(Console.Write);
        }

        static IEnumerable<Exception> GetInnerExceptions(Exception ex)
        {
            var innerException = ex;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            }
            while (innerException != null);
        }

        static void ProcessArgs(string[] args)
        {
            if (args.Length == 0) throw new ArgumentException("Args has a length of 0", nameof(args));

            // Assuming the first arg will always be the command argument

            switch (args[0])
            {
                case Arguments.Help:
                    PrintHelp();
                    break;
                case Arguments.Export:
                    BeginDataExport(args);
                    break;
                case Arguments.Train:
                    TrainModel(args);
                    break;
                case Arguments.ADT:
                    ProcessADTArgs(args);
                    break;
                default:
                    throw new ArgumentException("Argument does not match expected", nameof(args));
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("help");
        }
        
        static void BeginDataExport(string [] args)
        {
            var directory = args.IndexExists(1) ? "C:\\source\\ADT\\ExportedData" : args[1];

            var chosenDrum = GetDrumTypeInput();
            var generator = new MicrophoneDataGenerator(chosenDrum, directory);
            generator.StartCollecting();
        }

        static void TrainModel(string [] args)
        {
            var directory = args.IndexExists(1) ? "C:\\source\\ADT\\ExportedData" : args[1];
            var modelPath = Path.Combine(directory, "Models");
            var metrics = FrequencyMultiClassificationModelFactory.Train(directory, modelPath);
            PrettyPrintTrainingMetrics(metrics);
        }

        static void ProcessADTArgs(string[] args)
        {
           

            if (args.IndexExists(1))
            {
                // check if the arg is show
                if (args[1] == "show")
                {
                    // Using a for loop we want the index
                    for (var index = 0; index < MidiDevice.Outputs.Count; index++)
                    {
                        Console.WriteLine($"Port {index} | {MidiDevice.Outputs[index].Name}");
                    }
                }
                // assume it is the directory arg
                else
                {
                    int number;
                    bool success = Int32.TryParse(args[2], out number);

                    if (success)
                    {
                        // Minus 1 from the count as number is zero indexed
                        if (number > (MidiDevice.Outputs.Count - 1))
                        {
                            throw new ArgumentException("Chosen midi port does not exist");
                        }

                        Transcribe(MidiDevice.Outputs[number], args[1]);
                    }
                }
                
                
            }

            // If success, then transcribe to a specific port
            // Else, try second arg for "show", else, throw
            
            else
            {
                
            }
        }

        static void Transcribe(MidiOutputDevice midiOutputDevice, string modelDirectory)
        {
            Console.WriteLine("Beginning predictions!");
            Console.WriteLine("Press any key to stop...");

            DrumPredictor.Predict(midiOutputDevice, modelDirectory);

            Console.ReadKey();
            
            DrumPredictor.Stop();
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
    }
}
