using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class FloorLowFrequencyTrainer : BaseTrainer
    {
        public FloorLowFrequencyTrainer() : base("FLowFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency50),
                nameof(DrumTypeData.Frequency150),
                nameof(DrumTypeData.Frequency150),
                nameof(DrumTypeData.Frequency250),
                nameof(DrumTypeData.Frequency350)
            });
        }
    }
}
