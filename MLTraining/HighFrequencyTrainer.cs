using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class HighFrequencyTrainer : BaseTrainer
    {
        public HighFrequencyTrainer() : base("HighFrequencyDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.f9000Avg),nameof(DrumTypeData.f9000L1),nameof(DrumTypeData.f10000),nameof(DrumTypeData.f10000Mean),nameof(DrumTypeData.f10000Avg),nameof(DrumTypeData.f10000L1),nameof(DrumTypeData.f10500),nameof(DrumTypeData.f10500Mean),nameof(DrumTypeData.f10500Avg),nameof(DrumTypeData.f10500L1),nameof(DrumTypeData.f11000),nameof(DrumTypeData.f11000Mean),nameof(DrumTypeData.f11000Avg),nameof(DrumTypeData.f11000L1),nameof(DrumTypeData.f12000),nameof(DrumTypeData.f12000Mean),nameof(DrumTypeData.f12000Avg),nameof(DrumTypeData.f12000L1),nameof(DrumTypeData.f13000),nameof(DrumTypeData.f13000Mean),nameof(DrumTypeData.f13000Avg),nameof(DrumTypeData.f13000L1),nameof(DrumTypeData.f13500),nameof(DrumTypeData.f13500Mean),nameof(DrumTypeData.f13500Avg),nameof(DrumTypeData.f13500L1),nameof(DrumTypeData.f14000),nameof(DrumTypeData.f14000Mean),nameof(DrumTypeData.f14000Avg),nameof(DrumTypeData.f14000L1),nameof(DrumTypeData.f15000),nameof(DrumTypeData.f15000Mean),nameof(DrumTypeData.f15000Avg),nameof(DrumTypeData.f15000L1),nameof(DrumTypeData.f16000),nameof(DrumTypeData.f16000Mean),nameof(DrumTypeData.f16000Avg),nameof(DrumTypeData.f16000L1),nameof(DrumTypeData.f17000),nameof(DrumTypeData.f17000Mean),nameof(DrumTypeData.f17000Avg),nameof(DrumTypeData.f17000L1),nameof(DrumTypeData.f18000),nameof(DrumTypeData.f18000Mean),nameof(DrumTypeData.f18000Avg),nameof(DrumTypeData.f18000L1),nameof(DrumTypeData.f19000),nameof(DrumTypeData.f19000Mean),nameof(DrumTypeData.f19000Avg),nameof(DrumTypeData.f19000L1),nameof(DrumTypeData.f20000),nameof(DrumTypeData.f20000Mean),nameof(DrumTypeData.f20000Avg),nameof(DrumTypeData.f20000L1),
            });
        }
    }
}
