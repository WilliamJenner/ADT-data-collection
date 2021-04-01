﻿using MLTraining.DataStructures;

namespace MLTraining.Trainers
{
    public class MediumFrequencyTrainer : BaseTrainer
    {
        public MediumFrequencyTrainer() : base("MedFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency1000),
                nameof(DrumTypeData.Frequency1170),
                nameof(DrumTypeData.Frequency1370),
                nameof(DrumTypeData.Frequency1600),
                nameof(DrumTypeData.Frequency1850),
                nameof(DrumTypeData.Frequency2150),
                nameof(DrumTypeData.Frequency2500),
                nameof(DrumTypeData.Frequency2900),
            });
        }
    }
}
