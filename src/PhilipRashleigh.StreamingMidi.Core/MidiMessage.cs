using System;
using System.Linq;
using Melanchall.DryWetMidi.Common;

namespace PhilipRashleigh.StreamingMidi.Core
{
    public class MidiMessage
    {
        private static string[] _noteName = {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};

        //see https://ccrma.stanford.edu/~craig/articles/linuxmidi/misc/essenmidi.html for usage help
        public MidiCommand Command { get; private set; }
        public SevenBitNumber Note { get; private set; }
        public SevenBitNumber Velocity { get; private set; }

        public static MidiMessage Parse(string message)
        {
            var arr = message.Split(',');

            return new MidiMessage
            {
                Command = (MidiCommand) Enum.Parse(typeof(MidiCommand), arr[0]),
                Note = SevenBitNumber.Parse(arr[1]),
                Velocity = SevenBitNumber.Parse(arr[2])
            };
        }

        public string FriendlyNoteName
        {
            get
            {
                var octave = Note / 12 - 1;
                var noteIndex = Note % 12;

                return $"{_noteName[noteIndex]}{octave}";
            }
        }
    }
}