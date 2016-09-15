using System;

namespace EmotionsGame
{
    public static class Config
    {
        public const string EmotionServiceKey = "";
        public static readonly TimeSpan FacesDetectionInterval = TimeSpan.FromMilliseconds(60);
        public static readonly TimeSpan EmotionsDetectionInterval = TimeSpan.FromMilliseconds(1000);
    }
}
