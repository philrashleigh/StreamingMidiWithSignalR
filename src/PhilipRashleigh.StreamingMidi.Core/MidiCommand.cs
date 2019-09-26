namespace PhilipRashleigh.StreamingMidi.Core
{
    public enum MidiCommand : byte
    {
        NoteOff = 0x80,
        NoteOn = 0x90,
        AfterTouch = 0xA0,
        ContinuousController = 0xB0,
        PatchChange = 0xC0,
        ChannelPressure = 0xD0,
        PitchBend = 0xE0
    };
}