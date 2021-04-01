﻿using MLTraining.DataStructures;

namespace MLTraining.Trainers
{
    public class LowFrequencyTrainer : BaseTrainer
    {
        public LowFrequencyTrainer() : base("LowFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency50),
                nameof(DrumTypeData.Frequency150),
                nameof(DrumTypeData.Frequency150),
                nameof(DrumTypeData.Frequency250),
                nameof(DrumTypeData.Frequency350),
                nameof(DrumTypeData.Frequency450),
                nameof(DrumTypeData.Frequency570),
                nameof(DrumTypeData.Frequency700),
                nameof(DrumTypeData.Frequency840),
            });
        }
    }
}
