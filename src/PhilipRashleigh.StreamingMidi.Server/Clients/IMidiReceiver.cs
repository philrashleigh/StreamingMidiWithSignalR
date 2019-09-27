namespace PhilipRashleigh.StreamingMidi.Server.Clients
{
    public interface IMidiReceiver
    {
        void Receive(string message);
    }
}