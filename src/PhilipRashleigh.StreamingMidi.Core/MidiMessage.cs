using System.Linq;

namespace PhilipRashleigh.StreamingMidi.Core
{
    public class MidiMessage
    {
        public MidiCommand Command { get; set; }
        public byte Note { get; set; }
        public float Velocity { get; set; }

        public static MidiMessage Parse(string message)
        {
            var arr = message.Split(',').Select(byte.Parse).ToArray();

            return new MidiMessage
            {
                Command = (MidiCommand)arr[0],
                Note = arr[1],
                Velocity = arr[2] / 127f
            };
        }
    }
}