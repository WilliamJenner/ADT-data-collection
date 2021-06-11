using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CSCore.SoundIn;
using M;
using Microsoft.ML;
using MLTraining.DataStructures;
using OnsetDetection;

namespace OnsetPredictions
{
    public static class DrumPredictor
    {
        private static ConcurrentBag<IntervalPredictionEngine> PredictionEngines = new ConcurrentBag<IntervalPredictionEngine>();
        private static MidiBroadcaster MidiBroadcaster;

        private static DrumTypeData LatestHighFreqData;
        private static DrumTypeData LatestMedFreqData;
        private static DrumTypeData LatestLowFreqData;
        private static DrumTypeData LatestData;


        public static void Predict(MidiOutputDevice outputDevice, string modelDirectory)
        {
           

            MidiBroadcaster = new MidiBroadcaster(true, outputDevice);
            
            LatestHighFreqData = new DrumTypeData();
            LatestMedFreqData = new DrumTypeData();
            LatestLowFreqData = new DrumTypeData();
            LatestData = new DrumTypeData();

            PredictionEngines = new ConcurrentBag<IntervalPredictionEngine>()
            {
                new IntervalPredictionEngine(Path.Combine(modelDirectory, "HighFrequencyDrumTypeClassificationModel.zip" ), () => LatestHighFreqData, MidiBroadcaster.Broadcast),
                new IntervalPredictionEngine(Path.Combine(modelDirectory, "MediumFrequencyDrumTypeClassificationModel.zip" ), () => LatestMedFreqData, MidiBroadcaster.Broadcast),
                new IntervalPredictionEngine(Path.Combine(modelDirectory, "LowFrequencyDrumTypeClassificationModel.zip" ), () => LatestLowFreqData, MidiBroadcaster.Broadcast),
                new IntervalPredictionEngine(Path.Combine(modelDirectory, "AllBandDrumTypeClassificationModel.zip" ), () => LatestData, MidiBroadcaster.Broadcast),
            };
            
            Parallel.ForEach(PredictionEngines, engine => engine.Predicting = true);

            var onsetWriter = new OnsetWriter(PeakDetectionCallback);
            onsetWriter.DetectAndBroadcast();
        }

        public static void Stop()
        {
            MidiBroadcaster.Broadcasting = false;

            foreach (var intervalPredictionEngine in PredictionEngines)
            {
                intervalPredictionEngine.Dispose();
            }
        }

        private static void PeakDetectionCallback(OnsetPeakModel peakValue, double frequency)
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
