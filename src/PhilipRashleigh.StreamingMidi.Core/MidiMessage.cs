using System;
using Melanchall.DryWetMidi.Common;

namespace PhilipRashleigh.StreamingMidi.Core
{
    public class MidiMessage
    {
        private static string[] _noteName = {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};

        public MidiMessage(MidiCommand command, SevenBitNumber note, SevenBitNumber velocity)
        {
            Command = command;
            Note = note;
            Velocity = velocity;
        }
        
        //see https://ccrma.stanford.edu/~craig/articles/linuxmidi/misc/essenmidi.html for usage help
        public MidiCommand Command { get; }
        public SevenBitNumber Note { get; }
        public SevenBitNumber Velocity { get; }
        
        public string FriendlyNoteName
        {
            get
            {
                var octave = Note / 12 - 1;
                var noteIndex = Note % 12;

                return $"{_noteName[noteIndex]}{octave}";
            }
        }
        
        public static MidiMessage Parse(string message)
        {
            var arr = message.Split(',');

            return new MidiMessage((MidiCommand) Enum.Parse(typeof(MidiCommand), arr[0]), 
                SevenBitNumber.Parse(arr[1]),
                SevenBitNumber.Parse(arr[2]));
        }

        public override string ToString() => $"{(int) Command},{Note},{Velocity}";
    }
}