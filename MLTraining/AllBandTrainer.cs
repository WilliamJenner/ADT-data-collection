using Microsoft.ML.Data;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class AllBandTrainer : BaseTrainer
    {
        public AllBandTrainer(string modelName, string baseDatasetsRelativePath, string baseModelsRelativePath) : base(modelName, baseDatasetsRelativePath, baseModelsRelativePath)
        {
            
        }

        public MulticlassClassificationMetrics TrainModel()
        {
            return Train(new string[]
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
                nameof(DrumTypeData.f2000L1Norm), nameof(DrumTypeData.f2000Mean), nameof(DrumTypeData.f2150),
                nameof(DrumTypeData.f2150Avg), nameof(DrumTypeData.f2150L1Norm), nameof(DrumTypeData.f2150Mean),
                nameof(DrumTypeData.f2500), nameof(DrumTypeData.f2500Avg), nameof(DrumTypeData.f2500L1Norm),
                nameof(DrumTypeData.f2500Mean), nameof(DrumTypeData.f2900), nameof(DrumTypeData.f2900Avg),
                nameof(DrumTypeData.f2900L1Norm), nameof(DrumTypeData.f2900Mean), nameof(DrumTypeData.f3000),
                nameof(DrumTypeData.f3000Avg), nameof(DrumTypeData.f3000L1Norm), nameof(DrumTypeData.f3000Mean),
                nameof(DrumTypeData.f3400), nameof(DrumTypeData.f3400Avg), nameof(DrumTypeData.f3400L1Norm),
                nameof(DrumTypeData.f3400Mean), nameof(DrumTypeData.f4000), nameof(DrumTypeData.f4000Avg),
                nameof(DrumTypeData.f4000L1Norm), nameof(DrumTypeData.f4000Mean), nameof(DrumTypeData.f4800),
                nameof(DrumTypeData.f4800Avg), nameof(DrumTypeData.f4800L1Norm), nameof(DrumTypeData.f4800Mean),
                nameof(DrumTypeData.f5000), nameof(DrumTypeData.f5000Avg), nameof(DrumTypeData.f5000L1Norm),
                nameof(DrumTypeData.f5000Mean), nameof(DrumTypeData.f5800), nameof(DrumTypeData.f5800Avg),
                nameof(DrumTypeData.f5800L1Norm), nameof(DrumTypeData.f5800Mean), nameof(DrumTypeData.f6000),
                nameof(DrumTypeData.f6000Avg), nameof(DrumTypeData.f6000L1Norm), nameof(DrumTypeData.f6000Mean),
                nameof(DrumTypeData.f7000), nameof(DrumTypeData.f7000Avg), nameof(DrumTypeData.f7000L1Norm),
                nameof(DrumTypeData.f7000Mean), nameof(DrumTypeData.f8000), nameof(DrumTypeData.f8000Avg),
                nameof(DrumTypeData.f8000L1Norm), nameof(DrumTypeData.f8000Mean), nameof(DrumTypeData.f8500),
                nameof(DrumTypeData.f8500Avg), nameof(DrumTypeData.f8500L1Norm), nameof(DrumTypeData.f8500Mean),
                nameof(DrumTypeData.f9000), nameof(DrumTypeData.f9000Avg), nameof(DrumTypeData.f9000L1Norm),
                nameof(DrumTypeData.f9000Mean), nameof(DrumTypeData.f10000), nameof(DrumTypeData.f10000Avg),
                nameof(DrumTypeData.f10000L1Norm), nameof(DrumTypeData.f10000Mean), nameof(DrumTypeData.f10500),
                nameof(DrumTypeData.f10500Avg), nameof(DrumTypeData.f10500L1Norm), nameof(DrumTypeData.f10500Mean),
                nameof(DrumTypeData.f11000), nameof(DrumTypeData.f11000Avg), nameof(DrumTypeData.f11000L1Norm),
                nameof(DrumTypeData.f11000Mean), nameof(DrumTypeData.f12000), nameof(DrumTypeData.f12000Avg),
                nameof(DrumTypeData.f12000L1Norm), nameof(DrumTypeData.f12000Mean), nameof(DrumTypeData.f13000),
                nameof(DrumTypeData.f13000Avg), nameof(DrumTypeData.f13000L1Norm), nameof(DrumTypeData.f13000Mean),
                nameof(DrumTypeData.f13500), nameof(DrumTypeData.f13500Avg), nameof(DrumTypeData.f13500L1Norm),
                nameof(DrumTypeData.f13500Mean), nameof(DrumTypeData.f14000), nameof(DrumTypeData.f14000Avg),
                nameof(DrumTypeData.f14000L1Norm), nameof(DrumTypeData.f14000Mean), nameof(DrumTypeData.f15000),
                nameof(DrumTypeData.f15000Avg), nameof(DrumTypeData.f15000L1Norm), nameof(DrumTypeData.f15000Mean),
                nameof(DrumTypeData.f16000), nameof(DrumTypeData.f16000Avg), nameof(DrumTypeData.f16000L1Norm),
                nameof(DrumTypeData.f16000Mean), nameof(DrumTypeData.f17000), nameof(DrumTypeData.f17000Avg),
                nameof(DrumTypeData.f17000L1Norm), nameof(DrumTypeData.f17000Mean), nameof(DrumTypeData.f18000),
                nameof(DrumTypeData.f18000Avg), nameof(DrumTypeData.f18000L1Norm), nameof(DrumTypeData.f18000Mean),
                nameof(DrumTypeData.f19000), nameof(DrumTypeData.f19000Avg), nameof(DrumTypeData.f19000L1Norm),
                nameof(DrumTypeData.f19000Mean), nameof(DrumTypeData.f20000), nameof(DrumTypeData.f20000Avg),
                nameof(DrumTypeData.f20000L1Norm), nameof(DrumTypeData.f20000Mean),
            });
        }
    }
}