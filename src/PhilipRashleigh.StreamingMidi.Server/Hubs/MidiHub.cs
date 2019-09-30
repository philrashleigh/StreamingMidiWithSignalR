using System;
using System.Collections.Generic;
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
        private readonly ILogger<MidiHub> _logger;
        private readonly MidiFileManager _midiFileManager;

        public MidiHub(ILogger<MidiHub> logger, MidiFileManager midiFileManager)
        {
            _logger = logger;
            _midiFileManager = midiFileManager;
        }

        public IEnumerable<string> GetRecordings() => _midiFileManager.GetMidiFileList();

        public async IAsyncEnumerable<string> Receive(string name, CancellationToken cancellationToken)
        {
            var recording = _midiFileManager.ReadMidiFileAsEventsAndMillisecondDelta(name);

            foreach (var (message, delta) in recording)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                
                await Task.Delay(TimeSpan.FromMilliseconds(delta), cancellationToken);
                
                _logger.Log(LogLevel.Debug, $"Playing - {message.Command}: {message.FriendlyNoteName}");
                
                yield return message.ToString();
            }
        }
        
        public async Task Send(string name, IAsyncEnumerable<string> stream)
        {
            _logger.Log(LogLevel.Information, "Recording started");

            var midiBuilder = new MidiBuilder();

            await foreach (var item in stream)
            {
                var message = MidiMessage.Parse(item);
                
                _logger.Log(LogLevel.Debug, $"Recording - {message.Command}: {message.FriendlyNoteName}");
                
                midiBuilder.Add(message);
            }

            _logger.Log(LogLevel.Information, "Recording complete");
            
            _midiFileManager.Save(midiBuilder, name);
            
            _logger.Log(LogLevel.Information, $"File saved as {name}.mid");
        }
    }
}