using System;
using System.Linq;
using M;

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
                    Transcribe(MidiDevice.Outputs.First());
                }
                else
                {
                    ProcessArgs(args);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oops! An exception occurred.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try --help");
            }
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
                    BeginDataExport();
                    break;
                case Arguments.Train:
                    TrainModel();
                    break;
                case Arguments.Port:
                    ProcessPortArgs(args);
                    break;
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("help");
        }
        
        static void BeginDataExport()
        {

        }

        static void TrainModel()
        {

        }

        static void ProcessPortArgs(string[] args)
        {
            int number;
            bool success = Int32.TryParse(args[1], out number);


            // If success, then transcribe to a specific port
            // Else, try second arg for "show", else, throw
            if (success)
            {
                // Minus 1 from the count as number would is zero indexed
                if (number > (MidiDevice.Outputs.Count - 1))
                {
                    throw new ArgumentException("Chosen midi port does not exist");
                }

                Transcribe(MidiDevice.Outputs[number]);
            }
            else
            {
                if (args[1] != "show")
                {
                    throw new ArgumentException("Second --port argument not recognized", nameof(args));
                }
                
                // Using a for loop we want the index
                for (var index = 0; index < MidiDevice.Outputs.Count; index++)
                {
                    Console.WriteLine($"Port {index} | {MidiDevice.Outputs[index].Name}");
                }
            }
        }

        static void Transcribe(MidiOutputDevice midiOutputDevice)
        {

        }
    }
}
