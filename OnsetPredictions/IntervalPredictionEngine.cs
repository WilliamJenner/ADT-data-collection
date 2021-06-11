using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.ML;
using Microsoft.ML.Data;
using MLTraining.DataStructures;

namespace OnsetPredictions
{
    public class IntervalPredictionEngine : IDisposable
    {
        // % score for a prediction to be broadcast
        private const int INTERVAL_SCORE_TOLERANCE = 80;

        private readonly string _modelPath;
        private Func<DrumTypeData> GetLatestData;
        private PredictionEngine<DrumTypeData, DrumTypePrediction> PredictionEngine;
        private Timer Timer;
        public bool Predicting = false;
        private Action<DrumSoundType, double, string> OnPredict;

        /// <summary>
        /// Predicts the type of drum on an interval (specified in milliseconds).
        /// Must pass in a way to access the latest data
        /// </summary>
        /// <param name="getLatestData">Func with no parameters which returns the latest DrumTypeData</param>
        /// <param name="onPredict">Callback which returns predictions</param>
        /// <param name="interval">Interval for the timer (milliseconds). Default is 100 </param>
        public IntervalPredictionEngine(string modelPath, Func<DrumTypeData> getLatestData, Action<DrumSoundType, double, string> onPredict, double interval = 100)
        {
            _modelPath = modelPath;
            GetLatestData = getLatestData;
            OnPredict = onPredict;
            var mlContext = new MLContext();
            DataViewSchema predictionPipelineSchema;
            ITransformer predictionPipeline = mlContext.Model.Load(modelPath, out predictionPipelineSchema);
            // Create PredictionEngine
            PredictionEngine = mlContext.Model.CreatePredictionEngine<DrumTypeData, DrumTypePrediction>(predictionPipeline);
            SetTimer(interval);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval">Interval property passed to <see cref="System.Timers.Timer"/>. Specified in milliseconds</param>
        private void SetTimer(double interval)
        {
            // Create a timer with a two second interval.
            Timer = new Timer(interval);
            // Hook up the Elapsed event for the timer. 
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Predict();
        }

        private void Predict()
        {
            if (Predicting)
            {
                Debug.WriteLine($"{DateTime.Now:mm’:’ss.fffffffK} | Prediction {_modelPath}");
                var latestData = GetLatestData();

                if (latestData.HasValue())
                {
                    var prediction = PredictionEngine.Predict(latestData);
                    //PrintScore(prediction.Score);
                    latestData.ResetData();
                    // We will have to loop through the scores anyway to get highest, may as well do it all in one go
                    float highScore = 0;
                    int highScoreIndex = 0;
                    for (int i = 0; i < prediction.Score.Length; i++)
                    {
                        if (prediction.Score[i] > highScore)
                        {
                            highScore = prediction.Score[i];
                            highScoreIndex = i;
                        }
                    }

                    if (highScore * 100 > INTERVAL_SCORE_TOLERANCE)
                    {
                        OnPredict((DrumSoundType) highScoreIndex, highScore * 100, _modelPath);
                    }

                    
                }
            }
        }

        public void Dispose()
        {
            PredictionEngine?.Dispose();
            Timer?.Stop();
            Timer?.Dispose();
        }

        private void PrintScore(float[] score)
        {
            var maxScore = score.Max();

            //if (maxScore * 100 > 90)
            //{

                Console.WriteLine("************************************************************");
                Console.WriteLine($"*    {_modelPath} Prediction {DateTime.Now:O}   ");
                Console.WriteLine("*-----------------------------------------------------------");

            for (int i = 0; i < score.Length; i++)
            {
                Console.WriteLine($"{(DrumSoundType)i}: {score[i]:P}");
            }

            ////var indexAtMax = score.ToList().IndexOf(maxScore);

            //    Console.WriteLine($"{(DrumSoundType) indexAtMax}: {score[indexAtMax]:P}");

                Console.WriteLine("************************************************************");
            //}
        }
    }
}
