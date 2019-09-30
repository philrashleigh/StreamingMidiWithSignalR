using System;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;

namespace PhilipRashleigh.StreamingMidi.Core
{
    public class MidiBuilder
    {
        private readonly TrackChunk _trackChunk;
        private DateTime? _deltaStartTime; 

        public MidiBuilder()
        {
            _trackChunk = new TrackChunk();
        }
        
        public void Add(MidiMessage message)
        {
            var midiEvent = message.Command switch
            {
                MidiCommand.NoteOn => (MidiEvent) new NoteOnEvent(message.Note, message.Velocity),
                MidiCommand.NoteOff => new NoteOffEvent(message.Note, message.Velocity),
                _ => null
            };

            if (midiEvent == null)
            {
                return;
            }
            
            midiEvent.DeltaTime = CalculateDelta();
                
            _trackChunk.Events.Add(midiEvent);
        }

        private int CalculateDelta()
        {
            var now = DateTime.Now;
            
            //We're doing 1000 ticks to a beat, and 60 BPM such that 1 tick == 1 millisecond
            var deltaInMilliSeconds = _deltaStartTime.HasValue ? (now - _deltaStartTime.Value).Milliseconds : 0;

            _deltaStartTime = now;
            
            return deltaInMilliSeconds;
        }

        public MidiFile Build()
        {
            var midiFile = new MidiFile(_trackChunk);
            
            //1000 milliseconds in a second so using this should allow for timing with millisecond deltas
            var tempoMap = TempoMap.Create(new TicksPerQuarterNoteTimeDivision(1000), Tempo.FromBeatsPerMinute(60));
            
            midiFile.ReplaceTempoMap(tempoMap);
            
            return midiFile;
        }
    }
}