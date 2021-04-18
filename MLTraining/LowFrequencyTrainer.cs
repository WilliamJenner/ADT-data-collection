using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class LowFrequencyTrainer : BaseTrainer
    {
        public LowFrequencyTrainer() : base("LowFrequencyDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.f10),nameof(DrumTypeData.f10Mean),nameof(DrumTypeData.f10Avg),nameof(DrumTypeData.f20),nameof(DrumTypeData.f20Mean),nameof(DrumTypeData.f20Avg),nameof(DrumTypeData.f250),nameof(DrumTypeData.f250Mean),nameof(DrumTypeData.f250Avg),nameof(DrumTypeData.f350),nameof(DrumTypeData.f350Mean),nameof(DrumTypeData.f350Avg),nameof(DrumTypeData.f450),nameof(DrumTypeData.f450Mean),nameof(DrumTypeData.f450Avg),nameof(DrumTypeData.f570),nameof(DrumTypeData.f570Mean),nameof(DrumTypeData.f570Avg),nameof(DrumTypeData.f700),nameof(DrumTypeData.f700Mean),nameof(DrumTypeData.f700Avg),nameof(DrumTypeData.f840),nameof(DrumTypeData.f840Mean),nameof(DrumTypeData.f840Avg),nameof(DrumTypeData.f1000),nameof(DrumTypeData.f1000Mean),nameof(DrumTypeData.f1000Avg),nameof(DrumTypeData.f1170),nameof(DrumTypeData.f1170Mean),nameof(DrumTypeData.f1170Avg),nameof(DrumTypeData.f1370),nameof(DrumTypeData.f1370Mean),nameof(DrumTypeData.f1370Avg),nameof(DrumTypeData.f1600),nameof(DrumTypeData.f1600Mean),nameof(DrumTypeData.f1600Avg),nameof(DrumTypeData.f1850),nameof(DrumTypeData.f1850Mean),nameof(DrumTypeData.f1850Avg),nameof(DrumTypeData.f2000),nameof(DrumTypeData.f2000Mean),
            });
        }
    }
}
