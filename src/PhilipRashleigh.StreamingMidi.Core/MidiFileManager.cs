using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melanchall.DryWetMidi.Smf;

namespace PhilipRashleigh.StreamingMidi.Core
{
    public class MidiFileManager
    {
        private readonly string _directory;

        public MidiFileManager(string directory)
        {
            _directory = directory;
        }

        public IEnumerable<string> GetMidiFileList() =>
            Directory.GetFiles(_directory, "*.mid").Select(Path.GetFileNameWithoutExtension);

        public void Save(MidiBuilder midiBuilder, string name)
        {
            var file = midiBuilder.Build();
            file.Write(Path.Combine(_directory, $"{Path.GetFileNameWithoutExtension(name)}.mid"), true, MidiFileFormat.SingleTrack);
        }
    }
}