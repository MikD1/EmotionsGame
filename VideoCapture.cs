using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.Core;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml.Controls;

namespace EmotionsGame
{
    public class VideoCapture
    {
        public VideoCapture()
        {
            _capture = new MediaCapture();
        }

        public event Action<IReadOnlyCollection<BitmapBounds>> FacesDetected;

        public VideoEncodingProperties VideoProperties { get; private set; }

        public async Task<SoftwareBitmap> CaptureFrame()
        {
            VideoFrame destination = new VideoFrame(BitmapPixelFormat.Bgra8, (int)VideoProperties.Width, (int)VideoProperties.Height);
            VideoFrame frame = await _capture.GetPreviewFrameAsync(destination);
            return frame.SoftwareBitmap;
        }

        public async Task Initialize(CaptureElement captureView, TimeSpan facesDetectionUpdateInterval)
        {
            DeviceInformationCollection cameras = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            DeviceInformation camera = cameras.First();

            await InitializeCapture(captureView, camera.Id);
            await InitializeFaceDetector(facesDetectionUpdateInterval);
            await _capture.StartPreviewAsync();
        }
        private async Task InitializeCapture(CaptureElement captureView, string cameraId)
        {
            MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings() { VideoDeviceId = cameraId };
            await _capture.InitializeAsync(settings);
            captureView.Source = _capture;

            IMediaEncodingProperties properties = _capture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
            VideoProperties = properties as VideoEncodingProperties;
        }
        private async Task InitializeFaceDetector(TimeSpan facesDetectionUpdateInterval)
        {
            FaceDetectionEffectDefinition effect = new FaceDetectionEffectDefinition();
            effect.SynchronousDetectionEnabled = false;
            effect.DetectionMode = _faceDetectionMode;

            _faceDetector = (FaceDetectionEffect)(await _capture.AddVideoEffectAsync(effect, MediaStreamType.VideoPreview));
            _faceDetector.FaceDetected += HandleFaceDetector;
            _faceDetector.DesiredDetectionInterval = facesDetectionUpdateInterval;
            _faceDetector.Enabled = true;
        }

        private void HandleFaceDetector(FaceDetectionEffect sender, FaceDetectedEventArgs args)
        {
            if (FacesDetected != null)
            {
                List<BitmapBounds> result = new List<BitmapBounds>();
                foreach (DetectedFace face in args.ResultFrame.DetectedFaces)
                {
                    result.Add(face.FaceBox);
                }

                FacesDetected(result);
            }
        }

        private FaceDetectionEffect _faceDetector;
        private readonly MediaCapture _capture;

        private readonly FaceDetectionMode _faceDetectionMode = FaceDetectionMode.HighQuality;
    }
}
