using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OnsetPredictions
{
    class MidiDrumScoreEqualityComparer : IEqualityComparer<Tuple<DrumSoundType, double>>
    {
        public bool Equals(Tuple<DrumSoundType, double> x, Tuple<DrumSoundType, double> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Item1 == y.Item1 && ((Object) x.Item2).Equals(y.Item2);
        }

        public int GetHashCode(Tuple<DrumSoundType, double> obj)
        {
            return HashCode.Combine((int) obj.Item1, obj.Item2);
        }
    }
}
