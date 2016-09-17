using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace EmotionsGame
{
    public class GameView
    {
        public CoreDispatcher Dispatcher { get; set; }

        public CaptureElement Capture { get; set; }
        public Canvas Canvas { get; set; }
        public TextBlock Player1Score { get; set; }
        public TextBlock Player2Score { get; set; }
    }
}
