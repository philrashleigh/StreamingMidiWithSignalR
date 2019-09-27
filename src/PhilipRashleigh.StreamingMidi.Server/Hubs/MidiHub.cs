using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PhilipRashleigh.StreamingMidi.Core;
using PhilipRashleigh.StreamingMidi.Server.Clients;

// ReSharper disable UnusedMember.Global
namespace PhilipRashleigh.StreamingMidi.Server.Hubs
{
    public class MidiHub : Hub<IMidiReceiver>
    {
        private readonly IDictionary<string, IAsyncEnumerable<string>> _streams;
        
        private readonly ILogger<MidiHub> _logger;

        public MidiHub(ILogger<MidiHub> logger)
        {
            _logger = logger;
            _streams = new Dictionary<string, IAsyncEnumerable<string>>();
        }

        public async IAsyncEnumerable<string> Receive(CancellationToken cancellationToken)
        {
            foreach (var stream in _streams.Values)
            {
                await foreach (var message in stream.WithCancellation(cancellationToken))
                {
                    yield return message;
                }
            }
        }
        
        public async Task Send(IAsyncEnumerable<string> stream)
        {
            _streams.Add(Context.ConnectionId, stream);
            
            await foreach (var message in stream)
            {
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

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_streams.ContainsKey(Context.ConnectionId))
            {
                _streams.Remove(Context.ConnectionId);
            }
            
            return base.OnDisconnectedAsync(exception);
        }
    }
}