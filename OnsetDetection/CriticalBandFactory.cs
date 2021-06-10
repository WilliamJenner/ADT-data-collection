using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnsetDetection
{
    public static class CriticalBandFactory
    {
        /// <summary>
        /// Gets a list of all critical bands being used within the application
        /// </summary>
        /// <returns></returns>
        public static List<double> Get() => CriticalBandFrequencies;

        private static List<double> CriticalBandFrequencies =>
            BarkScale.BarkScale.CriticalBands().Select(x => (double)x.CenterFrequencyHz).Union(Enumerable.Range(1, 20001)
                    .Where(integer => integer % 1000 == 0)
                    .Select(Convert.ToDouble))
                .Distinct()
                .OrderBy(c => c)
                .ToList();
    }
}
