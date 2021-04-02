using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.ML.Data;

namespace MLTraining.DataStructures
{
    public class DrumTypeData
    {
        [LoadColumn(0)] public float Type { get; set; }
        [LoadColumn(1)] public float Frequency50 { get; set; }
        [LoadColumn(2)] public float Frequency100 { get; set; }
        [LoadColumn(3)] public float Frequency150 { get; set; }
        [LoadColumn(4)] public float Frequency200 { get; set; }
        [LoadColumn(5)] public float Frequency250 { get; set; }
        [LoadColumn(6)] public float Frequency300 { get; set; }
        [LoadColumn(7)] public float Frequency350 { get; set; }
        [LoadColumn(8)] public float Frequency400 { get; set; }
        [LoadColumn(9)] public float Frequency450 { get; set; }
        [LoadColumn(10)] public float Frequency500 { get; set; }
        [LoadColumn(11)] public float Frequency550 { get; set; }
        [LoadColumn(12)] public float Frequency600 { get; set; }
        [LoadColumn(13)] public float Frequency650 { get; set; }
        [LoadColumn(14)] public float Frequency700 { get; set; }
        [LoadColumn(15)] public float Frequency750 { get; set; }
        [LoadColumn(16)] public float Frequency800 { get; set; }
        [LoadColumn(17)] public float Frequency850 { get; set; }
        [LoadColumn(18)] public float Frequency900 { get; set; }
        [LoadColumn(19)] public float Frequency950 { get; set; }
        [LoadColumn(20)] public float Frequency1000 { get; set; }
        [LoadColumn(21)] public float Frequency1050 { get; set; }
        [LoadColumn(22)] public float Frequency1100 { get; set; }
        [LoadColumn(23)] public float Frequency1150 { get; set; }
        [LoadColumn(24)] public float Frequency1200 { get; set; }
        [LoadColumn(25)] public float Frequency1250 { get; set; }
        [LoadColumn(26)] public float Frequency1300 { get; set; }
        [LoadColumn(27)] public float Frequency1350 { get; set; }
        [LoadColumn(28)] public float Frequency1400 { get; set; }
        [LoadColumn(29)] public float Frequency1450 { get; set; }
        [LoadColumn(30)] public float Frequency1500 { get; set; }
        [LoadColumn(31)] public float Frequency1550 { get; set; }
        [LoadColumn(32)] public float Frequency1600 { get; set; }
        [LoadColumn(33)] public float Frequency1650 { get; set; }
        [LoadColumn(34)] public float Frequency1700 { get; set; }
        [LoadColumn(35)] public float Frequency1750 { get; set; }
        [LoadColumn(36)] public float Frequency1800 { get; set; }
        [LoadColumn(37)] public float Frequency1850 { get; set; }
        [LoadColumn(38)] public float Frequency1900 { get; set; }
        [LoadColumn(39)] public float Frequency1950 { get; set; }
        [LoadColumn(40)] public float Frequency2000 { get; set; }
        [LoadColumn(41)] public float Frequency2050 { get; set; }
        [LoadColumn(42)] public float Frequency2100 { get; set; }
        [LoadColumn(43)] public float Frequency2150 { get; set; }
        [LoadColumn(44)] public float Frequency2200 { get; set; }
        [LoadColumn(45)] public float Frequency2250 { get; set; }
        [LoadColumn(46)] public float Frequency2300 { get; set; }
        [LoadColumn(47)] public float Frequency2350 { get; set; }
        [LoadColumn(48)] public float Frequency2400 { get; set; }
        [LoadColumn(49)] public float Frequency2450 { get; set; }
        [LoadColumn(50)] public float Frequency2500 { get; set; }
        [LoadColumn(51)] public float Frequency2550 { get; set; }
        [LoadColumn(52)] public float Frequency2600 { get; set; }
        [LoadColumn(53)] public float Frequency2650 { get; set; }
        [LoadColumn(54)] public float Frequency2700 { get; set; }
        [LoadColumn(55)] public float Frequency2750 { get; set; }
        [LoadColumn(56)] public float Frequency2800 { get; set; }
        [LoadColumn(57)] public float Frequency2850 { get; set; }
        [LoadColumn(58)] public float Frequency2900 { get; set; }
        [LoadColumn(59)] public float Frequency2950 { get; set; }
        [LoadColumn(60)] public float Frequency3000 { get; set; }
        [LoadColumn(61)] public float Frequency3050 { get; set; }
        [LoadColumn(62)] public float Frequency3100 { get; set; }
        [LoadColumn(63)] public float Frequency3150 { get; set; }
        [LoadColumn(64)] public float Frequency3200 { get; set; }
        [LoadColumn(65)] public float Frequency3250 { get; set; }
        [LoadColumn(66)] public float Frequency3300 { get; set; }
        [LoadColumn(67)] public float Frequency3350 { get; set; }
        [LoadColumn(68)] public float Frequency3400 { get; set; }
        [LoadColumn(69)] public float Frequency3450 { get; set; }
        [LoadColumn(70)] public float Frequency3500 { get; set; }
        [LoadColumn(71)] public float Frequency3550 { get; set; }
        [LoadColumn(72)] public float Frequency3600 { get; set; }
        [LoadColumn(73)] public float Frequency3650 { get; set; }
        [LoadColumn(74)] public float Frequency3700 { get; set; }
        [LoadColumn(75)] public float Frequency3750 { get; set; }
        [LoadColumn(76)] public float Frequency3800 { get; set; }
        [LoadColumn(77)] public float Frequency3850 { get; set; }
        [LoadColumn(78)] public float Frequency3900 { get; set; }
        [LoadColumn(79)] public float Frequency3950 { get; set; }
        [LoadColumn(80)] public float Frequency4000 { get; set; }


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

