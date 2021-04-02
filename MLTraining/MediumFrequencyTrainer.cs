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
                nameof(DrumTypeData.Frequency1350  ),
                nameof(DrumTypeData.Frequency1400  ),
                nameof(DrumTypeData.Frequency1450  ),
                nameof(DrumTypeData.Frequency1500  ),
                nameof(DrumTypeData.Frequency1550  ),
                nameof(DrumTypeData.Frequency1600  ),
                nameof(DrumTypeData.Frequency1650  ),
                nameof(DrumTypeData.Frequency1700  ),
                nameof(DrumTypeData.Frequency1750  ),
                nameof(DrumTypeData.Frequency1800  ),
                nameof(DrumTypeData.Frequency1850  ),
                nameof(DrumTypeData.Frequency1900  ),
                nameof(DrumTypeData.Frequency1950  ),
                nameof(DrumTypeData.Frequency2000  ),
                nameof(DrumTypeData.Frequency2050  ),
                nameof(DrumTypeData.Frequency2100  ),
                nameof(DrumTypeData.Frequency2150  ),
                nameof(DrumTypeData.Frequency2200  ),
                nameof(DrumTypeData.Frequency2250  ),
                nameof(DrumTypeData.Frequency2300  ),
                nameof(DrumTypeData.Frequency2350  ),
                nameof(DrumTypeData.Frequency2400  ),
                nameof(DrumTypeData.Frequency2450  ),
                nameof(DrumTypeData.Frequency2500  ),
                nameof(DrumTypeData.Frequency2550  ),
                nameof(DrumTypeData.Frequency2600  )
            });
        }
    }
}
