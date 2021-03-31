using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CSCore.SoundIn;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace OnsetPredictions
{
    public interface IOnsetWriterBuilder
    {
        void Build(IEnumerable<double> frequencies);
        void DetectAndBroadcast();
        ConcurrentDictionary<double, OnsetDetector> GetOnsetWriters();
        void Reset();
    }

    public class DrumPredictor : IOnsetWriterBuilder
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-5.0
        // Using ConcurrentDictionary as it can handle multiple threads concurrently / large collections
        private ConcurrentDictionary<double, OnsetDetector> OnsetWriters = new ConcurrentDictionary<double, OnsetDetector>();
        private ConcurrentBag<IntervalPredictionEngine> PredictionEngines = new ConcurrentBag<IntervalPredictionEngine>();
        private MidiBroadcaster MidiBroadcaster;
        private DrumTypeData LatestData;
        private ISoundIn soundIn;

        public DrumPredictor(ISoundIn soundIn)
        {
            this.soundIn = soundIn;
            MidiBroadcaster = new MidiBroadcaster(true);
            LatestData = new DrumTypeData();
            PredictionEngines = new ConcurrentBag<IntervalPredictionEngine>()
            {
                new IntervalPredictionEngine("HighFreqDrumTypeClassificationModel", () => LatestData, MidiBroadcaster.Broadcast),
                new IntervalPredictionEngine("MedFreqDrumTypeClassificationModel", () => LatestData, MidiBroadcaster.Broadcast),
                new IntervalPredictionEngine("LowFreqDrumTypeClassificationModel", () => LatestData, MidiBroadcaster.Broadcast),
                //new IntervalPredictionEngine("AllBandDrumTypeClassificationModel", () => LatestData, MidiBroadcaster.Broadcast),

            };

            Reset();
        }

        /// <summary>
        /// Creates an onset writer for each frequency in parallel and begins detecting
        /// </summary>
        /// <param name="frequencies"></param>
        public void Build(IEnumerable<double> frequencies)
        {
            Parallel.ForEach(frequencies, frequency =>
            {
                var writer = new OnsetDetector(this.soundIn, frequency, PeakDetectionCallback);
                OnsetWriters.AddOrUpdate(frequency, writer, (d, oldWriter) => writer);
            });
        }

        public void DetectAndBroadcast()
        {
            Parallel.ForEach(GetOnsetWriters(), (writer) =>
            {
                writer.Value.Detecting = true;
            });

            Parallel.ForEach(PredictionEngines, ((predictionEngine) =>
            {
                predictionEngine.Predicting = true;
            }));
        }

        public ConcurrentDictionary<double, OnsetDetector> GetOnsetWriters()
        {
            return OnsetWriters;
        }

        public void Reset()
        {
            OnsetWriters.Clear();
        }

        private void PeakDetectionCallback(float peakValue, double frequency)
        {
            LatestData.SetValues(peakValue, frequency);
        }
    }
}
