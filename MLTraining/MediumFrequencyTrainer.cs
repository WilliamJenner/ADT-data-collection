using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class MediumFrequencyTrainer : BaseTrainer
    {
        public MediumFrequencyTrainer(string modelName, string baseDatasetsRelativePath, string baseModelsRelativePath) : base(modelName, baseDatasetsRelativePath, baseModelsRelativePath)
        {
           
        }

        public MulticlassClassificationMetrics TrainModel()
        {
            return Train(new string[]
            {
                nameof(DrumTypeData.f2000Mean), nameof(DrumTypeData.f2150), nameof(DrumTypeData.f2150Avg),
                nameof(DrumTypeData.f2150L1Norm), nameof(DrumTypeData.f2150Mean), nameof(DrumTypeData.f2500),
                nameof(DrumTypeData.f2500Avg), nameof(DrumTypeData.f2500L1Norm), nameof(DrumTypeData.f2500Mean),
                nameof(DrumTypeData.f2900), nameof(DrumTypeData.f2900Avg), nameof(DrumTypeData.f2900L1Norm),
                nameof(DrumTypeData.f2900Mean), nameof(DrumTypeData.f3000), nameof(DrumTypeData.f3000Avg),
                nameof(DrumTypeData.f3000L1Norm), nameof(DrumTypeData.f3000Mean), nameof(DrumTypeData.f3400),
                nameof(DrumTypeData.f3400Avg), nameof(DrumTypeData.f3400L1Norm), nameof(DrumTypeData.f3400Mean),
                nameof(DrumTypeData.f4000), nameof(DrumTypeData.f4000Avg), nameof(DrumTypeData.f4000L1Norm),
                nameof(DrumTypeData.f4000Mean), nameof(DrumTypeData.f4800), nameof(DrumTypeData.f4800Avg),
                nameof(DrumTypeData.f4800L1Norm), nameof(DrumTypeData.f4800Mean), nameof(DrumTypeData.f5000),
                nameof(DrumTypeData.f5000Avg), nameof(DrumTypeData.f5000L1Norm), nameof(DrumTypeData.f5000Mean),
                nameof(DrumTypeData.f5800), nameof(DrumTypeData.f5800Avg), nameof(DrumTypeData.f5800L1Norm),
                nameof(DrumTypeData.f5800Mean), nameof(DrumTypeData.f6000), nameof(DrumTypeData.f6000Avg),
                nameof(DrumTypeData.f6000L1Norm), nameof(DrumTypeData.f6000Mean), nameof(DrumTypeData.f7000),
                nameof(DrumTypeData.f7000Avg), nameof(DrumTypeData.f7000L1Norm), nameof(DrumTypeData.f7000Mean),
                nameof(DrumTypeData.f8000), nameof(DrumTypeData.f8000Avg), nameof(DrumTypeData.f8000L1Norm),
                nameof(DrumTypeData.f8000Mean), nameof(DrumTypeData.f8500), nameof(DrumTypeData.f8500Avg),
                nameof(DrumTypeData.f8500L1Norm), nameof(DrumTypeData.f8500Mean), nameof(DrumTypeData.f9000),
                nameof(DrumTypeData.f9000Avg),
            });
        }
    }
}
