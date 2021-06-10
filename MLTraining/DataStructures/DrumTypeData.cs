using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.ML.Data;

namespace MLTraining.DataStructures
{
    public class DrumTypeData
    {
        [LoadColumn(0)] public float Type { get; set; }
        [LoadColumn(1)] public float f50 { get; set; }
        [LoadColumn(2)] public float f50Avg { get; set; }
        [LoadColumn(3)] public float f50L1Norm { get; set; }
        [LoadColumn(4)] public float f50Mean { get; set; }
        [LoadColumn(5)] public float f150 { get; set; }
        [LoadColumn(6)] public float f150Avg { get; set; }
        [LoadColumn(7)] public float f150L1Norm { get; set; }
        [LoadColumn(8)] public float f150Mean { get; set; }
        [LoadColumn(9)] public float f250 { get; set; }
        [LoadColumn(10)] public float f250Avg { get; set; }
        [LoadColumn(11)] public float f250L1Norm { get; set; }
        [LoadColumn(12)] public float f250Mean { get; set; }
        [LoadColumn(13)] public float f350 { get; set; }
        [LoadColumn(14)] public float f350Avg { get; set; }
        [LoadColumn(15)] public float f350L1Norm { get; set; }
        [LoadColumn(16)] public float f350Mean { get; set; }
        [LoadColumn(17)] public float f450 { get; set; }
        [LoadColumn(18)] public float f450Avg { get; set; }
        [LoadColumn(19)] public float f450L1Norm { get; set; }
        [LoadColumn(20)] public float f450Mean { get; set; }
        [LoadColumn(21)] public float f570 { get; set; }
        [LoadColumn(22)] public float f570Avg { get; set; }
        [LoadColumn(23)] public float f570L1Norm { get; set; }
        [LoadColumn(24)] public float f570Mean { get; set; }
        [LoadColumn(25)] public float f700 { get; set; }
        [LoadColumn(26)] public float f700Avg { get; set; }
        [LoadColumn(27)] public float f700L1Norm { get; set; }
        [LoadColumn(28)] public float f700Mean { get; set; }
        [LoadColumn(29)] public float f840 { get; set; }
        [LoadColumn(30)] public float f840Avg { get; set; }
        [LoadColumn(31)] public float f840L1Norm { get; set; }
        [LoadColumn(32)] public float f840Mean { get; set; }
        [LoadColumn(33)] public float f1000 { get; set; }
        [LoadColumn(34)] public float f1000Avg { get; set; }
        [LoadColumn(35)] public float f1000L1Norm { get; set; }
        [LoadColumn(36)] public float f1000Mean { get; set; }
        [LoadColumn(37)] public float f1170 { get; set; }
        [LoadColumn(38)] public float f1170Avg { get; set; }
        [LoadColumn(39)] public float f1170L1Norm { get; set; }
        [LoadColumn(40)] public float f1170Mean { get; set; }
        [LoadColumn(41)] public float f1370 { get; set; }
        [LoadColumn(42)] public float f1370Avg { get; set; }
        [LoadColumn(43)] public float f1370L1Norm { get; set; }
        [LoadColumn(44)] public float f1370Mean { get; set; }
        [LoadColumn(45)] public float f1600 { get; set; }
        [LoadColumn(46)] public float f1600Avg { get; set; }
        [LoadColumn(47)] public float f1600L1Norm { get; set; }
        [LoadColumn(48)] public float f1600Mean { get; set; }
        [LoadColumn(49)] public float f1850 { get; set; }
        [LoadColumn(50)] public float f1850Avg { get; set; }
        [LoadColumn(51)] public float f1850L1Norm { get; set; }
        [LoadColumn(52)] public float f1850Mean { get; set; }
        [LoadColumn(53)] public float f2000 { get; set; }
        [LoadColumn(54)] public float f2000Avg { get; set; }
        [LoadColumn(55)] public float f2000L1Norm { get; set; }
        [LoadColumn(56)] public float f2000Mean { get; set; }
        [LoadColumn(57)] public float f2150 { get; set; }
        [LoadColumn(58)] public float f2150Avg { get; set; }
        [LoadColumn(59)] public float f2150L1Norm { get; set; }
        [LoadColumn(60)] public float f2150Mean { get; set; }
        [LoadColumn(61)] public float f2500 { get; set; }
        [LoadColumn(62)] public float f2500Avg { get; set; }
        [LoadColumn(63)] public float f2500L1Norm { get; set; }
        [LoadColumn(64)] public float f2500Mean { get; set; }
        [LoadColumn(65)] public float f2900 { get; set; }
        [LoadColumn(66)] public float f2900Avg { get; set; }
        [LoadColumn(67)] public float f2900L1Norm { get; set; }
        [LoadColumn(68)] public float f2900Mean { get; set; }
        [LoadColumn(69)] public float f3000 { get; set; }
        [LoadColumn(70)] public float f3000Avg { get; set; }
        [LoadColumn(71)] public float f3000L1Norm { get; set; }
        [LoadColumn(72)] public float f3000Mean { get; set; }
        [LoadColumn(73)] public float f3400 { get; set; }
        [LoadColumn(74)] public float f3400Avg { get; set; }
        [LoadColumn(75)] public float f3400L1Norm { get; set; }
        [LoadColumn(76)] public float f3400Mean { get; set; }
        [LoadColumn(77)] public float f4000 { get; set; }
        [LoadColumn(78)] public float f4000Avg { get; set; }
        [LoadColumn(79)] public float f4000L1Norm { get; set; }
        [LoadColumn(80)] public float f4000Mean { get; set; }
        [LoadColumn(81)] public float f4800 { get; set; }
        [LoadColumn(82)] public float f4800Avg { get; set; }
        [LoadColumn(83)] public float f4800L1Norm { get; set; }
        [LoadColumn(84)] public float f4800Mean { get; set; }
        [LoadColumn(85)] public float f5000 { get; set; }
        [LoadColumn(86)] public float f5000Avg { get; set; }
        [LoadColumn(87)] public float f5000L1Norm { get; set; }
        [LoadColumn(88)] public float f5000Mean { get; set; }
        [LoadColumn(89)] public float f5800 { get; set; }
        [LoadColumn(90)] public float f5800Avg { get; set; }
        [LoadColumn(91)] public float f5800L1Norm { get; set; }
        [LoadColumn(92)] public float f5800Mean { get; set; }
        [LoadColumn(93)] public float f6000 { get; set; }
        [LoadColumn(94)] public float f6000Avg { get; set; }
        [LoadColumn(95)] public float f6000L1Norm { get; set; }
        [LoadColumn(96)] public float f6000Mean { get; set; }
        [LoadColumn(97)] public float f7000 { get; set; }
        [LoadColumn(98)] public float f7000Avg { get; set; }
        [LoadColumn(99)] public float f7000L1Norm { get; set; }
        [LoadColumn(100)] public float f7000Mean { get; set; }
        [LoadColumn(101)] public float f8000 { get; set; }
        [LoadColumn(102)] public float f8000Avg { get; set; }
        [LoadColumn(103)] public float f8000L1Norm { get; set; }
        [LoadColumn(104)] public float f8000Mean { get; set; }
        [LoadColumn(105)] public float f8500 { get; set; }
        [LoadColumn(106)] public float f8500Avg { get; set; }
        [LoadColumn(107)] public float f8500L1Norm { get; set; }
        [LoadColumn(108)] public float f8500Mean { get; set; }
        [LoadColumn(109)] public float f9000 { get; set; }
        [LoadColumn(110)] public float f9000Avg { get; set; }
        [LoadColumn(111)] public float f9000L1Norm { get; set; }
        [LoadColumn(112)] public float f9000Mean { get; set; }
        [LoadColumn(113)] public float f10000 { get; set; }
        [LoadColumn(114)] public float f10000Avg { get; set; }
        [LoadColumn(115)] public float f10000L1Norm { get; set; }
        [LoadColumn(116)] public float f10000Mean { get; set; }
        [LoadColumn(117)] public float f10500 { get; set; }
        [LoadColumn(118)] public float f10500Avg { get; set; }
        [LoadColumn(119)] public float f10500L1Norm { get; set; }
        [LoadColumn(120)] public float f10500Mean { get; set; }
        [LoadColumn(121)] public float f11000 { get; set; }
        [LoadColumn(122)] public float f11000Avg { get; set; }
        [LoadColumn(123)] public float f11000L1Norm { get; set; }
        [LoadColumn(124)] public float f11000Mean { get; set; }
        [LoadColumn(125)] public float f12000 { get; set; }
        [LoadColumn(126)] public float f12000Avg { get; set; }
        [LoadColumn(127)] public float f12000L1Norm { get; set; }
        [LoadColumn(128)] public float f12000Mean { get; set; }
        [LoadColumn(129)] public float f13000 { get; set; }
        [LoadColumn(130)] public float f13000Avg { get; set; }
        [LoadColumn(131)] public float f13000L1Norm { get; set; }
        [LoadColumn(132)] public float f13000Mean { get; set; }
        [LoadColumn(133)] public float f13500 { get; set; }
        [LoadColumn(134)] public float f13500Avg { get; set; }
        [LoadColumn(135)] public float f13500L1Norm { get; set; }
        [LoadColumn(136)] public float f13500Mean { get; set; }
        [LoadColumn(137)] public float f14000 { get; set; }
        [LoadColumn(138)] public float f14000Avg { get; set; }
        [LoadColumn(139)] public float f14000L1Norm { get; set; }
        [LoadColumn(140)] public float f14000Mean { get; set; }
        [LoadColumn(141)] public float f15000 { get; set; }
        [LoadColumn(142)] public float f15000Avg { get; set; }
        [LoadColumn(143)] public float f15000L1Norm { get; set; }
        [LoadColumn(144)] public float f15000Mean { get; set; }
        [LoadColumn(145)] public float f16000 { get; set; }
        [LoadColumn(146)] public float f16000Avg { get; set; }
        [LoadColumn(147)] public float f16000L1Norm { get; set; }
        [LoadColumn(148)] public float f16000Mean { get; set; }
        [LoadColumn(149)] public float f17000 { get; set; }
        [LoadColumn(150)] public float f17000Avg { get; set; }
        [LoadColumn(151)] public float f17000L1Norm { get; set; }
        [LoadColumn(152)] public float f17000Mean { get; set; }
        [LoadColumn(153)] public float f18000 { get; set; }
        [LoadColumn(154)] public float f18000Avg { get; set; }
        [LoadColumn(155)] public float f18000L1Norm { get; set; }
        [LoadColumn(156)] public float f18000Mean { get; set; }
        [LoadColumn(157)] public float f19000 { get; set; }
        [LoadColumn(158)] public float f19000Avg { get; set; }
        [LoadColumn(159)] public float f19000L1Norm { get; set; }
        [LoadColumn(160)] public float f19000Mean { get; set; }
        [LoadColumn(161)] public float f20000 { get; set; }
        [LoadColumn(162)] public float f20000Avg { get; set; }
        [LoadColumn(163)] public float f20000L1Norm { get; set; }
        [LoadColumn(164)] public float f20000Mean { get; set; }


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

