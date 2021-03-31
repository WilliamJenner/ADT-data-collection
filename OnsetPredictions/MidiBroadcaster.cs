using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using M;

namespace OnsetPredictions
{
    public class MidiBroadcaster
    {
        public bool Broadcasting = false;
        
        private System.Timers.Timer Timer;
        private ConcurrentBag<DrumSoundType> _drumSounds;
        private MidiOutputDevice dev = MidiDevice.Outputs[3];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="broadcasting"></param>
        /// <param name="interval">Interval for the internal timer (milliseconds). Default is 100 </param>
        public MidiBroadcaster(bool broadcasting, double interval = 100)
        {
            Broadcasting = broadcasting;
            _drumSounds = new ConcurrentBag<DrumSoundType>();
            SetTimer(interval);
            dev.Open();
        }

        public void Broadcast(DrumSoundType drumType)
        {
            _drumSounds.Add(drumType);
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
                var midiValues = _drumSounds.Distinct().Select(n => n.ToMidiDrum()).ToList();

                // Play each value
                foreach (var midiValue in midiValues)
                {
                    Console.WriteLine(midiValue);

                    // Transform the value to the correct note
                    var note = MidiUtility.NoteIdToNote((byte)midiValue, true);
                    dev.Send(new MidiMessageNoteOn(note, 127, 10));

                }

                Thread.Sleep(10);

                foreach (var midiValue in midiValues)
                {

                    // Transform the value to the correct note
                    var note = MidiUtility.NoteIdToNote((byte)midiValue, true);
                    dev.Send(new MidiMessageNoteOff(note, 127, 10));
                }

                // Clear the sound queue
                _drumSounds.Clear();
            }
        }
    }
}
