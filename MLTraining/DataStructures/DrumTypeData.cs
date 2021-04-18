using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.ML.Data;

namespace MLTraining.DataStructures
{
    public class DrumTypeData
    {
        [LoadColumn(0)] public float Type { get; set; }
        [LoadColumn(1)] public float Frequency10 { get; set; }
        [LoadColumn(2)] public float Frequency20 { get; set; }
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
        [LoadColumn(14)] public float Frequency2000 { get; set; }
        [LoadColumn(15)] public float Frequency2150 { get; set; }
        [LoadColumn(16)] public float Frequency2500 { get; set; }
        [LoadColumn(17)] public float Frequency2900 { get; set; }
        [LoadColumn(18)] public float Frequency3000 { get; set; }
        [LoadColumn(19)] public float Frequency3400 { get; set; }
        [LoadColumn(20)] public float Frequency4000 { get; set; }
        [LoadColumn(21)] public float Frequency4800 { get; set; }
        [LoadColumn(22)] public float Frequency5000 { get; set; }
        [LoadColumn(23)] public float Frequency5800 { get; set; }
        [LoadColumn(24)] public float Frequency6000 { get; set; }
        [LoadColumn(25)] public float Frequency7000 { get; set; }
        [LoadColumn(26)] public float Frequency8000 { get; set; }
        [LoadColumn(27)] public float Frequency8500 { get; set; }
        [LoadColumn(28)] public float Frequency9000 { get; set; }
        [LoadColumn(29)] public float Frequency10000 { get; set; }
        [LoadColumn(30)] public float Frequency10500 { get; set; }
        [LoadColumn(31)] public float Frequency11000 { get; set; }
        [LoadColumn(32)] public float Frequency12000 { get; set; }
        [LoadColumn(33)] public float Frequency13000 { get; set; }
        [LoadColumn(34)] public float Frequency13500 { get; set; }
        [LoadColumn(35)] public float Frequency14000 { get; set; }
        [LoadColumn(36)] public float Frequency15000 { get; set; }
        [LoadColumn(37)] public float Frequency16000 { get; set; }
        [LoadColumn(38)] public float Frequency17000 { get; set; }
        [LoadColumn(39)] public float Frequency18000 { get; set; }
        [LoadColumn(40)] public float Frequency19000 { get; set; }
        [LoadColumn(41)] public float Frequency20000 { get; set; }



        /// <summary>
        ///     Returns true if at least one property (excluding Type) contains a value > 0
        /// </summary>
        /// <returns></returns>
        public bool HasValue()
        {
            //return Frequency50 > 0 || Frequency150 > 0 || Frequency250 > 0 || Frequency350 > 0 ||
            //       Frequency450 > 0 || Frequency570 > 0 || Frequency700 > 0 || Frequency840 > 0 || Frequency1000 > 0 ||
            //       Frequency1170 > 0 || Frequency1370 > 0 ||
            //       Frequency1600 > 0 || Frequency1850 > 0 || Frequency2150 > 0 || Frequency2500 > 0 ||
            //       Frequency2900 > 0 || Frequency3400 > 0 || Frequency4000 > 0 || Frequency4800 > 0 ||
            //       Frequency5800 > 0 || Frequency7000 > 0 ||
            //       Frequency8500 > 0 || Frequency10500 > 0 || Frequency13500 > 0;

            return Frequency10 > 0 ||
                   Frequency20 > 0 ||
                   Frequency250 > 0 ||
                   Frequency350 > 0 ||
                   Frequency450 > 0 ||
                   Frequency570 > 0 ||
                   Frequency700 > 0 ||
                   Frequency840 > 0 ||
                   Frequency1000 > 0 ||
                   Frequency1170 > 0 ||
                   Frequency1370 > 0 ||
                   Frequency1600 > 0 ||
                   Frequency1850 > 0 ||
                   Frequency2000 > 0 ||
                   Frequency2150 > 0 ||
                   Frequency2500 > 0 ||
                   Frequency2900 > 0 ||
                   Frequency3000 > 0 ||
                   Frequency3400 > 0 ||
                   Frequency4000 > 0 ||
                   Frequency4800 > 0 ||
                   Frequency5000 > 0 ||
                   Frequency5800 > 0 ||
                   Frequency6000 > 0 ||
                   Frequency7000 > 0 ||
                   Frequency8000 > 0 ||
                   Frequency8500 > 0 ||
                   Frequency9000 > 0 ||
                   Frequency10000 > 0 ||
                   Frequency10500 > 0 ||
                   Frequency11000 > 0 ||
                   Frequency12000 > 0 ||
                   Frequency13000 > 0 ||
                   Frequency13500 > 0 ||
                   Frequency14000 > 0 ||
                   Frequency15000 > 0 ||
                   Frequency16000 > 0 ||
                   Frequency17000 > 0 ||
                   Frequency18000 > 0 ||
                   Frequency19000 > 0 ||
                   Frequency20000 > 0;
        }

        public void SetValues(float peakValue, double frequency)
        {
            try
            {
                var type = typeof(DrumTypeData);

                var prop = type.GetProperties().First(prop => prop.Name.Contains($"{frequency:####}"));

                prop.SetValue(this, peakValue, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EXCEPTION at SetValues, {ex.Message}");
            }
        }
    }
}