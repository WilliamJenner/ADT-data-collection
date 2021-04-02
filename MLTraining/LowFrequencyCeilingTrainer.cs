using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using MLTraining.DataStructures;

namespace MLTraining
{
    public class LowFrequencyCeilingTrainer : BaseTrainer
    {
        public LowFrequencyCeilingTrainer() : base("LowFreqCeilingDrumTypeClassificationModel")
        {
            Train(new string[]
            {

                nameof(DrumTypeData.Frequency800   ),
                nameof(DrumTypeData.Frequency850   ),
                nameof(DrumTypeData.Frequency900   ),
                nameof(DrumTypeData.Frequency950   ),
                nameof(DrumTypeData.Frequency1000  ),
                nameof(DrumTypeData.Frequency1050  ),
                nameof(DrumTypeData.Frequency1100  ),
                nameof(DrumTypeData.Frequency1150  ),
                nameof(DrumTypeData.Frequency1200  ),
                nameof(DrumTypeData.Frequency1250  ),
                nameof(DrumTypeData.Frequency1300  )
            });
        }
    }
}
