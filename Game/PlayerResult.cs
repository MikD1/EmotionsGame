using Microsoft.ProjectOxford.Common;
using System.Collections.Generic;
using Windows.Graphics.Imaging;

namespace EmotionsGame
{
    public class PlayerResult
    {
        public PlayerResult(SoftwareBitmap frame, Rectangle faceRectangle, float score)
        {
            Frame = frame;
            FaceRectangle = faceRectangle;
            Score = score * 100f;

            AllScores = new Dictionary<EmotionVariants, float>();
        }

        public float Score { get; private set; }
        public Dictionary<EmotionVariants, float> AllScores { get; private set; }

        // TODO: Dispose
        public SoftwareBitmap Frame { get; private set; }

        public Rectangle FaceRectangle { get; private set; }

        public void AddScore(EmotionVariants emotion, float score)
        {
            AllScores[emotion] = score * 100f;
        }

        public static bool operator >(PlayerResult a, PlayerResult b)
        {
            return a.Score > b.Score;
        }
        public static bool operator <(PlayerResult a, PlayerResult b)
        {
            return a.Score < b.Score;
        }
    }
}
