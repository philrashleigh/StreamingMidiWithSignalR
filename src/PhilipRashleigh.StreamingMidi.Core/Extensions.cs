using Melanchall.DryWetMidi.Smf;

namespace PhilipRashleigh.StreamingMidi.Core
{
    public static class Extensions
    {
        public static MidiMessage ToMidiMessage(this NoteEvent noteEvent)
        {
            var command = noteEvent is NoteOnEvent ? MidiCommand.NoteOn : MidiCommand.NoteOff;
            
            return new MidiMessage(command, noteEvent.NoteNumber, noteEvent.Velocity);
        }
    }
}