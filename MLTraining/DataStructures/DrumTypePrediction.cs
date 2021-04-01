using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace MLTraining.DataStructures
{
    public class DrumTypePrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;
        [ColumnName("Score")]
        public float[] Score;
    }
}
