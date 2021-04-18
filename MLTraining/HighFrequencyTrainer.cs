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
                nameof(DrumTypeData.Frequency9000   ),
                nameof(DrumTypeData.Frequency10000  ),
                nameof(DrumTypeData.Frequency10500  ),
                nameof(DrumTypeData.Frequency11000  ),
                nameof(DrumTypeData.Frequency12000  ),
                nameof(DrumTypeData.Frequency13000  ),
                nameof(DrumTypeData.Frequency13500  ),
                nameof(DrumTypeData.Frequency14000  ),
                nameof(DrumTypeData.Frequency15000  ),
                nameof(DrumTypeData.Frequency16000  ),
                nameof(DrumTypeData.Frequency17000  ),
                nameof(DrumTypeData.Frequency18000  ),
                nameof(DrumTypeData.Frequency19000  ),
                nameof(DrumTypeData.Frequency20000  ),
            });
        }
    }
}
