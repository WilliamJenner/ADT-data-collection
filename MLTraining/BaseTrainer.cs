using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using MLTraining.DataStructures;

namespace MLTraining
{
    public abstract class BaseTrainer
    {
        private readonly string ModelName;
        private readonly string BaseDatasetsRelativePath;
        protected readonly string BaseModelsRelativePath;

        private readonly string ModelPath;
        protected readonly string ModelRelativePath;
        private readonly string TestDataPath;
        private readonly string AllDataRelativePath;
        private readonly string TestDataRelativePath;

        private readonly string TrainDataPath;
        private readonly string AllDataPath;
        private readonly string TrainDataRelativePath;

        protected MLContext MlContext;

        protected BaseTrainer(string modelName, string baseDatasetsRelativePath, string baseModelsRelativePath)
        {
            ModelName = modelName;
            MlContext = new MLContext(0);

            BaseDatasetsRelativePath = baseDatasetsRelativePath;
            BaseModelsRelativePath = baseModelsRelativePath;

            var modelPath = $"{modelName}.zip";
            var testFile = $"test.csv";
            var trainFile = $"train.csv";
            var allFile = $"all.csv";

            ModelPath = Path.Combine(baseModelsRelativePath, modelPath);
            TestDataPath = Path.Combine(baseDatasetsRelativePath, testFile);
            TrainDataPath = Path.Combine(baseDatasetsRelativePath, trainFile);
            AllDataPath = Path.Combine(baseDatasetsRelativePath, allFile);
        }

        protected MulticlassClassificationMetrics Train(string[] columns)
        {
            var metrics = BuildTrainEvaluateAndSaveModel(MlContext, columns);
            PredictExamples(MlContext);

            return metrics;
        }

        private MulticlassClassificationMetrics BuildTrainEvaluateAndSaveModel(MLContext mlContext, string[] inputColumnNames)
        {
            var featuresColumnName = "Features";
            var drumTypeColumnName = nameof(DrumTypeData.Type);
            var keyColumn = "KeyColumn";

            // STEP 1: Common data loading configuration
            var testTrainSplit = TrainingDataFactory.GetSplitTrainingData(mlContext, BaseDatasetsRelativePath);

            // STEP 2: Common data process configuration with pipeline data transformations
            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "KeyColumn",
                    inputColumnName: nameof(DrumTypeData.Type))
                .Append(mlContext.Transforms.Concatenate("Features", inputColumnNames));

