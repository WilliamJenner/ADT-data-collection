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
        private float[] Buffer;
        private IWaveSource ConvertedSource;
        public double Frequency;
        private MemoryStream OnsetStream;
        private WaveWriter OnsetWaveWriter;
        private int Read;
        private ISoundIn SoundInSource;
        private WaveWriter WaveFileWriter;
        private MemoryStream WavFileStream;
        private SingleBlockNotificationStream NotificationSource;
        private PeakMeter pm;
        private IWaveSource WaveSource;

        private float PreviousPeak;

        public ConcurrentDictionary<string, float> ProcessedOnsetPeaks;
        public bool Detecting { get; set; }

        public OnsetWriter(ISoundIn soundIn, double frequency)
        {
            Frequency = frequency;
            SoundInSource = soundIn;
            Bootstrap();
        }

        public OnsetWriter(IWaveSource waveSource, double frequency)
        {
            Frequency = frequency;
            WaveSource = waveSource;
            Bootstrap(); // override detecting
        }

        private void Bootstrap(bool detecting = true)
        {
            var source = SoundInSource == null ? WaveSource : new SoundInSource(SoundInSource);
            
            Detecting = detecting;
            ProcessedOnsetPeaks = new ConcurrentDictionary<string, float>();
            NotificationSource = new SingleBlockNotificationStream(source.ToSampleSource());
            Buffer = new float[source.WaveFormat.BytesPerSecond / 2];

            pm = new PeakMeter(source.ToSampleSource().AppendSource(x =>
                {
                    var biQuad = new BiQuadFilterSource(x)
                    {
                        Filter = new BandpassFilter(source.WaveFormat.SampleRate, Frequency)
                    };
                    return biQuad;
                }))
                { Interval = 10 };
            pm.PeakCalculated += Pm_PeakCalculated;

            // Define how to read data based on source type
            switch (source)
            {
                case SoundInSource soundInSource:
                    soundInSource.DataAvailable += GetDataAvailable();
                    break;
                default: 
                    while ((Read = pm.Read(Buffer, 0, Buffer.Length)) > 0)
                    {

                    }
                    break;
            }
        }

        private void Pm_PeakCalculated(object sender, PeakEventArgs e)
        {
            Debug.WriteLine("Peak Calculated");

            if (Detecting)
            {
                bool activated = e.PeakValue > 0.5 && e.PeakValue > PreviousPeak;
                
                // If activated and this peak is greater than the previous
                if (activated)
                {
                    Console.WriteLine($"{DateTime.Now:T} | ONSET DETECTION AT {Frequency}");
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now:T} | No onset at {Frequency}");
                }

                var newPeakValue = (activated ? e.PeakValue : 0);

                ProcessedOnsetPeaks.AddOrUpdate(DateTime.Now.ToString("HH:mm:ss.f"), newPeakValue,
                    (time, oldValue) => oldValue > newPeakValue ? oldValue : newPeakValue);

                // Assign a new value to PreviousPeak
                PreviousPeak = e.PeakValue;
            }
        }

        //public void DetectOnsets()
        //{
        //    NotificationSource.SingleBlockRead += (sender, args) =>
        //    {
        //        var castedSender = ((SingleBlockNotificationStream) sender);

        //        if (castedSender.BaseSource != null)
        //            try
        //            {
        //                //SoundInSource.DataAvailable += (sender, args) => { };
        //                //Thread.Sleep(10);

        //                //OnsetWaveWriter.Dispose();
        //                //OnsetStream.Seek(0, SeekOrigin.Begin);

        //                //IWaveSource onsetFileReader = new WaveFileReader(OnsetStream);

        //                //OnsetStream = new MemoryStream();
        //                //OnsetWaveWriter = new WaveWriter(new MemoryStream(), ConvertedSource.WaveFormat);
        //                //SoundInSource.DataAvailable += GetDataAvailable();

        //                //var notificationSource = new SingleBlockNotificationStream(ConvertedSource.ToSampleSource());

        //                //notificationSource.SingleBlockRead += (s, a) => { Console.WriteLine(a.Left); };

        //                var spectrumProvider = new FftProvider((sender as SingleBlockNotificationStream).WaveFormat.Channels,
        //                    FftSize.Fft4096);

        //                spectrumProvider.Add(args.Left, args.Right);

        //                var fftRead = new float[4096];

        //                spectrumProvider.GetFftData(fftRead);

        //                var vector = Vector<float>.Build.DenseOfArray(fftRead);
        //                var mean = vector.Mean();

        //                File.AppendAllText("fft.csv",  mean + ",");
        //                Debug.WriteLine(mean);
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine(ex.ToString());
        //            }
        //    };
        //}

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