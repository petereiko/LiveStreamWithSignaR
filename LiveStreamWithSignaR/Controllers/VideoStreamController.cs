using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LiveStreamWithSignaR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoStreamController : ControllerBase
    {
        [HttpGet("stream")]
        public IActionResult StreamVideo()
        {
            var stream = GetVideoStream("rtsp://admin:Securityr%26d1@192.168.8.143:554/Streaming/Channels/101");
            return new FileStreamResult(stream, "video/mp4");
        }
        //ffmpeg -i rtsp://admin:Securityr%26d1@192.168.8.143:554/Streaming/Channels/101 -f mp4 -codec:v copy -an -movflags frag_keyframe+empty_moov - | ffplay -
        private Stream GetVideoStream(string rtspUrl)
        {
            var ffmpegPath = "ffmpeg"; // Make sure ffmpeg is in your system PATH

            var startInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments= $"-i {rtspUrl} -f mp4 -codec:v copy -an -movflags frag_keyframe+empty_moov - | ffplay -",
                //Arguments = $"-i {rtspUrl} -f mp4 -codec:v copy -an -movflags frag_keyframe+empty_moov pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            return process.StandardOutput.BaseStream;
        }
    }
}