            return f50 > 0 ||
                   f50Avg > 0 ||
                   f50L1Norm > 0 ||
                   f50Mean > 0 ||
                   f150 > 0 ||
                   f150Avg > 0 ||
                   f150L1Norm > 0 ||
                   f150Mean > 0 ||
                   f250 > 0 ||
                   f250Avg > 0 ||
                   f250L1Norm > 0 ||
                   f250Mean > 0 ||
                   f350 > 0 ||
                   f350Avg > 0 ||
                   f350L1Norm > 0 ||
                   f350Mean > 0 ||
                   f450 > 0 ||
                   f450Avg > 0 ||
                   f450L1Norm > 0 ||
                   f450Mean > 0 ||
                   f570 > 0 ||
                   f570Avg > 0 ||
                   f570L1Norm > 0 ||
                   f570Mean > 0 ||
                   f700 > 0 ||
                   f700Avg > 0 ||
                   f700L1Norm > 0 ||
                   f700Mean > 0 ||
                   f840 > 0 ||
                   f840Avg > 0 ||
                   f840L1Norm > 0 ||
                   f840Mean > 0 ||
                   f1000 > 0 ||
                   f1000Avg > 0 ||
                   f1000L1Norm > 0 ||
                   f1000Mean > 0 ||
                   f1170 > 0 ||
                   f1170Avg > 0 ||
                   f1170L1Norm > 0 ||
                   f1170Mean > 0 ||
                   f1370 > 0 ||
                   f1370Avg > 0 ||
                   f1370L1Norm > 0 ||
                   f1370Mean > 0 ||
                   f1600 > 0 ||
                   f1600Avg > 0 ||
                   f1600L1Norm > 0 ||
                   f1600Mean > 0 ||
                   f1850 > 0 ||
                   f1850Avg > 0 ||
                   f1850L1Norm > 0 ||
                   f1850Mean > 0 ||
                   f2000 > 0 ||
                   f2000Avg > 0 ||
                   f2000L1Norm > 0 ||
                   f2000Mean > 0 ||
                   f2150 > 0 ||
                   f2150Avg > 0 ||
                   f2150L1Norm > 0 ||
                   f2150Mean > 0 ||
                   f2500 > 0 ||
                   f2500Avg > 0 ||
                   f2500L1Norm > 0 ||
                   f2500Mean > 0 ||
                   f2900 > 0 ||
                   f2900Avg > 0 ||
                   f2900L1Norm > 0 ||
                   f2900Mean > 0 ||
                   f3000 > 0 ||
                   f3000Avg > 0 ||
                   f3000L1Norm > 0 ||
                   f3000Mean > 0 ||
                   f3400 > 0 ||
                   f3400Avg > 0 ||
                   f3400L1Norm > 0 ||
                   f3400Mean > 0 ||
                   f4000 > 0 ||
                   f4000Avg > 0 ||
                   f4000L1Norm > 0 ||
                   f4000Mean > 0 ||
                   f4800 > 0 ||
                   f4800Avg > 0 ||
                   f4800L1Norm > 0 ||
                   f4800Mean > 0 ||
                   f5000 > 0 ||
                   f5000Avg > 0 ||
                   f5000L1Norm > 0 ||
                   f5000Mean > 0 ||
                   f5800 > 0 ||
                   f5800Avg > 0 ||
                   f5800L1Norm > 0 ||
                   f5800Mean > 0 ||
                   f6000 > 0 ||
                   f6000Avg > 0 ||
                   f6000L1Norm > 0 ||
                   f6000Mean > 0 ||
                   f7000 > 0 ||
                   f7000Avg > 0 ||
                   f7000L1Norm > 0 ||
                   f7000Mean > 0 ||
                   f8000 > 0 ||
                   f8000Avg > 0 ||
                   f8000L1Norm > 0 ||
                   f8000Mean > 0 ||
                   f8500 > 0 ||
                   f8500Avg > 0 ||
                   f8500L1Norm > 0 ||
                   f8500Mean > 0 ||
                   f9000 > 0 ||
                   f9000Avg > 0 ||
                   f9000L1Norm > 0 ||
                   f9000Mean > 0 ||
                   f10000 > 0 ||
                   f10000Avg > 0 ||
                   f10000L1Norm > 0 ||
                   f10000Mean > 0 ||
                   f10500 > 0 ||
                   f10500Avg > 0 ||
                   f10500L1Norm > 0 ||
                   f10500Mean > 0 ||
                   f11000 > 0 ||
                   f11000Avg > 0 ||
                   f11000L1Norm > 0 ||
                   f11000Mean > 0 ||
                   f12000 > 0 ||
                   f12000Avg > 0 ||
                   f12000L1Norm > 0 ||
                   f12000Mean > 0 ||
                   f13000 > 0 ||
                   f13000Avg > 0 ||
                   f13000L1Norm > 0 ||
                   f13000Mean > 0 ||
                   f13500 > 0 ||
                   f13500Avg > 0 ||
                   f13500L1Norm > 0 ||
                   f13500Mean > 0 ||
                   f14000 > 0 ||
                   f14000Avg > 0 ||
                   f14000L1Norm > 0 ||
                   f14000Mean > 0 ||
                   f15000 > 0 ||
                   f15000Avg > 0 ||
                   f15000L1Norm > 0 ||
                   f15000Mean > 0 ||
                   f16000 > 0 ||
                   f16000Avg > 0 ||
                   f16000L1Norm > 0 ||
                   f16000Mean > 0 ||
                   f17000 > 0 ||
                   f17000Avg > 0 ||
                   f17000L1Norm > 0 ||
                   f17000Mean > 0 ||
                   f18000 > 0 ||
                   f18000Avg > 0 ||
                   f18000L1Norm > 0 ||
                   f18000Mean > 0 ||
                   f19000 > 0 ||
                   f19000Avg > 0 ||
                   f19000L1Norm > 0 ||
                   f19000Mean > 0 ||
                   f20000 > 0 ||
                   f20000Avg > 0 ||
                   f20000L1Norm > 0 ||
                   f20000Mean > 0;
        }

