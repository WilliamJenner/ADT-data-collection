using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class HighFrequencyTrainer : BaseTrainer
    {
        public HighFrequencyTrainer() : base("HighFreqDrumTypeClassificationModel") {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency3400),
                nameof(DrumTypeData.Frequency4000),
                nameof(DrumTypeData.Frequency4800),
                nameof(DrumTypeData.Frequency5800),
                nameof(DrumTypeData.Frequency7000),
                nameof(DrumTypeData.Frequency8500),
                nameof(DrumTypeData.Frequency10500),
                nameof(DrumTypeData.Frequency13500)
            });
        }
    }
}
