using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace OnsetDetection
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

        public float Mean()
        {
            return (float)FftVector.Mean();
        }

        public float Average()
        {
            return FftVector.Average();
        }

        public float L1Norm()
        {
            return (float)FftVector.L1Norm();
        }

        public string ToString(bool leadingComma = true, string seperator = ",")
        {
            try
            {
                if (PeakValue == 0) return GetEmptyString(leadingComma);


                var dataPoints = new string[] { PeakValue.ToString(), Mean().ToString(), Average().ToString(), L1Norm().ToString() };

                var s = string.Join(seperator, dataPoints);

                if (leadingComma) s += ",";

                return s;
            }
            catch (Exception ex)
            {
                return GetEmptyString(leadingComma);
            }
        }

        private string GetEmptyString(bool leadingComma, string seperator = ",")
        {
            // This has to have the same amount of data points as ToString
            var dataPoints = new string[] { "0", "0", "0", "0" };
            var s = string.Join(seperator, dataPoints);
            if (leadingComma) s += ",";
            return s;
        }

    }
}
