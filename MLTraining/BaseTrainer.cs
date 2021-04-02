using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using MLTraining.DataStructures;

namespace MLTraining
{
    public abstract class BaseTrainer
    {
        private readonly string _modelName;

        //private readonly string BaseDatasetsRelativePath = "C:\\source\\ADT\\ConsoleApp1\\sound\\experiment";
        private readonly string BaseDatasetsRelativePath = "C:\\source\\ADT\\MLTraining\\data";

        protected readonly string BaseModelsRelativePath = "C:\\source\\ADT\\MLTraining\\models";

        private readonly string ModelPath;
        protected readonly string ModelRelativePath;
        private readonly string TestDataPath;
        private readonly string AllDataRelativePath;
        private readonly string TestDataRelativePath;

        private readonly string TrainDataPath;
        private readonly string AllDataPath;
        private readonly string TrainDataRelativePath;

        protected MLContext MlContext;

        protected BaseTrainer(string modelName)
        {
            _modelName = modelName;
            MlContext = new MLContext(0);
            ModelRelativePath = $"{BaseModelsRelativePath}\\{modelName}.zip";
            TestDataRelativePath = $"{BaseDatasetsRelativePath}\\test.csv";
            TrainDataRelativePath = $"{BaseDatasetsRelativePath}\\train.csv";
            AllDataRelativePath = $"{BaseDatasetsRelativePath}\\all.csv";
            ModelPath = GetAbsolutePath(ModelRelativePath);
            TestDataPath = GetAbsolutePath(TestDataRelativePath);
            TrainDataPath = GetAbsolutePath(TrainDataRelativePath);
            AllDataPath = GetAbsolutePath(AllDataRelativePath);
        }

        private string AppPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

        public void Train(string[] columns)
        {
            //1.
            //BuildTrainEvaluateAndSaveModel(MlContext, columns);
            FitAndCrossValidate(MlContext, columns, true);

        }

        private void BuildTrainEvaluateAndSaveModel(MLContext mlContext, string[] inputColumnNames)
        {
            var featuresColumnName = "Features";
            var drumTypeColumnName = nameof(DrumTypeData.Type);
            var keyColumn = "KeyColumn";

            // STEP 1: Common data loading configuration
            var trainingDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TrainDataPath, hasHeader: true, separatorChar: ',');
            var testDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TestDataPath, hasHeader: true, separatorChar: ',');
            var allData = mlContext.Data.LoadFromTextFile<DrumTypeData>(AllDataPath, hasHeader: true, separatorChar: ',');


            // STEP 2: Common data process configuration with pipeline data transformations
            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "KeyColumn",
                    inputColumnName: nameof(DrumTypeData.Type))
                .Append(mlContext.Transforms.Concatenate("Features", inputColumnNames));

            // STEP 3: Set the training algorithm, then append the trainer to the pipeline  
            var trainer = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "KeyColumn", featureColumnName: "Features")
            .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: nameof(DrumTypeData.Type), inputColumnName: "KeyColumn"));

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            // STEP 4: Train the model fitting to the DataSet
            Console.WriteLine("=============== Training the model ===============");
            ITransformer trainedModel = trainingPipeline.Fit(trainingDataView);

            // STEP 5: Evaluate the model and show accuracy stats
            Console.WriteLine("===== Evaluating Model's accuracy with Test data =====");
            var predictions = trainedModel.Transform(testDataView);
            var metrics = mlContext.MulticlassClassification.Evaluate(predictions, nameof(DrumTypeData.Type), "Score");
            PrintMultiClassClassificationMetrics(_modelName, metrics);

            // STEP 6: Save/persist the trained model to a .ZIP file
            mlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);
            Console.WriteLine("The model is saved to {0}", ModelPath);
        }

        public void FitAndCrossValidate(MLContext mlContext, string[] inputColumnNames, bool save = false)
        {
            var trainingDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TrainDataPath, hasHeader: true, separatorChar: ',');
            var testDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TestDataPath, hasHeader: true, separatorChar: ',');
            var allData = mlContext.Data.LoadFromTextFile<DrumTypeData>(AllDataPath, hasHeader: true, separatorChar: ',');

            Console.WriteLine($"{DateTime.Now:mm:ss tt zz} | Starting {_modelName}");
            var dynamicPipeline =
                // Concatenate all the features together into one column 'Features'.
                mlContext.Transforms.Concatenate("Features", inputColumnNames)
                    // Note that the label is text, so it needs to be converted to key.
                    .Append(mlContext.Transforms.Conversion.MapValueToKey("Type"), TransformerScope.TrainTest)
                    // Use the multi-class SDCA model to predict the label using features.
                    .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "Type"));

            var trainTestSplit = mlContext.Data.TrainTestSplit(allData);

            // Train the model.
            var model = dynamicPipeline.Fit(trainTestSplit.TrainSet);
            // Compute quality metrics on the test set.
            var cvMetrics = mlContext.MulticlassClassification.Evaluate(model.Transform(trainTestSplit.TestSet), labelColumnName: "Type");
            Console.WriteLine($"{_modelName}:" + cvMetrics.MicroAccuracy);

            // Now run the 5-fold cross-validation experiment, using the same pipeline.
            var cvResults = mlContext.MulticlassClassification.CrossValidate(allData, dynamicPipeline, numberOfFolds: 5, labelColumnName: "Type");

            // The results object is an array of 5 elements. For each of the 5 folds, we have metrics, model and scored test data.
            // Let's compute the average micro-accuracy.
            var microAccuracies = cvResults.Select(r => r.Metrics.MicroAccuracy);
            Console.WriteLine($"{_modelName} CV:" + microAccuracies.Average());

            if (save)
            {
                mlContext.Model.Save(model, trainingDataView.Schema, ModelPath);
                Console.WriteLine("The model is saved to {0}", ModelPath);
            }
        }

        public string GetAbsolutePath(string relativePath)
        {
            var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            var assemblyFolderPath = _dataRoot.Directory.FullName;

            var fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
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