using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EmotionsGame
{
    public sealed partial class GamePage : Page
    {
        public GamePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            GameOptions options = (GameOptions)e.Parameter;
            EmotionTitle.Text = options.EmotionVariant.ToString();

            GameView view = new GameView();
            view.Dispatcher = Dispatcher;
            view.Capture = CaptureView;
            view.Canvas = CanvasView;
            view.Player1Score = Player1ScoreView;
            view.Player2Score = Player2ScoreView;

            _game = new Game(view);
            _game.GameFinished += OnGameFinished;
            _game.Run(options.EmotionVariant);
        }

        private void OnGameFinished(GameResult result)
        {
            Frame.Navigate(typeof(FinishPage), result);
        }

        private Game _game;
    }
}
