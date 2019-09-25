using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PhilipRashleigh.StreamingMidi.Server.Clients;

namespace PhilipRashleigh.StreamingMidi.Server.Hubs
{
    public class MidiHub : Hub<IMidiReceiver>
    {
        public async Task Send(IAsyncEnumerable<string> stream)
        {
            await foreach (var item in stream)
            {

            }
        }
    }
}