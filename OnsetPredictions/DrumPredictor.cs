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
using OnsetDataGeneration;

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
        private ISoundIn soundIn;

        private DrumTypeData LatestHighFreqData;
        private DrumTypeData LatestMedFreqData;
        private DrumTypeData LatestLowFreqData;
        private DrumTypeData LatestData;


        public DrumPredictor(ISoundIn soundIn)
        {
            this.soundIn = soundIn;
            MidiBroadcaster = new MidiBroadcaster(true);
            
            LatestHighFreqData = new DrumTypeData();
            LatestMedFreqData = new DrumTypeData();
            LatestLowFreqData = new DrumTypeData();
            LatestData = new DrumTypeData();

            PredictionEngines = new ConcurrentBag<IntervalPredictionEngine>()
            {
                new IntervalPredictionEngine("HighFrequencyDrumTypeClassificationModel", () => LatestHighFreqData, MidiBroadcaster.Broadcast),
                new IntervalPredictionEngine("MediumFrequencyDrumTypeClassificationModel", () => LatestMedFreqData, MidiBroadcaster.Broadcast),
                new IntervalPredictionEngine("LowFrequencyDrumTypeClassificationModel", () => LatestLowFreqData, MidiBroadcaster.Broadcast),
                new IntervalPredictionEngine("AllBandDrumTypeClassificationModel", () => LatestData, MidiBroadcaster.Broadcast),
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

        private void PeakDetectionCallback(OnsetPeakModel peakValue, double frequency)
        {
            LatestData.SetValues(peakValue.PeakValue, peakValue.Average(), peakValue.Mean(), peakValue.L1Norm(), frequency);

            if (frequency < 2000)
            {
                LatestLowFreqData.SetValues(peakValue.PeakValue, peakValue.Average(), peakValue.Mean(), peakValue.L1Norm(), frequency);
            }  
            else if (frequency >= 2000 && frequency < 10000)
            {
                LatestMedFreqData.SetValues(peakValue.PeakValue, peakValue.Average(), peakValue.Mean(), peakValue.L1Norm(), frequency);
            }
            else
            {
                LatestHighFreqData.SetValues(peakValue.PeakValue, peakValue.Average(), peakValue.Mean(), peakValue.L1Norm(), frequency);
            }
        }
    }
}
