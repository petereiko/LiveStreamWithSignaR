using LiveStreamWithSignaR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LiveStreamWithSignaR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        private readonly IHubContext<LiveStreamHub> _hubContext;
        private readonly ICameraService _cameraService;

        public StreamController(IHubContext<LiveStreamHub> hubContext, ICameraService cameraService)
        {
            _hubContext = hubContext;
            _cameraService = cameraService;
        }

        [HttpGet("start")]
        public IActionResult Start()
        {
            _ = Task.Run(() => _cameraService.StartStreaming(_hubContext));
            return Ok("Streaming started.");
        }

        [HttpGet("stop")]
        public IActionResult Stop()
        {
            _cameraService.StopStreaming();
            return Ok("Streaming stopped.");
        }


        [HttpGet("textconnectivity")]
        public IActionResult TestConnectivity()
        {
            return Ok("Connection successful");
        }
    }
}
