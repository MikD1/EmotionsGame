namespace EmotionsGame
{
    public class GameResult
    {
        public GameResult(EmotionVariants emotion)
        {
            Emotion = emotion;
        }

        public EmotionVariants Emotion { get; private set; }
        public PlayerResult Player1 { get; set; }
        public PlayerResult Player2 { get; set; }

        public PlayerResult GetWinner()
        {
            if (Player1 == null && Player2 == null)
            {
                return null;
            }

            if (Player1 == null)
            {
                return Player2;
            }

            if (Player2 == null)
            {
                return Player1;
            }

            return Player1 > Player2 ? Player1 : Player2;
        }
    }
}
