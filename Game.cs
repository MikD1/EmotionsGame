using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace EmotionsGame
{
    public class Game
    {
        public Game(GameView view)
        {
            _view = view;

            _capture = new VideoCapture();
            _capture.FacesDetected += OnCaptureFacesDetected;

            _detectEmotionsTimer = new DispatcherTimer();
            _detectEmotionsTimer.Interval = Config.EmotionsDetectionInterval;
            _detectEmotionsTimer.Tick += OnDetectEmotionsTimerTick;

            _emotionService = new EmotionServiceClient(Config.EmotionServiceKey);
        }

        public async void Run()
        {
            await _capture.Initialize(_view.Capture, Config.FacesDetectionInterval);
            _detectEmotionsTimer.Start();
        }

        private void HighlightFace(BitmapBounds face)
        {
            double x = _view.Canvas.ActualWidth / _capture.VideoProperties.Width;
            double y = _view.Canvas.ActualHeight / _capture.VideoProperties.Height;

            Rectangle rectangle = new Rectangle();
            rectangle.Margin = new Thickness(x * face.X, y * face.Y, 0, 0);
            rectangle.Width = x * face.Width;
            rectangle.Height = y * face.Height;
            rectangle.Stroke = _faceRectangleBrush;
            rectangle.StrokeThickness = 3;
            rectangle.VerticalAlignment = VerticalAlignment.Top;
            rectangle.HorizontalAlignment = HorizontalAlignment.Left;

            _view.Canvas.Children.Add(rectangle);
        }
        private async Task DetectEmotions()
        {
            try
            {
                using (SoftwareBitmap frame = await _capture.CaptureFrame())
                {
                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                        encoder.SetSoftwareBitmap(frame);
                        await encoder.FlushAsync();

                        stream.Seek(0L);
                        Emotion[] emotions = await _emotionService.RecognizeAsync(stream.AsStream());
                        //foreach (Emotion e in emotions)
                        //{
                        //}

                        if (emotions.Length > 0)
                        {
                            _view.Player1Score.Text = emotions[0].Scores.Happiness.ToString();
                        }

                        if (emotions.Length > 1)
                        {
                            _view.Player2Score.Text = emotions[1].Scores.Happiness.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO
            }
        }

        private async void OnCaptureFacesDetected(IReadOnlyCollection<BitmapBounds> faces)
        {
            _detectedFaces = faces;
            await _view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _view.Canvas.Children.Clear();
                foreach (BitmapBounds face in _detectedFaces)
                {
                    HighlightFace(face);
                }
            });
        }
        private async void OnDetectEmotionsTimerTick(object sender, object e)
        {
            if (_detectedFaces == null || _detectedFaces.Count == 0)
            {
                return;
            }

            _detectEmotionsTimer.Stop();
            await DetectEmotions();
            _detectEmotionsTimer.Start();
        }

        private IReadOnlyCollection<BitmapBounds> _detectedFaces;
        private readonly GameView _view;
        private readonly VideoCapture _capture;
        private readonly DispatcherTimer _detectEmotionsTimer;
        private readonly EmotionServiceClient _emotionService;
        private readonly SolidColorBrush _faceRectangleBrush = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
    }
}
