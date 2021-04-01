//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using Microsoft.ML;
//using Microsoft.ML.Data;
//using Microsoft.ML.Trainers;
//using MLTraining.DataStructures;

//namespace MLTraining
//{
//    public abstract class BaseTrainer
//    {
//        private readonly string _modelName;
//        //private readonly string BaseDatasetsRelativePath = "C:\\source\\ADT\\ConsoleApp1\\sound\\experiment";
//        private readonly string BaseDatasetsRelativePath = "C:\\source\\ADT\\MLTraining\\data";
//        private readonly string TrainDataRelativePath;
//        private readonly string TestDataRelativePath;

//        private readonly string TrainDataPath;
//        private readonly string TestDataPath;

//        protected readonly string BaseModelsRelativePath = "C:\\source\\ADT\\MLTraining\\models";
//        protected readonly string ModelRelativePath;

//        private readonly string ModelPath;
//        private string AppPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

//        protected MLContext MlContext;

//        protected BaseTrainer(string modelName)
//        {
//            _modelName = modelName;
//            MlContext = new MLContext(0);
//            ModelRelativePath = $"{BaseModelsRelativePath}\\{modelName}.zip";
//            TestDataRelativePath = $"{BaseDatasetsRelativePath}\\test.tsv";
//            TrainDataRelativePath = $"{BaseDatasetsRelativePath}\\train.tsv";
//            ModelPath = GetAbsolutePath(ModelRelativePath);
//            TestDataPath = GetAbsolutePath(TestDataRelativePath);
//            TrainDataPath = GetAbsolutePath(TrainDataRelativePath);
//        }

//        public void Train(string[] columns)
//        {
//            //1.
//            BuildTrainEvaluateAndSaveModel(MlContext, columns);
//        }

//        private void BuildTrainEvaluateAndSaveModel(MLContext mlContext, string[] inputColumnNames)
//        {
//            string featuresColumnName = "Features";
//            string keyColumn = "KeyColumn";

//            // STEP 1: Common data loading configuration
//            var trainingDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TrainDataPath, hasHeader: true);
//            var testDataView = mlContext.Data.LoadFromTextFile<DrumTypeData>(TestDataPath, hasHeader: true);

//            //var options = new KMeansTrainer.Options
//            //{
//            //    FeatureColumnName = featuresColumnName,
//            //    NumberOfClusters = 5,
//            //    OptimizationTolerance = 1e-30f
//            //};

//            //var pipeline = mlContext.Transforms
//            //    .Concatenate(featuresColumnName, inputColumnNames)
//            //    .Append(mlContext.Clustering.Trainers.KMeans(options));

//            //var model = pipeline.Fit(trainingDataView);
//            //var transformedTestData = model.Transform(testDataView);

//            //// Convert IDataView object to a list.
//            //var predictions = mlContext.Data.CreateEnumerable<Prediction>(
//            //    transformedTestData, reuseRowObject: false).ToList();

//            //// Print 5 predictions. Note that the label is only used as a comparison
//            //// with the predicted label. It is not used during training.
//            //PrintPredictionMetrics(predictions);

//            //// Expected output:
//            ////   Label: 1, Prediction: 1
//            ////   Label: 1, Prediction: 1
//            ////   Label: 2, Prediction: 2
//            ////   Label: 2, Prediction: 2
//            ////   Label: 2, Prediction: 2

//            //// Evaluate the overall metrics
//            //var metrics = mlContext.Clustering.Evaluate(
//            //    transformedTestData, "Type", "Score", "Features");

//            //PrintMetrics(metrics);

//            //var predictor = mlContext.Model.CreatePredictionEngine<DrumTypeData, DrumTypePrediction>(model);

//            //var prediction = predictor.Predict(SampleDrumTypeData.Kick);
//            //Console.WriteLine($"Kick");
//            //Console.WriteLine($"Cluster: {prediction.PredictedClusterId}");
//            //Console.WriteLine($"Distances: {string.Join(" ", prediction.Score)}");

