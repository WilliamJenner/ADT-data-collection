using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSCore.SoundIn;

namespace OnsetDataGeneration
{
    public interface IOnsetWriterBuilder
    {
        void Detect(IEnumerable<double> frequencies);
        ConcurrentDictionary<double, OnsetWriter> GetOnsetWriters();
        void Reset();
    }

    public class OnsetWriterBuilder : IOnsetWriterBuilder
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-5.0
        // Using ConcurrentDictionary as it can handle multiple threads concurrently / large collections
        private ConcurrentDictionary<double, OnsetWriter> OnsetWriters = new ConcurrentDictionary<double, OnsetWriter>();
        private ISoundIn soundIn;

        public OnsetWriterBuilder(ISoundIn soundIn)
        {
            this.soundIn = soundIn;

            Reset();
        }

        /// <summary>
        /// Creates an onset writer for each frequency in parallel and begins detecting
        /// </summary>
        /// <param name="frequencies"></param>
        public void Detect(IEnumerable<double> frequencies)
        {
            Parallel.ForEach(frequencies, frequency =>
            {
                var writer = new OnsetWriter(this.soundIn, frequency);
                OnsetWriters.AddOrUpdate(frequency, writer, (d, oldWriter) => writer);
            });
        }

        public ConcurrentDictionary<double, OnsetWriter> GetOnsetWriters()
        {
            return OnsetWriters;
        }

        public void Reset()
        {
            OnsetWriters.Clear();
        }
    }
}
