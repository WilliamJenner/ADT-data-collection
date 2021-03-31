using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class CeilingLowFrequencyTrainer : BaseTrainer
    {
        public CeilingLowFrequencyTrainer() : base("CLowFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency450),
                nameof(DrumTypeData.Frequency570),
                nameof(DrumTypeData.Frequency700),
                nameof(DrumTypeData.Frequency840),
            });
        }
    }
}