            // STEP 3: Set the training algorithm, then append the trainer to the pipeline  
            var trainer = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "KeyColumn", featureColumnName: "Features", historySize: 100, optimizationTolerance: 1E-20f)
            .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: nameof(DrumTypeData.Type), inputColumnName: "KeyColumn"));

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            // STEP 4: Train the model fitting to the DataSet
            ITransformer trainedModel = trainingPipeline.Fit(testTrainSplit.TrainSet);

            // STEP 5: Evaluate the model and show accuracy stats
            var predictions = trainedModel.Transform(testTrainSplit.TestSet);
            var metrics = mlContext.MulticlassClassification.Evaluate(predictions, nameof(DrumTypeData.Type));
            //PrintMultiClassClassificationMetrics(ModelName, metrics);

            if (!Directory.Exists(BaseModelsRelativePath))
            {
                Directory.CreateDirectory(BaseModelsRelativePath);
            }

            // STEP 6: Save/persist the trained model to a .ZIP file
            mlContext.Model.Save(trainedModel, testTrainSplit.TrainSet.Schema, ModelPath);

            // STEP 7: Return the metrics
            return metrics;
        }

        public void FitAndCrossValidate(MLContext mlContext, string[] inputColumnNames, bool save = false)
        {
            var trainingDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TrainDataPath, hasHeader: true, separatorChar: ',');
            var testDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TestDataPath, hasHeader: true, separatorChar: ',');
            var allData = mlContext.Data.LoadFromTextFile<DrumTypeData>(AllDataPath, hasHeader: true, separatorChar: ',');

            Console.WriteLine($"{DateTime.Now:mm:ss tt zz} | Starting {ModelName}");
            var dynamicPipeline =
                // Concatenate all the features together into one column 'Features'.
                mlContext.Transforms.Concatenate("Features", inputColumnNames)
                    // Note that the label is text, so it needs to be converted to key.
                    .Append(mlContext.Transforms.Conversion.MapValueToKey("Type"), TransformerScope.TrainTest)
                    // Use the multi-class SDCA model to predict the label using features.
                    .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "Type"));

            var trainTestSplit = mlContext.Data.TrainTestSplit(allData, testFraction: 0.2D);

            // Train the model.
            var model = dynamicPipeline.Fit(trainTestSplit.TrainSet);
            // Compute quality metrics on the test set.
            var cvMetrics = mlContext.MulticlassClassification.Evaluate(model.Transform(trainTestSplit.TestSet), labelColumnName: "Type");
            Console.WriteLine($"{ModelName} accuracy |\t" + cvMetrics.MicroAccuracy);

            // Now run the 5-fold cross-validation experiment, using the same pipeline.
            var cvResults = mlContext.MulticlassClassification.CrossValidate(allData, dynamicPipeline, numberOfFolds: 5, labelColumnName: "Type");

            // The results object is an array of 5 elements. For each of the 5 folds, we have metrics, model and scored test data.
            // Let's compute the average micro-accuracy.
            var microAccuracies = cvResults.Select(r => r.Metrics.MicroAccuracy);
            Console.WriteLine($"{ModelName} Cross Validation Accuracy |\t" + microAccuracies.Average());

            

            if (save)
            {
                mlContext.Model.Save(model, trainingDataView.Schema, ModelPath);
                Console.WriteLine("The model is saved to {0}", ModelPath);
            }
        }

        public void PredictExamples(MLContext mlContext)
        {
            DataViewSchema predictionPipelineSchema;
            ITransformer predictionPipeline = mlContext.Model.Load(ModelPath, out predictionPipelineSchema);

            var predEngine = mlContext.Model.CreatePredictionEngine<DrumTypeData, DrumTypePrediction>(predictionPipeline);

            // During prediction we will get Score column with 3 float values.
            // We need to find way to map each score to original label.
            // In order to do what we need to get TrainingLabelValues from Score column.
            // TrainingLabelValues on top of Score column represent original labels for i-th value in Score array.
            // Let's look how we can convert key value for PredictedLabel to original labels.
            // We need to read KeyValues for "PredictedLabel" column.
            VBuffer<float> keys = default;
            predEngine.OutputSchema["PredictedLabel"].GetKeyValues(ref keys);
            
            //Console.WriteLine("===== Predicting snare ====");
            //var prediction = predEngine.Predict(SampleDrumTypeData.Snare);

            //for (int i = 0; i < prediction.Score.Length; i++)
            //{
            //    Console.WriteLine($"{i} : {prediction.Score[i] * 100}");
            //}

            //Console.WriteLine("===== Predicting Cymbal ====");
            //prediction = predEngine.Predict(SampleDrumTypeData.Cymbal);

            //for (int i = 0; i < prediction.Score.Length; i++)
            //{
            //    Console.WriteLine($"{i} : {prediction.Score[i] * 100}");
            //}

            //Console.WriteLine("===== Predicting kick ====");
            //prediction = predEngine.Predict(SampleDrumTypeData.Kick);

            //for (int i = 0; i < prediction.Score.Length; i++)
            //{
            //    Console.WriteLine($"{i} : {prediction.Score[i] * 100}");
            //}
        }

        public void PrintMultiClassClassificationMetrics(string name, MulticlassClassificationMetrics metrics)
        {
            Console.WriteLine("************************************************************");
            Console.WriteLine($"*    Metrics for {name} multi-class classification model   ");
            Console.WriteLine("*-----------------------------------------------------------");
            Console.WriteLine(
                $"    AccuracyMacro = {metrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine(
                $"    AccuracyMicro = {metrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"    LogLoss = {metrics.LogLoss:0.####}, the closer to 0, the better");
            Console.WriteLine($"    LogLossReduction = {metrics.LogLossReduction:0.####}, the closer to 0, the better");

            for (var index = 0; index < metrics.PerClassLogLoss.Count; index++)
            {
                var logLoss = metrics.PerClassLogLoss[index];
                Console.WriteLine(
                    $"    LogLoss for class {index} = {logLoss:0.####}, the closer to 0, the better");
            }

            Console.WriteLine("************************************************************");
        }
    }
}