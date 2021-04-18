using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class MediumFrequencyTrainer : BaseTrainer
    {
        public MediumFrequencyTrainer() : base("MedFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency2000  ),
                nameof(DrumTypeData.Frequency2150  ),
                nameof(DrumTypeData.Frequency2500  ),
                nameof(DrumTypeData.Frequency2900  ),
                nameof(DrumTypeData.Frequency3000  ),
                nameof(DrumTypeData.Frequency3400  ),
                nameof(DrumTypeData.Frequency4000  ),
                nameof(DrumTypeData.Frequency4800  ),
                nameof(DrumTypeData.Frequency5000  ),
                nameof(DrumTypeData.Frequency5800  ),
                nameof(DrumTypeData.Frequency6000  ),
                nameof(DrumTypeData.Frequency7000  ),
                nameof(DrumTypeData.Frequency8000  ),
                nameof(DrumTypeData.Frequency8500  ),
            });
        }
    }
}
