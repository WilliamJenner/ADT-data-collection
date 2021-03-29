using Newtonsoft.Json;

namespace OnsetPredictions.BarkScale
{
    public class CriticalBandModel
    {
        public int Number { get; set; }

        [JsonProperty("Center frequency (Hz)")] public int CenterFrequencyHz { get; set; }

        [JsonProperty("Cut-off frequency (Hz)")] public int CutOffFrequencyHz { get; set; }

        [JsonProperty("Bandwidth (Hz)")] public int BandwidthHz { get; set; }
    }
}