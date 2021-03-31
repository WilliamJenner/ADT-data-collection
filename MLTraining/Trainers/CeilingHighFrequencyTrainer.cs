using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining.Trainers
{
    public class CeilingHighFrequencyTrainer : BaseTrainer
    {
        public CeilingHighFrequencyTrainer() : base("CHighFreqDrumTypeClassificationModel") {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency7000),
                nameof(DrumTypeData.Frequency8500),
                nameof(DrumTypeData.Frequency10500),
                nameof(DrumTypeData.Frequency13500)
            });
        }
    }
}
