using MLTraining.DataStructures;

namespace MLTraining.Trainers
{
    public class CeilingMediumFrequencyTrainer : BaseTrainer
    {
        public CeilingMediumFrequencyTrainer() : base("CMedFreqDrumTypeClassificationModel")
        {
            Train(new string[]
            {
                nameof(DrumTypeData.Frequency1850),
                nameof(DrumTypeData.Frequency2150),
                nameof(DrumTypeData.Frequency2500),
                nameof(DrumTypeData.Frequency2900),
            });
        }
    }
}
