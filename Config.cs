using System;

namespace EmotionsGame
{
    public static class Config
    {
        public const string ResultsFolder = "EmotionsGame";
        public const string EmotionServiceKey = "";
        public static readonly TimeSpan FacesDetectionInterval = TimeSpan.FromMilliseconds(60);
        public static readonly TimeSpan EmotionsDetectionInterval = TimeSpan.FromMilliseconds(1000);
        public static readonly TimeSpan GameRoundInterval = TimeSpan.FromSeconds(10);
    }
}
