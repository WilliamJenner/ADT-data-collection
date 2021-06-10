using System.Collections.Generic;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class FrequencyMultiClassificationModelFactory
    {
        private static string LowFrequencyModel => "LowFrequencyDrumTypeClassificationModel";
        private static string MediumFrequencyModel => "MediumFrequencyDrumTypeClassificationModel";
        private static string HighFrequencyModel => "HighFrequencyDrumTypeClassificationModel";
        private static string AllFrequencyModel => "AllBandDrumTypeClassificationModel";

        public static IEnumerable<ModelMetrics> Train(string baseDatasetsRelativePath, string baseModelsRelativePath)
        {
            var lft = new LowFrequencyTrainer(LowFrequencyModel, baseDatasetsRelativePath, baseModelsRelativePath);
            var mft = new MediumFrequencyTrainer(MediumFrequencyModel, baseDatasetsRelativePath,
                baseModelsRelativePath);
            var hft = new HighFrequencyTrainer(HighFrequencyModel, baseDatasetsRelativePath, baseModelsRelativePath);
            var abt = new AllBandTrainer(AllFrequencyModel, baseDatasetsRelativePath, baseModelsRelativePath);

            var metrics = new List<ModelMetrics>
            {
                new ModelMetrics(lft.TrainModel(), LowFrequencyModel),
                new ModelMetrics(mft.TrainModel(), MediumFrequencyModel),
                new ModelMetrics(hft.TrainModel(), HighFrequencyModel),
                new ModelMetrics(abt.TrainModel(), AllFrequencyModel)
            };

            return metrics;
        }
    }
}