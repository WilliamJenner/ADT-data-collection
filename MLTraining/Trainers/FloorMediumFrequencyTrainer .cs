using MLTraining.DataStructures;

namespace MLTraining.Trainers
{
    public class FloorMediumFrequencyTrainer : BaseTrainer
    {
        public FloorMediumFrequencyTrainer() : base("FMedFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency1000),
                nameof(DrumTypeData.Frequency1170),
                nameof(DrumTypeData.Frequency1370),
                nameof(DrumTypeData.Frequency1600),
            });
        }
    }
}
