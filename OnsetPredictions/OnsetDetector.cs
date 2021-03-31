using System;
using System.Collections.Concurrent;
using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;

namespace OnsetPredictions
{
    public class OnsetDetector
    {
        private readonly float[] Buffer;
        private readonly SoundInSource SoundInSource;
        private readonly Action<float, double> DetectionCallback;
        public double Frequency;
        private SingleBlockNotificationStream NotificationSource;
        private readonly PeakMeter pm;
        private float PreviousPeak;


        public ConcurrentDictionary<string, float> ProcessedOnsetPeaks;

        private int Read;

        /// <summary>
        ///     Predicts Onsets in an ISoundIn, by processing through a BandpassFilter,
        ///     then a peak meter, and analyzing each peak
        /// </summary>
        /// <param name="soundIn"></param>
        /// <param name="frequency">Frequency for the applied BandpassFilter</param>
        /// <param name="peakDetectionCallback">callback for when a peak is processed</param>
        /// <param name="interval">Interval between peak calculations. The interval is specified in milliseconds. </param>
        public OnsetDetector(ISoundIn soundIn, double frequency, Action<float, double> peakDetectionCallback, int interval = 100)
        {
            Frequency = frequency;
            Detecting = false;
            DetectionCallback = peakDetectionCallback;
            ProcessedOnsetPeaks = new ConcurrentDictionary<string, float>();
            SoundInSource = new SoundInSource(soundIn);
            NotificationSource = new SingleBlockNotificationStream(SoundInSource.ToSampleSource());
            Buffer = new float[SoundInSource.WaveFormat.BytesPerSecond / 2];

            SoundInSource.DataAvailable += GetDataAvailable();


            pm = new PeakMeter(SoundInSource.ToSampleSource().AppendSource(x =>
            {
                var biQuad = new BiQuadFilterSource(x);
                biQuad.Filter = new BandpassFilter(SoundInSource.WaveFormat.SampleRate, frequency);
                return biQuad;
            })) {Interval = interval};


            pm.PeakCalculated += OnPmOnPeakCalculated;
        }

        public bool Detecting { get; set; }

        private void OnPmOnPeakCalculated(object sender, PeakEventArgs e)
        {
            if (Detecting)
            {
                var activated = e.PeakValue > 0.5 && e.PeakValue > PreviousPeak;

                var newPeakValue = activated ? e.PeakValue : 0;

                if (activated)
                {
                    Console.WriteLine($"ONSET AT {Frequency}");
                }

                // Assign a new value to PreviousPeak
                PreviousPeak = e.PeakValue;

                DetectionCallback(newPeakValue, Frequency);
            }
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