using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;

namespace OnsetDetection
{
    public class BandpassOnsetDetector
    {
        private float[] Buffer;
        private readonly SoundInSource SoundInSource;
        private readonly Action<OnsetPeakModel, double> DetectionCallback;
        private double Frequency;
        private readonly PeakMeter pm;
        private float PreviousPeak;
        private FftProvider FftProvider;
        private int Read;

        public ConcurrentDictionary<string, float> ProcessedOnsetPeaks;
        /// <summary>
        ///     Predicts Onsets in an ISoundIn, by processing through a BandpassFilter,
        ///     then a peak meter, and analyzing each peak
        /// </summary>
        /// <param name="soundIn"></param>
        /// <param name="frequency">Frequency for the applied BandpassFilter</param>
        /// <param name="peakDetectionCallback">callback for when a peak is processed</param>
        /// <param name="interval">Interval between peak calculations. The interval is specified in milliseconds. </param>
        public BandpassOnsetDetector(ISoundIn soundIn, double frequency, Action<OnsetPeakModel, double> peakDetectionCallback, int interval = 100)
        {
            Frequency = frequency;
            Detecting = false;
            DetectionCallback = peakDetectionCallback;
            ProcessedOnsetPeaks = new ConcurrentDictionary<string, float>();
            SoundInSource = new SoundInSource(soundIn);
            FftProvider = new FftProvider(SoundInSource.WaveFormat.Channels, FftSize.Fft4096);
            Buffer = new float[SoundInSource.WaveFormat.BytesPerSecond / 2];
            SoundInSource.DataAvailable += GetDataAvailable();

            pm = new PeakMeter(SoundInSource.ToSampleSource()
                .AppendSource(x =>
                {
                    // double the volume to catch more peaks
                    var biQuad = new GainSource(x) { Volume = 1f };
                    return biQuad;
                })
                .AppendSource(x =>
                {
                    var biQuad = new BiQuadFilterSource(x);
                    biQuad.Filter = new BandpassFilter(SoundInSource.WaveFormat.SampleRate, frequency);
                    return biQuad;
                }))
            { Interval = interval };


            pm.PeakCalculated += OnPmOnPeakCalculated;
        }

        public bool Detecting { get; set; }

        private void OnPmOnPeakCalculated(object sender, PeakEventArgs e)
        {
            try
            {
                if (Detecting)
                {
                    var activated = e.PeakValue > 0.5 && e.PeakValue > PreviousPeak;
                    var fftData = new float[(int)FftSize.Fft4096];

                    // If activated and this peak is greater than the previous
                    if (activated)
                    {
                        // Write FFT if activated
                        var numberOfSamples = (int)(SoundInSource.WaveFormat.BytesPerSecond / 2.0) / SoundInSource.WaveFormat.SampleRate;
                        FftProvider.Add(Buffer, numberOfSamples);
                        FftProvider.GetFftData(fftData);
                    }

                    var newPeakValue = activated ? e.PeakValue : 0;

                    var onsetPeak = new OnsetPeakModel(newPeakValue, fftData);

                    // Assign a new value to PreviousPeak
                    PreviousPeak = e.PeakValue;
                    DetectionCallback(onsetPeak, Frequency);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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