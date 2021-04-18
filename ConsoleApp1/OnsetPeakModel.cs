using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace OnsetDataGeneration
{
    public class OnsetPeakModel
    {
        public float PeakValue { get; set; }
        private float[] fftData;
        private Vector<float> _fftVector;
        // Gets the FftVector, will not calculate again if already exists
        // This is basically a wrapper around _fftVector, but prevents recursive access
        private Vector<float> FftVector
        {
            get
            {
                if (_fftVector == null)
                {
                    _fftVector = Vector<float>.Build.DenseOfArray(fftData);
                }
                return _fftVector;
            }

            set
            {

            }
        }


        public OnsetPeakModel(float peakValue, float[] fftData)
        {
            PeakValue = peakValue;
            this.fftData = fftData;
        }

        public string ToString(bool leadingComma = true, string seperator = ",")
        {
            var fftMean = FftVector.Mean();
            var fftAvg = FftVector.Average();

            var dataPoints = new string[] { PeakValue.ToString(), fftMean.ToString(), fftAvg.ToString() };
            
            var s = string.Join(seperator, dataPoints);

            if (leadingComma) s += ",";

            return s;
        }

    }
}
