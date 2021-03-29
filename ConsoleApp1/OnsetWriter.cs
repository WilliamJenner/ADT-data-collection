using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using CSCore;
using CSCore.Codecs.WAV;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace OnsetDataGeneration
{
    public class OnsetWriter
    {
        private readonly float[] Buffer;
        private readonly IWaveSource ConvertedSource;
        public double Frequency;
        private MemoryStream OnsetStream;
        private WaveWriter OnsetWaveWriter;
        private int Read;
        private readonly SoundInSource SoundInSource;
        private readonly WaveWriter WaveFileWriter;
        private readonly MemoryStream WavFileStream;
        private SingleBlockNotificationStream NotificationSource;
        private PeakMeter pm;

        private float PreviousPeak;

        public ConcurrentDictionary<string, float> ProcessedOnsetPeaks;
        public bool Detecting { get; set; }

        public OnsetWriter(ISoundIn soundIn, double frequency)
        {
            Frequency = frequency;
            Detecting = false;
            ProcessedOnsetPeaks = new ConcurrentDictionary<string, float>();

            //var options = DetectorOptions.Default;
            //options.ActivationThreshold = 10;
            //options.SliceLength = 10.0f;
            //options.SlicePaddingLength = 0.5f;
            //options.Online = false;
            //options.DetectionFunction = Detectors.SF;

            //OnsetDetector = new OnsetDetector.OnsetDetector(options, null);

            SoundInSource = new SoundInSource(soundIn);

            //SoundInSource.ToSampleSource()
                //.AppendSource(x =>
                //{
                //    var biQuad = new BiQuadFilterSource(x);
                //    biQuad.Filter = new BandpassFilter(SoundInSource.WaveFormat.SampleRate, frequency);
                //    return biQuad;
                //});

            WavFileStream = new MemoryStream();
            OnsetStream = new MemoryStream();

            WaveFileWriter = new WaveWriter(WavFileStream, SoundInSource.WaveFormat);
            OnsetWaveWriter = new WaveWriter(OnsetStream, SoundInSource.WaveFormat);
            
            NotificationSource = new SingleBlockNotificationStream(SoundInSource.ToSampleSource());

            Buffer = new float[SoundInSource.WaveFormat.BytesPerSecond/2];
            SoundInSource.DataAvailable += GetDataAvailable();

            //File.WriteAllText($"C:\\source\\ADT\\sound\\fft {Frequency}.csv", $"Time, Peak {Environment.NewLine}");

            pm = new PeakMeter(SoundInSource.ToSampleSource().AppendSource(x =>
            {
                var biQuad = new BiQuadFilterSource(x);
                biQuad.Filter = new BandpassFilter(SoundInSource.WaveFormat.SampleRate, frequency);
                return biQuad;
            })) {Interval = 10};
            pm.PeakCalculated += Pm_PeakCalculated;
        }

        private void Pm_PeakCalculated(object sender, PeakEventArgs e)
        {
            if (Detecting)
            {
                bool activated = e.PeakValue > 0.5 && e.PeakValue > PreviousPeak;

                //Debug.WriteLine(e.PeakValue);
                //File.AppendAllText($"C:\\source\\ADT\\sound\\fft {Frequency}.csv",
                //    $"{DateTime.Now:O}, {(activated ? e.PeakValue : 0)} {Environment.NewLine}");

                // If activated and this peak is greater than the previous
                if (activated)
                {
                    Console.WriteLine($"ONSET DETECTION AT {Frequency}");
                }

                var newPeakValue = (activated ? e.PeakValue : 0);

                ProcessedOnsetPeaks.AddOrUpdate(DateTime.Now.ToString("HH:mm:ss.f"), newPeakValue,
                    (time, oldValue) => oldValue > newPeakValue ? oldValue : newPeakValue);

                // Assign a new value to PreviousPeak
                PreviousPeak = e.PeakValue;
            }
        }

        public void DetectOnsets()
        {
            NotificationSource.SingleBlockRead += (sender, args) =>
            {
                var castedSender = ((SingleBlockNotificationStream) sender);

                if (castedSender.BaseSource != null)
                    try
                    {
                        //SoundInSource.DataAvailable += (sender, args) => { };
                        //Thread.Sleep(10);

                        //OnsetWaveWriter.Dispose();
                        //OnsetStream.Seek(0, SeekOrigin.Begin);

                        //IWaveSource onsetFileReader = new WaveFileReader(OnsetStream);

                        //OnsetStream = new MemoryStream();
                        //OnsetWaveWriter = new WaveWriter(new MemoryStream(), ConvertedSource.WaveFormat);
                        //SoundInSource.DataAvailable += GetDataAvailable();

                        //var notificationSource = new SingleBlockNotificationStream(ConvertedSource.ToSampleSource());

                        //notificationSource.SingleBlockRead += (s, a) => { Console.WriteLine(a.Left); };

                        var spectrumProvider = new FftProvider((sender as SingleBlockNotificationStream).WaveFormat.Channels,
                            FftSize.Fft4096);

                        spectrumProvider.Add(args.Left, args.Right);

                        var fftRead = new float[4096];

                        spectrumProvider.GetFftData(fftRead);

                        var vector = Vector<float>.Build.DenseOfArray(fftRead);
                        var mean = vector.Mean();

                        File.AppendAllText("fft.csv",  mean + ",");
                        Debug.WriteLine(mean);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
            };
        }

        //public void Write(string filePath)
        //{
        //    SoundInSource.Dispose();
        //    WaveFileWriter.Dispose();
        //    OnsetWaveWriter.Dispose();

        //    WavFileStream.Seek(0, SeekOrigin.Begin);
        //    OnsetStream.Seek(0, SeekOrigin.Begin);

        //    IWaveSource waveFileReader = new WaveFileReader(WavFileStream);
        //    IWaveSource onsetFileReader = new WaveFileReader(OnsetStream);

        //    WriteStreamToFile(filePath, waveFileReader);

        //    var onsets = new List<Onset>();
        //    onsets.AddRangeWithCallback(OnsetDetector.Detect(onsetFileReader.ToSampleSource()), enumerable =>
        //    {
        //        foreach (var onset in enumerable) Console.WriteLine($"{onset.OnsetTime} {onset.OnsetAmplitude}");
        //    });

        //    File.WriteAllLines(filePath + ".csv",
        //        onsets.Select(s => s.ToString()).ToArray());
        //}

        private static void WriteStreamToFile(string filePath, IWaveSource waveSource)
        {
            using var w = new WaveWriter(filePath, waveSource.WaveFormat);
            var read = 0;
            var buffer = new byte[waveSource.WaveFormat.BytesPerSecond];

            while ((read = waveSource.Read(buffer, 0, buffer.Length)) > 0
            ) //read all available data from the source chain
                w.Write(buffer, 0, read); //write the read data to the wave file
        }

        private EventHandler<DataAvailableEventArgs> GetDataAvailable()
        {
            return (s, e) =>
            {
                    //keep reading as long as we still get some data
                    //if you're using such a loop, make sure that soundInSource.FillWithZeros is set to false
                    while ((Read = pm.Read(Buffer, 0, Buffer.Length)) > 0)
                    {
                        //write the read data to a file
                        //// ReSharper disable once AccessToDisposedClosure
                        //if (!WaveFileWriter.IsDisposed && WavFileStream.CanRead) WaveFileWriter.Write(Buffer, 0, Read);
                        //if (!OnsetWaveWriter.IsDisposed && OnsetStream.CanRead) OnsetWaveWriter.Write(Buffer, 0, Read);
                    }
            };
        }
    }
}