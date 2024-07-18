using LiveStreamWithSignaR.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace LiveStreamWithSignaR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IHubContext<VideoHub> _hubContext;

        public VideoController(IHubContext<VideoHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [EnableCors("***")]
        [HttpGet("start")]
        public IActionResult StartStream()
        {
            Task.Run(() => StartFFmpeg());
            return Ok();
        }

        [EnableCors("***")]
        [HttpGet("string")]
        public IActionResult GetString()
        {
            return Ok("Api works");
        }

        //rtsp://192.168.8.143:554/Streaming/channels/1
        private void StartFFmpeg()
        {

            //            Arguments = "-i rtsp://username:password@camera-ip-address:554/Streaming/Channels/101 -f webm -c:v libvpx -c:a libvorbis -",

            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = "-i rtsp://admin:Securityr%26d1@192.168.8.143:554/Streaming/Channels/101 -f webm -c:v libvpx -c:a libvorbis -",
                //Arguments = "-i rtsp://admin:Securityr&d1@192.168.8.143:554/Streaming/Channels/1 -f mpegts -codec:v mpeg1video -s 640x480 -b:v 800k -r 30 -",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                var buffer = new byte[4096];
                var readBytes = 0;

                while ((readBytes = process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    _hubContext.Clients.All.SendAsync("ReceiveVideo", buffer, readBytes);
                }
            }
        }
    }
}
