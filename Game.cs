﻿using Microsoft.ProjectOxford.Emotion;
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
using System.Linq;

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

            _gameRoundTimer = new DispatcherTimer();
            _gameRoundTimer.Interval = Config.GameRoundInterval;
            _gameRoundTimer.Tick += OnGameRoundTimerTick;

            _emotionService = new EmotionServiceClient(Config.EmotionServiceKey);
        }

        public event Action<PlayerResult> GameFinished;

        public async void Run(EmotionVariants trackedEmotion)
        {
            _trackedEmotion = trackedEmotion;
            _isGameFinished = false;
            _highScore = null;

            await _capture.Initialize(_view.Capture, Config.FacesDetectionInterval);
            _detectEmotionsTimer.Start();
            _gameRoundTimer.Start();
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
                if (_isGameFinished)
                {
                    return;
                }

                SoftwareBitmap frame = await _capture.CaptureFrame();
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                    encoder.SetSoftwareBitmap(frame);
                    await encoder.FlushAsync();

                    stream.Seek(0L);
                    Emotion[] emotions = await _emotionService.RecognizeAsync(stream.AsStream());
                    List<Emotion> sortedEmotions = emotions.OrderBy(i => i.FaceRectangle.Left).ToList();

                    Emotion p1Emotion = sortedEmotions.Count > 0 ? emotions[0] : null;
                    Emotion p2Emotion = sortedEmotions.Count > 1 ? emotions[1] : null;

                    if (p1Emotion != null)
                    {
                        PlayerResult p1Result = new PlayerResult(frame, p1Emotion.FaceRectangle, _trackedEmotion, GetScore(p1Emotion));
                        _view.Player1Score.Text = p1Result.Score.ToString();
                        UpdateHighScore(p1Result);
                    }

                    if (p2Emotion != null)
                    {
                        PlayerResult p2Result = new PlayerResult(frame, p2Emotion.FaceRectangle, _trackedEmotion, GetScore(p2Emotion));
                        _view.Player2Score.Text = p2Result.Score.ToString();
                        UpdateHighScore(p2Result);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO
            }
        }
        private float GetScore(Emotion emotion)
        {
            switch (_trackedEmotion)
            {
                case EmotionVariants.Anger:
                    return emotion.Scores.Anger;
                case EmotionVariants.Contempt:
                    return emotion.Scores.Contempt;
                case EmotionVariants.Disgust:
                    return emotion.Scores.Disgust;
                case EmotionVariants.Fear:
                    return emotion.Scores.Fear;
                case EmotionVariants.Happiness:
                    return emotion.Scores.Happiness;
                case EmotionVariants.Neutral:
                    return emotion.Scores.Neutral;
                case EmotionVariants.Sadness:
                    return emotion.Scores.Sadness;
                case EmotionVariants.Surprise:
                    return emotion.Scores.Surprise;
                default:
                    return 0;
            }
        }

        private void UpdateHighScore(PlayerResult result)
        {
            if (_highScore == null || _highScore < result)
            {
                _highScore = result;
            }
        }

        private async void OnCaptureFacesDetected(IReadOnlyCollection<BitmapBounds> faces)
        {
            await _view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _view.Canvas.Children.Clear();
                foreach (BitmapBounds face in faces)
                {
                    HighlightFace(face);
                }
            });
        }
        private async void OnDetectEmotionsTimerTick(object sender, object e)
        {
            _detectEmotionsTimer.Stop();
            await DetectEmotions();

            if (!_isGameFinished)
            {
                _detectEmotionsTimer.Start();
            }
        }
        private void OnGameRoundTimerTick(object sender, object e)
        {
            _gameRoundTimer.Stop();
            _detectEmotionsTimer.Stop();
            _isGameFinished = true;

            _view.Capture.Source = null;
            _capture.Dispose();

            GameFinished?.Invoke(_highScore);
        }

        private bool _isGameFinished;
        private PlayerResult _highScore;
        private EmotionVariants _trackedEmotion;
        private readonly GameView _view;
        private readonly VideoCapture _capture;
        private readonly DispatcherTimer _detectEmotionsTimer;
        private readonly DispatcherTimer _gameRoundTimer;
        private readonly EmotionServiceClient _emotionService;
        private readonly SolidColorBrush _faceRectangleBrush = new SolidColorBrush(Color.FromArgb(128, 255, 255, 0));
    }
}
