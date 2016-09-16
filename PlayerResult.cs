using Microsoft.ProjectOxford.Common;
using System;
using Windows.Graphics.Imaging;

namespace EmotionsGame
{
    public class PlayerResult : IDisposable
    {
        public PlayerResult(SoftwareBitmap frame, Rectangle faceRectangle, EmotionVariants emotion, float score)
        {
            Frame = frame;
            FaceRectangle = faceRectangle;
            Emotion = emotion;
            Score = score * 100f;
        }

        public SoftwareBitmap Frame { get; private set; }
        public Rectangle FaceRectangle { get; private set; }
        public EmotionVariants Emotion { get; private set; }
        public float Score { get; private set; }

        public static bool operator >(PlayerResult a, PlayerResult b)
        {
            return a.Score > b.Score;
        }
        public static bool operator <(PlayerResult a, PlayerResult b)
        {
            return a.Score < b.Score;
        }

        public void Dispose()
        {
            Frame.Dispose();
        }
    }
}
