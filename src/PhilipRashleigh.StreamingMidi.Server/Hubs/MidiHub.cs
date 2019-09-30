using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PhilipRashleigh.StreamingMidi.Core;
using PhilipRashleigh.StreamingMidi.Server.Clients;
using PhilipRashleigh.StreamingMidi.Server.Other;

// ReSharper disable UnusedMember.Global
namespace PhilipRashleigh.StreamingMidi.Server.Hubs
{
    public class MidiHub : Hub<IMidiReceiver>
    {
        private readonly ILogger<MidiHub> _logger;
        private const string ReceiversGroup = "Receivers";
        
        public MidiHub(ILogger<MidiHub> logger, AppState state)
        {
            _logger = logger;
        }

        public void RegisterReceiver()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, ReceiversGroup);
        }
        
        public async Task Send(IAsyncEnumerable<string> stream)
        {
            _logger.Log(LogLevel.Information, "Sender Connected");

            await foreach (var message in stream)
            {
                await Clients.Group(ReceiversGroup).Receive(message);
                
                //On separate thread so as to minimise additional latency of logging preventing additional message processing
                #pragma warning disable 4014
                                Task.Run(() =>
                                {
                                    var midiMessage = MidiMessage.Parse(message);
                                    _logger.Log(LogLevel.Debug, $"{midiMessage.Command}, note: {midiMessage.Note}");
                                });
                #pragma warning restore 4014
            }
        }
    }
}