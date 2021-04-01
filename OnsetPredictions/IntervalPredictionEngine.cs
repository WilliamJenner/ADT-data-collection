using System;
using System.Collections.Generic;
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
        private readonly string _modelName;
        private Func<DrumTypeData> GetLatestData;
        private PredictionEngine<DrumTypeData, DrumTypePrediction> PredictionEngine;
        private System.Timers.Timer Timer;
        public bool Predicting = false;
        private Action<DrumSoundType> OnPredict;

        /// <summary>
        /// Predicts the type of drum on an interval (specified in milliseconds).
        /// Must pass in a way to access the latest data
        /// </summary>
        /// <param name="getLatestData">Func with no parameters which returns the latest DrumTypeData</param>
        /// <param name="onPredict">Callback which returns predictions</param>
        /// <param name="interval">Interval for the timer (milliseconds). Default is 100 </param>
        public IntervalPredictionEngine(string modelName, Func<DrumTypeData> getLatestData, Action<DrumSoundType> onPredict, double interval = 100)
        {
            _modelName = modelName;
            GetLatestData = getLatestData;
            OnPredict = onPredict;
            var mlContext = new MLContext();
            DataViewSchema predictionPipelineSchema;
            ITransformer predictionPipeline = mlContext.Model.Load($"C:\\source\\ADT\\MLTraining\\models\\{modelName}.zip", out predictionPipelineSchema);
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
            Timer = new System.Timers.Timer(interval);
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
                var latestData = GetLatestData();

                if (latestData.HasValue())
                {
                    var prediction = PredictionEngine.Predict(latestData);
                    //PrintScore(prediction.Score);

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

                    if (highScore * 100 > 90)
                    {
                        OnPredict((DrumSoundType) highScoreIndex);
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
                Console.WriteLine($"*    {_modelName} Prediction {DateTime.Now:O}   ");
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
