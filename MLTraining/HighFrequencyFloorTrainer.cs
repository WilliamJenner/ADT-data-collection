using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class HighFrequencyFloorTrainer : BaseTrainer
    {
        public HighFrequencyFloorTrainer() : base("HighFrequencyFloorDrumTypeClassificationModel") {
            Train(new string[]
            {
                
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