//            //Console.WriteLine($"Snare");
//            //prediction = predictor.Predict(SampleDrumTypeData.Snare);
//            //Console.WriteLine($"Cluster: {prediction.PredictedClusterId}");
//            //Console.WriteLine($"Distances: {string.Join(" ", prediction.Score)}");

//            //Console.WriteLine($"Cymbal");
//            //prediction = predictor.Predict(SampleDrumTypeData.Cymbal);
//            //Console.WriteLine($"Cluster: {prediction.PredictedClusterId}");
//            //Console.WriteLine($"Distances: {string.Join(" ", prediction.Score)}");

//            //Console.WriteLine($"LowTom");
//            //prediction = predictor.Predict(SampleDrumTypeData.LowTom);
//            //Console.WriteLine($"Cluster: {prediction.PredictedClusterId}");
//            //Console.WriteLine($"Distances: {string.Join(" ", prediction.Score)}");

//            // STEP 2: Common data process configuration with pipeline data transformations
//            //var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey(featuresColumnName, inputColumnName: nameof(DrumTypeData.Type))
//            //    .Append(mlContext.Transforms.Concatenate(featuresColumnName, inputColumnNames)
//            //        .AppendCacheCheckpoint(mlContext));

//            var dataProcessPipeline = mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: featuresColumnName,
//                    inputColumnName: nameof(DrumTypeData.Type))
//                .Append(mlContext.Transforms.Concatenate(featuresColumnName, inputColumnNames));

//            // Use in-memory cache for small/medium datasets to lower training time. 
//            // Do NOT use it (remove .AppendCacheCheckpoint()) when handling very large datasets. 


//            //IEstimator<ITransformer> dataPrepEstimator =
//            //    mlContext.Transforms.Concatenate("Features", inputColumnNames)
//            //        .Append(mlContext.Transforms.NormalizeMinMax("Features"));

//            //// Create data prep transformer
//            //ITransformer dataPrepTransformer = dataPrepEstimator.Fit(trainingDataView);
//            //IDataView transformedData = dataPrepTransformer.Transform(trainingDataView);

//            //IEstimator<ITransformer> sdcaEstimator = mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
//            //    labelColumnName: "KeyColumn",
//            //    featureColumnName: "Features" /*, historySize: 1000, optimizationTolerance: 1e-20f, 
//            //        enforceNonNegativity:true,  l2Regularization: 0.5f*/)
//            //    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: nameof(DrumTypeData.Type), inputColumnName: "KeyColumn"));
//            //var cvResults = mlContext.Regression.CrossValidate(transformedData, sdcaEstimator, numberOfFolds: 5);


//            //STEP 3: Set the training algorithm, then append the trainer to the pipeline
//            var trainer = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: nameof(DrumTypeData.Type), featureColumnName: featuresColumnName, historySize: 100, optimizationTolerance: 1e-30f).Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: nameof(DrumTypeData.Type), inputColumnName: "KeyColumn"));


//            //, historySize: 1000, optimizationTolerance: 1e-20f, 
//            //enforceNonNegativity:true,  l2Regularization: 0.5f) 


//            var trainingPipeline = dataProcessPipeline.Append(trainer);

//            // STEP 4: Train the model fitting to the DataSet
//            Console.WriteLine("=============== Training the model ===============");
//            ITransformer trainedModel = trainingPipeline.Fit(trainingDataView);

//            // STEP 5: Evaluate the model and show accuracy stats
//            Console.WriteLine("===== Evaluating Model's accuracy with Test data =====");
//            var predictions = trainedModel.Transform(testDataView);

//            var metrics = mlContext.MulticlassClassification.Evaluate(predictions, "KeyColumn");

//            PrintMultiClassClassificationMetrics(_modelName, metrics);

//            // STEP 6: Save/persist the trained model to a .ZIP file
//            //mlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);
//            //Console.WriteLine("The model is saved to {0}", ModelPath);

//            //TestSomePredictions(MlContext);
//        }

//        // Pretty-print of ClusteringMetrics object.
//        private static void PrintMetrics(ClusteringMetrics metrics)
//        {
//            Console.WriteLine("Normalized Mutual Information: " +
//                              $"{metrics.NormalizedMutualInformation:F2}");

