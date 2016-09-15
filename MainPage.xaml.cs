using Windows.UI.Xaml.Controls;

namespace EmotionsGame
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            GameView view = new GameView();
            view.Dispatcher = Dispatcher;
            view.Capture = CaptureView;
            view.Canvas = CanvasView;
            view.Player1Score = Player1ScoreView;
            view.Player2Score = Player2ScoreView;

            _game = new Game(view);
            _game.Run();
        }

        private readonly Game _game;
    }
}
