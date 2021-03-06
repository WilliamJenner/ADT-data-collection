using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CSCore.XAudio2.X3DAudio;
using M;

namespace OnsetPredictions
{
    public class MidiBroadcaster
    {
        public bool Broadcasting = false;
        
        private System.Timers.Timer Timer;
        private ConcurrentBag<Tuple<DrumSoundType, double, string>> _drumSounds;
        private MidiOutputDevice dev;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="broadcasting"></param>
        /// <param name="outputDevice"></param>
        /// <param name="interval">Interval for the internal timer (milliseconds). Default is 100 </param>
        public MidiBroadcaster(bool broadcasting, MidiOutputDevice outputDevice, double interval = 100)
        {
            dev = outputDevice;
            Broadcasting = broadcasting;
            _drumSounds = new ConcurrentBag<Tuple<DrumSoundType, double, string>>();
            SetTimer(interval);
            dev.Open();
        }

        public void Broadcast(DrumSoundType drumType, double highScore, string modelName)
        {
            _drumSounds.Add(new Tuple<DrumSoundType, double, string>(drumType, highScore, modelName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval">Interval property passed to <see cref="System.Timers.Timer"/>. Specified in milliseconds</param>
        private void SetTimer(double interval)
        {
            // Create a timer with a two second interval.
            Timer = new System.Timers.Timer(interval);
            // Hook up the Elapsed event for the timer. 
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            PlayEvents();
        }

        private void PlayEvents()
        {
            // Open the midi port
            if (dev.IsOpen)
            {

                // Transform queued sounds into their midi values
                var midiValues = _drumSounds.Distinct(new MidiDrumScoreEqualityComparer())
                    .Select(n => new {MidiDrum = n.Item1.ToMidiDrum(), HighScore = n.Item2, ModelName = n.Item3 })
                    .ToList();
                
                // Play each value
                foreach (var midiValue in midiValues)
                {
                    Console.WriteLine($"{DateTime.Now:T}: {midiValue.HighScore:##.000}% \t|\t {midiValue.MidiDrum}");

                    

                }

                var distinctDrums = midiValues.Select(m => m.MidiDrum).Distinct();

                foreach (var distinctDrum in distinctDrums)
                {
                    // Transform the value to the correct note
                    var note = MidiUtility.NoteIdToNote((byte)distinctDrum, true);
                    dev.Send(new MidiMessageNoteOn(note, 127, 10));
                }

                Thread.Sleep(10);

                foreach (var distinctDrum in distinctDrums)
                {

                    // Transform the value to the correct note
                    var note = MidiUtility.NoteIdToNote((byte)distinctDrum, true);
                    dev.Send(new MidiMessageNoteOff(note, 127, 10));
                }

                // Clear the sound queue
                _drumSounds.Clear();
            }
        }
    }
}
