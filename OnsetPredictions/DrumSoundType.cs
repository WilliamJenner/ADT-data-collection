using System;
using System.Collections;

namespace OnsetPredictions
{
    public enum DrumSoundType
    {
        Kick,
        Snare,
        Cymbal,
        LowTom,
        HighTom,
    }

    public static class DrumSoundTypeExtensions 
    {
        public static MidiDrum ToMidiDrum(this DrumSoundType soundType)
        {
            switch (soundType)
            {
                case DrumSoundType.Kick:
                    return MidiDrum.BassDrum1;
                case DrumSoundType.Snare:
                    return MidiDrum.SnareDrum1;
                case DrumSoundType.Cymbal:
                    return MidiDrum.CrashCymbal1;
                case DrumSoundType.HighTom:
                    return MidiDrum.HighTom1;
                case DrumSoundType.LowTom:
                    return MidiDrum.LowTom1;
                default:
                    throw new ArgumentOutOfRangeException("soundType", $"Drum sound type was {soundType}");
            }
        }
    }
}
