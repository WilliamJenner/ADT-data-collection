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
        private readonly string TestDataRelativePath;

        private readonly string TrainDataPath;
        private readonly string TrainDataRelativePath;

        protected MLContext MlContext;

        protected BaseTrainer(string modelName)
        {
            _modelName = modelName;
            MlContext = new MLContext(0);
            ModelRelativePath = $"{BaseModelsRelativePath}\\{modelName}.zip";
            TestDataRelativePath = $"{BaseDatasetsRelativePath}\\test.tsv";
            TrainDataRelativePath = $"{BaseDatasetsRelativePath}\\train.tsv";
            ModelPath = GetAbsolutePath(ModelRelativePath);
            TestDataPath = GetAbsolutePath(TestDataRelativePath);
            TrainDataPath = GetAbsolutePath(TrainDataRelativePath);
        }

        private string AppPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

        public void Train(string[] columns)
        {
            //1.
            BuildTrainEvaluateAndSaveModel(MlContext, columns);
        }

        private void BuildTrainEvaluateAndSaveModel(MLContext mlContext, string[] inputColumnNames)
        {
            var featuresColumnName = "Features";
            var drumTypeColumnName = nameof(DrumTypeData.Type);
            var keyColumn = "KeyColumn";

            // STEP 1: Common data loading configuration
            var trainingDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TrainDataPath, hasHeader: true);
            var testDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TestDataPath, hasHeader: true);

            // STEP 2: Common data process configuration with pipeline data transformations
            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "KeyColumn", inputColumnName: nameof(DrumTypeData.Type))
                .Append(mlContext.Transforms.Concatenate("Features", inputColumnNames)
                                                                       .AppendCacheCheckpoint(mlContext));
            // Use in-memory cache for small/medium datasets to lower training time. 
            // Do NOT use it (remove .AppendCacheCheckpoint()) when handling very large datasets. 

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