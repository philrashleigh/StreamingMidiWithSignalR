using System.Collections.Generic;

namespace PhilipRashleigh.StreamingMidi.Server.Other
{
    public class AppState
    {
        public IAsyncEnumerable<string> Stream { get; set; }
    }
}