//            Console.WriteLine("Average Distance: " +
//                              $"{metrics.AverageDistance:F2}");

//            Console.WriteLine("Davies Bouldin Index: " +
//                              $"{metrics.DaviesBouldinIndex:F2}");
//        }

//        private void TestSomePredictions(MLContext mlContext)
//        {
//            //Test Classification Predictions with some hard-coded samples 
//            var trainedModel = mlContext.Model.Load(ModelPath, out var modelInputSchema);

//            // Create prediction engine related to the loaded trained model
//            var predEngine = mlContext.Model.CreatePredictionEngine<DrumTypeData, DrumTypePrediction>(trainedModel);

//            // During prediction we will get Score column with 3 float values.
//            // We need to find way to map each score to original label.
//            // In order to do that we need to get TrainingLabelValues from Score column.
//            // TrainingLabelValues on top of Score column represent original labels for i-th value in Score array.
//            // Let's look how we can convert key value for PredictedLabel to original labels.
//            // We need to read KeyValues for "PredictedLabel" column.
//            VBuffer<float> keys = default;
//            predEngine.OutputSchema["PredictedLabel"].GetKeyValues(ref keys);
//            var labelsArray = keys.DenseValues().ToArray();

//            //Add a dictionary to map the above float values to strings. 
//            var drumTypes = new Dictionary<float, string>();
           
//            Console.WriteLine("=====Predicting using model====");
//            //Score sample 4
//            var snarePrediction = predEngine.Predict(SampleDrumTypeData.Snare);
//            //Score sample 0
//            var kickPrediction = predEngine.Predict(SampleDrumTypeData.Kick);
//            //Score sample 3
//            var lowTomPrediction = predEngine.Predict(SampleDrumTypeData.LowTom);
//            // score sample 5
//            var cymbalPrediction = predEngine.Predict(SampleDrumTypeData.Cymbal);
//        }

//        public string GetAbsolutePath(string relativePath)
//        {
//            var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
//            var assemblyFolderPath = _dataRoot.Directory.FullName;

//            var fullPath = Path.Combine(assemblyFolderPath, relativePath);

//            return fullPath;
//        }

//        private void PrintPredictionMetrics(IEnumerable<Prediction> predictions)
//        {
//            List<Prediction> correct = new List<Prediction>();

//            foreach (var prediction in predictions)
//            {
//                if (Math.Abs(prediction.PredictedLabel - (double)prediction.Type) < 1e-6)
//                {
//                    correct.Add(prediction);
//                }
//            }

//           decimal correctCount = correct.Count();
//           decimal predCount = predictions.Count();
//           decimal correctCountDivPredCount = correctCount / predCount;
//            Console.WriteLine($"This was {correctCountDivPredCount * 100:F}% accurate");
//        }

//        public void PrintMultiClassClassificationMetrics(string name, MulticlassClassificationMetrics metrics)
//        {
//            Console.WriteLine("************************************************************");
//            Console.WriteLine($"*    Metrics for {name} multi-class classification model   ");
//            Console.WriteLine("*-----------------------------------------------------------");
//            Console.WriteLine(
//                $"    AccuracyMacro = {metrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
//            Console.WriteLine(
//                $"    AccuracyMicro = {metrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
//            Console.WriteLine($"    LogLoss = {metrics.LogLoss:0.####}, the closer to 0, the better");
//            Console.WriteLine($"    LogLossReduction = {metrics.LogLossReduction:0.####}, the closer to 0, the better");

//            for (var index = 0; index < metrics.PerClassLogLoss.Count; index++)
//            {
//                var logLoss = metrics.PerClassLogLoss[index];
//                Console.WriteLine(
//                    $"    LogLoss for class {index} = {logLoss:0.####}, the closer to 0, the better");
//            }

//            Console.WriteLine("************************************************************");
//        }

//        // Class used to capture predictions.
//        private class Prediction
//        {
//            // Original label (not used during training, just for comparison).
//            public uint Type { get; set; }
//            // Predicted label from the trainer.
//            public uint PredictedLabel { get; set; }
//        }
//    }
//}