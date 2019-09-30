using System.Threading.Tasks;

namespace PhilipRashleigh.StreamingMidi.Server.Clients
{
    public interface IMidiReceiver
    {
        Task Receive(string message);
    }
}