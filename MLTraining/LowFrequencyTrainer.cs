using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class LowFrequencyTrainer : BaseTrainer
    {
        public LowFrequencyTrainer(string modelName, string baseDatasetsRelativePath, string baseModelsRelativePath) : base(modelName, baseDatasetsRelativePath, baseModelsRelativePath)
        {
            
        }

        public MulticlassClassificationMetrics TrainModel()
        {
            return base.Train(new string[]
            {
                nameof(DrumTypeData.f50), nameof(DrumTypeData.f50Avg), nameof(DrumTypeData.f50L1Norm),
                nameof(DrumTypeData.f50Mean), nameof(DrumTypeData.f150), nameof(DrumTypeData.f150Avg),
                nameof(DrumTypeData.f150L1Norm), nameof(DrumTypeData.f150Mean), nameof(DrumTypeData.f250),
                nameof(DrumTypeData.f250Avg), nameof(DrumTypeData.f250L1Norm), nameof(DrumTypeData.f250Mean),
                nameof(DrumTypeData.f350), nameof(DrumTypeData.f350Avg), nameof(DrumTypeData.f350L1Norm),
                nameof(DrumTypeData.f350Mean), nameof(DrumTypeData.f450), nameof(DrumTypeData.f450Avg),
                nameof(DrumTypeData.f450L1Norm), nameof(DrumTypeData.f450Mean), nameof(DrumTypeData.f570),
                nameof(DrumTypeData.f570Avg), nameof(DrumTypeData.f570L1Norm), nameof(DrumTypeData.f570Mean),
                nameof(DrumTypeData.f700), nameof(DrumTypeData.f700Avg), nameof(DrumTypeData.f700L1Norm),
                nameof(DrumTypeData.f700Mean), nameof(DrumTypeData.f840), nameof(DrumTypeData.f840Avg),
                nameof(DrumTypeData.f840L1Norm), nameof(DrumTypeData.f840Mean), nameof(DrumTypeData.f1000),
                nameof(DrumTypeData.f1000Avg), nameof(DrumTypeData.f1000L1Norm), nameof(DrumTypeData.f1000Mean),
                nameof(DrumTypeData.f1170), nameof(DrumTypeData.f1170Avg), nameof(DrumTypeData.f1170L1Norm),
                nameof(DrumTypeData.f1170Mean), nameof(DrumTypeData.f1370), nameof(DrumTypeData.f1370Avg),
                nameof(DrumTypeData.f1370L1Norm), nameof(DrumTypeData.f1370Mean), nameof(DrumTypeData.f1600),
                nameof(DrumTypeData.f1600Avg), nameof(DrumTypeData.f1600L1Norm), nameof(DrumTypeData.f1600Mean),
                nameof(DrumTypeData.f1850), nameof(DrumTypeData.f1850Avg), nameof(DrumTypeData.f1850L1Norm),
                nameof(DrumTypeData.f1850Mean), nameof(DrumTypeData.f2000), nameof(DrumTypeData.f2000Avg),
                nameof(DrumTypeData.f2000L1Norm),
            });
        }
    }
}
