using LiveStreamWithSignaR.Models;
using Microsoft.AspNetCore.SignalR;
using OpenCvSharp;
using System;

namespace LiveStreamWithSignaR
{
    public class CameraService : ICameraService
    {
        private readonly string _cameraUrl = "rtsp://admin:Securityr%26d1@192.168.8.143/Streaming/Channels/101";
        private CancellationTokenSource _cancellationTokenSource;


        public CameraService()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartStreaming(IHubContext<LiveStreamHub> hubContext)
        {
            using var capture = new VideoCapture(_cameraUrl);
            if (!capture.IsOpened())
            {
                throw new Exception("Camera connection failed.");
            }

            var cancellationToken = _cancellationTokenSource.Token;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    using var frame = new Mat();
                    capture.Read(frame);
                    if (frame.Empty())
                    {
                        continue;
                    }

                    // Resize the frame for lower resolution
                    Cv2.Resize(frame, frame, new Size(320, 240)); // Example: reduce to 320x240

                    // Set JPEG compression parameters
                    var compressionParams = new ImageEncodingParam[]
                    {
                    new ImageEncodingParam(ImwriteFlags.JpegQuality, 90) // Reduce quality to 50 (0-100 scale)
                    };

                    // Convert frame to compressed JPEG bytes
                    var imageBytes = frame.ImEncode(".jpg", compressionParams);
                    


                    // Convert to base64 string
                    string base64Image = System.Convert.ToBase64String(imageBytes);

                    // Send the frame to clients
                    await hubContext.Clients.All.SendAsync("ReceiveFrame", base64Image, cancellationToken);

                    // Adjust the delay for the desired frame rate (e.g., 30 FPS)
                    await Task.Delay(33);
                }
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation (if needed)
            }
        }

        public void StopStreaming()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource(); // Reset for future use
        }
    }
}
