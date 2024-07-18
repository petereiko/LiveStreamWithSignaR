using LiveStreamWithSignaR.Models;
using Microsoft.AspNetCore.SignalR;

namespace LiveStreamWithSignaR
{
    public interface ICameraService
    {
        Task StartStreaming(IHubContext<LiveStreamHub> hubContext);
        void StopStreaming();
    }
}