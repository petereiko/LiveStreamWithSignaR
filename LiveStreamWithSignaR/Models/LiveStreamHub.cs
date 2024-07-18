using Microsoft.AspNetCore.SignalR;

namespace LiveStreamWithSignaR.Models
{
    public class LiveStreamHub:Hub
    {
        public async Task SendFrame(byte[] frame)
        {
            await Clients.All.SendAsync("ReceiveFrame", frame);
        }
    }
}