            return Frequency50 > 0 ||
                   Frequency100 > 0 ||
                   Frequency150 > 0 ||
                   Frequency200 > 0 ||
                   Frequency250 > 0 ||
                   Frequency300 > 0 ||
                   Frequency350 > 0 ||
                   Frequency400 > 0 ||
                   Frequency450 > 0 ||
                   Frequency500 > 0 ||
                   Frequency550 > 0 ||
                   Frequency600 > 0 ||
                   Frequency650 > 0 ||
                   Frequency700 > 0 ||
                   Frequency750 > 0 ||
                   Frequency800 > 0 ||
                   Frequency850 > 0 ||
                   Frequency900 > 0 ||
                   Frequency950 > 0 ||
                   Frequency1000 > 0 ||
                   Frequency1050 > 0 ||
                   Frequency1100 > 0 ||
                   Frequency1150 > 0 ||
                   Frequency1200 > 0 ||
                   Frequency1250 > 0 ||
                   Frequency1300 > 0 ||
                   Frequency1350 > 0 ||
                   Frequency1400 > 0 ||
                   Frequency1450 > 0 ||
                   Frequency1500 > 0 ||
                   Frequency1550 > 0 ||
                   Frequency1600 > 0 ||
                   Frequency1650 > 0 ||
                   Frequency1700 > 0 ||
                   Frequency1750 > 0 ||
                   Frequency1800 > 0 ||
                   Frequency1850 > 0 ||
                   Frequency1900 > 0 ||
                   Frequency1950 > 0 ||
                   Frequency2000 > 0 ||
                   Frequency2050 > 0 ||
                   Frequency2100 > 0 ||
                   Frequency2150 > 0 ||
                   Frequency2200 > 0 ||
                   Frequency2250 > 0 ||
                   Frequency2300 > 0 ||
                   Frequency2350 > 0 ||
                   Frequency2400 > 0 ||
                   Frequency2450 > 0 ||
                   Frequency2500 > 0 ||
                   Frequency2550 > 0 ||
                   Frequency2600 > 0 ||
                   Frequency2650 > 0 ||
                   Frequency2700 > 0 ||
                   Frequency2750 > 0 ||
                   Frequency2800 > 0 ||
                   Frequency2850 > 0 ||
                   Frequency2900 > 0 ||
                   Frequency2950 > 0 ||
                   Frequency3000 > 0 ||
                   Frequency3050 > 0 ||
                   Frequency3100 > 0 ||
                   Frequency3150 > 0 ||
                   Frequency3200 > 0 ||
                   Frequency3250 > 0 ||
                   Frequency3300 > 0 ||
                   Frequency3350 > 0 ||
                   Frequency3400 > 0 ||
                   Frequency3450 > 0 ||
                   Frequency3500 > 0 ||
                   Frequency3550 > 0 ||
                   Frequency3600 > 0 ||
                   Frequency3650 > 0 ||
                   Frequency3700 > 0 ||
                   Frequency3750 > 0 ||
                   Frequency3800 > 0 ||
                   Frequency3850 > 0 ||
                   Frequency3900 > 0 ||
                   Frequency3950 > 0 ||
                   Frequency4000 > 0;
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