using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.ML.Data;

namespace MLTraining.DataStructures
{
    public class DrumTypeData
    {
        [LoadColumn(0)] public float Type { get; set; }
        [LoadColumn(1)] public float Frequency50 { get; set; }
        [LoadColumn(2)] public float Frequency150 { get; set; }
        [LoadColumn(3)] public float Frequency250 { get; set; }
        [LoadColumn(4)] public float Frequency350 { get; set; }
        [LoadColumn(5)] public float Frequency450 { get; set; }
        [LoadColumn(6)] public float Frequency570 { get; set; }
        [LoadColumn(7)] public float Frequency700 { get; set; }
        [LoadColumn(8)] public float Frequency840 { get; set; }
        [LoadColumn(9)] public float Frequency1000 { get; set; }
        [LoadColumn(10)] public float Frequency1170 { get; set; }
        [LoadColumn(11)] public float Frequency1370 { get; set; }
        [LoadColumn(12)] public float Frequency1600 { get; set; }
        [LoadColumn(13)] public float Frequency1850 { get; set; }
        [LoadColumn(14)] public float Frequency2150 { get; set; }
        [LoadColumn(15)] public float Frequency2500 { get; set; }
        [LoadColumn(16)] public float Frequency2900 { get; set; }
        [LoadColumn(17)] public float Frequency3400 { get; set; }
        [LoadColumn(18)] public float Frequency4000 { get; set; }
        [LoadColumn(19)] public float Frequency4800 { get; set; }
        [LoadColumn(20)] public float Frequency5800 { get; set; }
        [LoadColumn(21)] public float Frequency7000 { get; set; }
        [LoadColumn(22)] public float Frequency8500 { get; set; }
        [LoadColumn(23)] public float Frequency10500 { get; set; }
        [LoadColumn(24)] public float Frequency13500 { get; set; }

        /// <summary>
        ///     Returns true if at least one property (excluding Type) contains a value > 0
        /// </summary>
        /// <returns></returns>
        public bool HasValue()
        {
            return Frequency50 > 0 || Frequency150 > 0 || Frequency250 > 0 || Frequency350 > 0 ||
                   Frequency450 > 0 || Frequency570 > 0 || Frequency700 > 0 || Frequency840 > 0 || Frequency1000 > 0 ||
                   Frequency1170 > 0 || Frequency1370 > 0 ||
                   Frequency1600 > 0 || Frequency1850 > 0 || Frequency2150 > 0 || Frequency2500 > 0 ||
                   Frequency2900 > 0 || Frequency3400 > 0 || Frequency4000 > 0 || Frequency4800 > 0 ||
                   Frequency5800 > 0 || Frequency7000 > 0 ||
                   Frequency8500 > 0 || Frequency10500 > 0 || Frequency13500 > 0;
        }

        public void SetValues(float peakValue, double frequency)
        {
            try
            {
                Type type = typeof(DrumTypeData);

                PropertyInfo prop = type.GetProperties().First(prop => prop.Name.Contains($"{frequency:####}"));

                prop.SetValue(this, peakValue, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EXCEPTION at SetValues, {ex.Message}");
            }
        }
    }
}