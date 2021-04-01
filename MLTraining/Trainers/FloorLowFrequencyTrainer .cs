using MLTraining.DataStructures;

namespace MLTraining.Trainers
{
    public class FloorLowFrequencyTrainer : BaseTrainer
    {
        public FloorLowFrequencyTrainer() : base("FLowFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency50),
                nameof(DrumTypeData.Frequency150),
                nameof(DrumTypeData.Frequency150),
                nameof(DrumTypeData.Frequency250),
                nameof(DrumTypeData.Frequency350)
            });
        }
    }
}
