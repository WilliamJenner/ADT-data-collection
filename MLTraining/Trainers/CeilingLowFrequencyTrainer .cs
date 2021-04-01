using MLTraining.DataStructures;

namespace MLTraining.Trainers
{
    public class CeilingLowFrequencyTrainer : BaseTrainer
    {
        public CeilingLowFrequencyTrainer() : base("CLowFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency450),
                nameof(DrumTypeData.Frequency570),
                nameof(DrumTypeData.Frequency700),
                nameof(DrumTypeData.Frequency840),
            });
        }
    }
}
