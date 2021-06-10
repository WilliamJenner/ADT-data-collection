using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSCore.SoundIn;

namespace OnsetDetection
{
    public interface IOnsetWriter
    {
        void BuildOnsetDetectors(IEnumerable<double> frequencies);
        void DetectAndBroadcast();
        ConcurrentDictionary<double, BandpassOnsetDetector> GetBandpassOnsetDetectors();
        void Reset();
    }

    public class OnsetWriter : IOnsetWriter
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-5.0
        // Using ConcurrentDictionary as it can handle multiple threads concurrently / large collections
        private ConcurrentDictionary<double, BandpassOnsetDetector> _bandpassOnsetDetectors = new ConcurrentDictionary<double, BandpassOnsetDetector>();
        private Action<OnsetPeakModel, double> _peakDetectionCallback;
        private ISoundIn _soundIn;
        
        /// <summary>
        /// Constructs an OnsetWriter, a class composed of multiple BandpassOnsetDetectors
        /// </summary>
        /// <param name="peakDetectionCallback">Callback function for when a peak is detected</param>
        public OnsetWriter(Action<OnsetPeakModel, double> peakDetectionCallback)
        {
            _soundIn = new WasapiCapture();
            _soundIn.Initialize();
            this._peakDetectionCallback = peakDetectionCallback;
            BuildOnsetDetectors(CriticalBandFactory.Get());
        }

        /// <summary>
        /// Creates an onset writer for each frequency in parallel
        /// </summary>
        /// <param name="frequencies"></param>
        public void BuildOnsetDetectors(IEnumerable<double> frequencies)
        {
            Parallel.ForEach(frequencies, frequency =>
            {
                var writer = new BandpassOnsetDetector(_soundIn, frequency, _peakDetectionCallback);
                _bandpassOnsetDetectors.AddOrUpdate(frequency, writer, (d, oldWriter) => writer);
            });
        }

        public void DetectAndBroadcast()
        {
            _soundIn.Start();
            Parallel.ForEach(GetBandpassOnsetDetectors(), (bandpassDetector) =>
            {
                bandpassDetector.Value.Detecting = true;
            });
        }

        public ConcurrentDictionary<double, BandpassOnsetDetector> GetBandpassOnsetDetectors()
        {
            return _bandpassOnsetDetectors;
        }

        public void Reset()
        {
            _soundIn.Stop();
            _bandpassOnsetDetectors.Clear();
        }
    }
}
