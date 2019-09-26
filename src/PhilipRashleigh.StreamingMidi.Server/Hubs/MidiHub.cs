using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PhilipRashleigh.StreamingMidi.Core;
using PhilipRashleigh.StreamingMidi.Server.Clients;

namespace PhilipRashleigh.StreamingMidi.Server.Hubs
{
    public class MidiHub : Hub<IMidiReceiver>
    {
        private readonly ILogger<MidiHub> _logger;

        public MidiHub(ILogger<MidiHub> logger)
        {
            _logger = logger;
        }
        
        public async Task Send(IAsyncEnumerable<string> stream)
        {
            await foreach (var message in stream)
            {
                //On seperate thread so as to minimise additional latency of logging
                Task.Run(() =>
                {
                    var midiMessage = MidiMessage.Parse(message);
                    _logger.Log(LogLevel.Debug, $"{midiMessage.Command}, note: {midiMessage.Note}");
                });
            }
        }
    }
}