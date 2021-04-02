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
                nameof(DrumTypeData.Frequency2650  ),
                nameof(DrumTypeData.Frequency2700  ),
                nameof(DrumTypeData.Frequency2750  ),
                nameof(DrumTypeData.Frequency2800  ),
                nameof(DrumTypeData.Frequency2850  ),
                nameof(DrumTypeData.Frequency2900  ),
                nameof(DrumTypeData.Frequency2950  ),
                nameof(DrumTypeData.Frequency3000  ),
                nameof(DrumTypeData.Frequency3050  ),
                nameof(DrumTypeData.Frequency3100  ),
                nameof(DrumTypeData.Frequency3150  ),
                nameof(DrumTypeData.Frequency3200  ),
                nameof(DrumTypeData.Frequency3250  ),
                nameof(DrumTypeData.Frequency3300  ),
                nameof(DrumTypeData.Frequency3350  ),
                nameof(DrumTypeData.Frequency3400  ),
                nameof(DrumTypeData.Frequency3450  ),
                nameof(DrumTypeData.Frequency3500  ),
                nameof(DrumTypeData.Frequency3550  ),
                nameof(DrumTypeData.Frequency3600  ),
                nameof(DrumTypeData.Frequency3650  ),
                nameof(DrumTypeData.Frequency3700  ),
                nameof(DrumTypeData.Frequency3750  ),
                nameof(DrumTypeData.Frequency3800  ),
                nameof(DrumTypeData.Frequency3850  ),
                nameof(DrumTypeData.Frequency3900  ),
                nameof(DrumTypeData.Frequency3950  ),
                nameof(DrumTypeData.Frequency4000  )
            });
        }
    }
}