        public void SetValues(float peakValue, float avg, float mean, float l1Norm, double frequency)
        {
            try
            {
                var AVERAGE = "Avg";
                var MEAN = "Mean";
                var L1NORM = "L1";

                var type = typeof(DrumTypeData);

                // Reset every prop before changing
                //foreach (var prop in type.GetProperties().ToList()) prop.SetValue(this, (float) 0f, null);

                var peakProp = type.GetProperties().First(prop =>
                    prop.Name.Contains($"{frequency:####}")
                    && !prop.Name.Contains(AVERAGE)
                    && !prop.Name.Contains(MEAN)
                    && !prop.Name.Contains(L1NORM));

                var avgProp = type.GetProperties().First(prop =>
                    prop.Name.Contains($"{frequency:####}") && prop.Name.Contains(AVERAGE));

                var meanProp = type.GetProperties().First(prop =>
                    prop.Name.Contains($"{frequency:####}") && prop.Name.Contains(MEAN));

                var l1NormProp = type.GetProperties().First(prop =>
                    prop.Name.Contains($"{frequency:####}") && prop.Name.Contains(L1NORM));

                peakProp.SetValue(this, peakValue, null);
                avgProp.SetValue(this, avg, null);
                meanProp.SetValue(this, mean, null);
                l1NormProp.SetValue(this, l1Norm, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EXCEPTION at SetValues, {ex.Message}");
            }
        }

        /// <summary>
        /// Sets every value to 0f
        /// </summary>
        public void ResetData()
        {
            var type = typeof(DrumTypeData);
            foreach (var prop in type.GetProperties().ToList()) prop.SetValue(this, (float)0f, null);
        }
    }
}