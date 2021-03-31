using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining.Trainers
{
    public class FloorHighFrequencyTrainer : BaseTrainer
    {
        public FloorHighFrequencyTrainer() : base("FHighFreqDrumTypeClassificationModel") {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency3400),
                nameof(DrumTypeData.Frequency4000),
                nameof(DrumTypeData.Frequency4800),
                nameof(DrumTypeData.Frequency5800),
            });
        }
    }
}
