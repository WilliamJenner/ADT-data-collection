using MLTraining.DataStructures;

namespace MLTraining
{
    public class AllBandTrainer : BaseTrainer
    {
        public AllBandTrainer() : base("AllBandDrumTypeClassificationModel")
        {
            Train(new[]
            {
                nameof(DrumTypeData.Frequency10      ),
                nameof(DrumTypeData.Frequency20      ),
                nameof(DrumTypeData.Frequency250     ),
                nameof(DrumTypeData.Frequency350     ),
                nameof(DrumTypeData.Frequency450     ),
                nameof(DrumTypeData.Frequency570     ),
                nameof(DrumTypeData.Frequency700     ),
                nameof(DrumTypeData.Frequency840     ),
                nameof(DrumTypeData.Frequency1000    ),
                nameof(DrumTypeData.Frequency1170    ),
                nameof(DrumTypeData.Frequency1370    ),
                nameof(DrumTypeData.Frequency1600    ),
                nameof(DrumTypeData.Frequency1850    ),
                nameof(DrumTypeData.Frequency2000    ),
                nameof(DrumTypeData.Frequency2150    ),
                nameof(DrumTypeData.Frequency2500    ),
                nameof(DrumTypeData.Frequency2900    ),
                nameof(DrumTypeData.Frequency3000    ),
                nameof(DrumTypeData.Frequency3400    ),
                nameof(DrumTypeData.Frequency4000    ),
                nameof(DrumTypeData.Frequency4800    ),
                nameof(DrumTypeData.Frequency5000    ),
                nameof(DrumTypeData.Frequency5800    ),
                nameof(DrumTypeData.Frequency6000    ),
                nameof(DrumTypeData.Frequency7000    ),
                nameof(DrumTypeData.Frequency8000    ),
                nameof(DrumTypeData.Frequency8500    ),
                nameof(DrumTypeData.Frequency9000    ),
                nameof(DrumTypeData.Frequency10000   ),
                nameof(DrumTypeData.Frequency10500   ),
                nameof(DrumTypeData.Frequency11000   ),
                nameof(DrumTypeData.Frequency12000   ),
                nameof(DrumTypeData.Frequency13000   ),
                nameof(DrumTypeData.Frequency13500   ),
                nameof(DrumTypeData.Frequency14000   ),
                nameof(DrumTypeData.Frequency15000   ),
                nameof(DrumTypeData.Frequency16000   ),
                nameof(DrumTypeData.Frequency17000   ),
                nameof(DrumTypeData.Frequency18000   ),
                nameof(DrumTypeData.Frequency19000   ),
                nameof(DrumTypeData.Frequency20000   )
            });
        }
    }
}