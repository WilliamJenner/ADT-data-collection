using System.Collections.Concurrent;

namespace OnsetDataGeneration
{
    public class BarkFiltering
    {
        private BiQuadFilterSource BiQuadFilterSource;
        public ConcurrentBag<BiQuadFilterSource> FilteredSources;

        public BarkFiltering(BiQuadFilterSource biQuadFilterSource)
        {
            BiQuadFilterSource = biQuadFilterSource;
            FilteredSources = new ConcurrentBag<BiQuadFilterSource>();
            FilterSources();
        }

        /// <summary>
        /// Adds to the FilteredSources list a bandpass filter for each center frequency in the BarkScale
        /// For each part of the scale, we clone the biquad filter source
        /// </summary>
        private void FilterSources()
        {
            //Parallel.ForEach(barkScale.scale, (band) =>
            //{
            //    //var copy = (BiQuadFilterSource) BiQuadFilterSource.cl
            //    copy.Filter = new BandpassFilter(copy.WaveFormat.SampleRate, band.CenterFrequencyHz);
            //    copy.CriticalBand = band;
            //    FilteredSources.Add(copy);
            //});
        }
    }
}
