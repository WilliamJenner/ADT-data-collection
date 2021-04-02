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
                nameof(DrumTypeData.Frequency50    ),
                nameof(DrumTypeData.Frequency100   ),
                nameof(DrumTypeData.Frequency150   ),
                nameof(DrumTypeData.Frequency200   ),
                nameof(DrumTypeData.Frequency250   ),
                nameof(DrumTypeData.Frequency300   ),
                nameof(DrumTypeData.Frequency350   ),
                nameof(DrumTypeData.Frequency400   ),
                nameof(DrumTypeData.Frequency450   ),
                nameof(DrumTypeData.Frequency500   ),
                nameof(DrumTypeData.Frequency550   ),
                nameof(DrumTypeData.Frequency600   ),
                nameof(DrumTypeData.Frequency650   ),
                nameof(DrumTypeData.Frequency700   ),
                nameof(DrumTypeData.Frequency750   ),
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
