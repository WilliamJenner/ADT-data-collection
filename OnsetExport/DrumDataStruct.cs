using System;
using OnsetDetection;

namespace OnsetExport
{
    public class DrumDataStruct
    {
        public double Frequency { get; set; }
        public OnsetPeakModel OnsetPeakModel { get; set; }
        public string TimeOccured { get; set; }

        public DrumDataStruct(double frequency, OnsetPeakModel onsetPeakModel)
        {
            Frequency = frequency;
            OnsetPeakModel = onsetPeakModel;
            TimeOccured = DateTime.Now.ToString("HH:mm:ss.f");
        }

        /// <summary>
        /// Checks the peak values of both data structs and returns if a is greater than b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(DrumDataStruct a, DrumDataStruct b)
        {
            return a.OnsetPeakModel.PeakValue > b.OnsetPeakModel.PeakValue;
        }

        /// <summary>
        /// Checks the peak values of both data structs and returns if a is less than b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(DrumDataStruct a, DrumDataStruct b)
        {
            return a.OnsetPeakModel.PeakValue < b.OnsetPeakModel.PeakValue;
        }
    }
}
