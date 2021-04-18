using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class LowFrequencyTrainer : BaseTrainer
    {
        public LowFrequencyTrainer() : base("LowFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency10    ),
                nameof(DrumTypeData.Frequency20    ),
                nameof(DrumTypeData.Frequency250   ),
                nameof(DrumTypeData.Frequency350   ),
                nameof(DrumTypeData.Frequency450   ),
                nameof(DrumTypeData.Frequency570   ),
                nameof(DrumTypeData.Frequency700   ),
                nameof(DrumTypeData.Frequency840   ),
                nameof(DrumTypeData.Frequency1000  ),
                nameof(DrumTypeData.Frequency1170  ),
                nameof(DrumTypeData.Frequency1370  ),
                nameof(DrumTypeData.Frequency1600  ),
                nameof(DrumTypeData.Frequency1850  ),
            });
        }
    }
}
