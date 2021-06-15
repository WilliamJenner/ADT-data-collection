using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleTables;
using Extensions;
using M;
using MLTraining;
using MLTraining.DataStructures;
using OnsetExport;
using OnsetPredictions;

namespace ADT
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ProcessArgs(args);
        }

        private static void ProcessArgs(string[] args)
        {
            try
            {
                // No arguments, continue with default behaviour
                if (args.Length == 0)
                    Transcribe(MidiDevice.Outputs.First(), "C:\\source\\ADT\\ExportedData");
                else
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
                        case Arguments.Stop:
                            Environment.Exit(0);
                            break;
                        default:
                            throw new ArgumentException("Argument does not match expected", nameof(args));
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oops! An exception occurred.");
                PrettyPrintExceptionRecursive(ex);
                Console.WriteLine("Try --help");
                AcceptNextCommand();
            }
        }

        private static void PrettyPrintExceptionRecursive(Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

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

        private static void PrettyPrintTrainingMetrics(IEnumerable<ModelMetrics> metrics)
        {
            var columns = new[] {"Trainer", "MacroAccuracy", "MicroAccuracy", "LogLoss", "LogLossReduction"};

            var table = new ConsoleTable(columns);
            table.AddRow(string.Empty, "Closer to 1, the better", "Closer to 1, the better", "Closer to 0, the better",
                "Closer to 0, the better");


            var perClassAccuracies = new List<string>();

            foreach (var metric in metrics)
            {
                table.AddRow(metric.Name.Replace("ClassificationModel", string.Empty), metric.Metrics.MacroAccuracy,
                    metric.Metrics.MicroAccuracy, metric.Metrics.LogLoss, metric.Metrics.LogLossReduction);

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

        private static IEnumerable<Exception> GetInnerExceptions(Exception ex)
        {
            var innerException = ex;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            } while (innerException != null);
        }

        private static void PrintHelp()
        {
            var columns = new[] {"Command", "Argument 1", "Argument 2", "Action"};

            var table = new ConsoleTable(columns);
            table.AddRow("No arguments", string.Empty, string.Empty, "No command will start the ADT module for models with default path, and MIDI port 0");
            table.AddRow(Arguments.Help, string.Empty, string.Empty, "Displays the help table");
            table.AddRow(Arguments.Stop, string.Empty, string.Empty, "Terminates the program");
            table.AddRow(Arguments.ADT, "show", string.Empty, "Shows all available MIDI ports in the system");
            table.AddRow(Arguments.ADT, "C:\\directory\\subdir", "1", "Starts the ADT module for models at arg 1, and MIDI port arg 2");
            table.AddRow(Arguments.ADT, "2", string.Empty, "Starts the ADT module at default path, and MIDI port arg 1");
            table.AddRow(Arguments.Export, "C:\\directory\\subdir", string.Empty, "Starts the export module and saves data to arg 1");
            table.AddRow(Arguments.Train, "C:\\directory\\subdir", string.Empty, "Starts the machine learning module and saves models to arg 1");

            table.Options.EnableCount = false;
            table.Write();

            AcceptNextCommand();
        }

        private static void AcceptNextCommand()
        {
            Console.WriteLine();
            Console.WriteLine("> Type any command...");
            Console.WriteLine();

            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                ProcessArgs(new string[0]);
            }
            else
            {
                var arguments = line.Split(" ");
                ProcessArgs(arguments);
            }
        }

        private static void BeginDataExport(string[] args)
        {
            var directory = args.IndexExists(1) ? "C:\\source\\ADT\\ExportedData" : args[1];

            var chosenDrum = GetDrumTypeInput();
            var generator = new MicrophoneDataGenerator(chosenDrum, directory);
            generator.StartCollecting();
            AcceptNextCommand();
        }

        private static void TrainModel(string[] args)
        {
            var directory = args.IndexExists(1) ? args[1] : "C:\\source\\ADT\\ExportedData";
            var metrics = FrequencyMultiClassificationModelFactory.Train(directory, directory);
            PrettyPrintTrainingMetrics(metrics);
            AcceptNextCommand();
        }

        private static void ProcessADTArgs(string[] args)
        {
            int midiPort = -1;

            if (args.IndexExists(1))
            {
                // check if the arg is show
                if (args[1] == "show")
                {
                    // Using a for loop we want the index
                    for (var index = 0; index < MidiDevice.Outputs.Count; index++)
                        Console.WriteLine($"Port {index} | {MidiDevice.Outputs[index].Name}");

                    AcceptNextCommand();
                }
                // Woah, the port arg!
                else if (int.TryParse(args[1], out midiPort))
                {

                    Transcribe(MidiDevice.Outputs[midiPort], "C:\\source\\ADT\\ExportedData");
                }
                // assume it is the directory arg
                else
                {
                    int number;
                    var success = int.TryParse(args[2], out number);

                    if (success)
                    {
                        // Minus 1 from the count as number is zero indexed
                        if (number > MidiDevice.Outputs.Count - 1)
                            throw new ArgumentException("Chosen midi port does not exist");

                        Transcribe(MidiDevice.Outputs[number], args[1]);
                    }
                    else
                    {
                        Transcribe(MidiDevice.Outputs.First(), args[1]);
                    }
                }
            }
        }

        private static void Transcribe(MidiOutputDevice midiOutputDevice, string modelDirectory)
        {
            Console.WriteLine("Beginning predictions!");
            Console.WriteLine("> Press any key to stop...");

            DrumPredictor.Predict(midiOutputDevice, modelDirectory);

            Console.ReadKey();

            DrumPredictor.Stop();
            
            AcceptNextCommand();
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
            while (!int.TryParse(Console.ReadLine(), out value)) Console.WriteLine("Invalid value");
            return value;
        }

        /// <summary>
        ///     Write the DrumTable and reads input
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private static int GetDrumTypeInput()
        {
            // Create a table with all of the options of DrumSoundType, and ask read integer for chosen DrumSoundType
            var table = SetupTable<DrumSoundType>(new[] {"Drum", "Id"});
            table.Options.EnableCount = false;
            table.Write();

            Console.WriteLine("> Select which drum you are going to play");

            var drumTypeInts = EnumExtensions.GetEnumValues(typeof(DrumSoundType));
            // Pass in the drum type enum to ensure it's a valid choice
            return ReadInteger(drumTypeInts.ToArray());
        }

        /// <summary>
        ///     Creates the table from given enum and columns
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private static ConsoleTable SetupTable<T>(string[] columns) where T : Enum
        {
            var table = new ConsoleTable(columns);

            foreach (var enumValue in Enum.GetValues(typeof(T))) table.AddRow($"{enumValue}", (int) enumValue);

            return table;
        }
    }
}