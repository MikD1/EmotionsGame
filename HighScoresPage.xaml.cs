using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EmotionsGame
{
    public sealed partial class HighScoresPage : Page
    {
        public HighScoresPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _viewModel = new HighScoresPageViewModel();
            DataContext = _viewModel;

            await _viewModel.Initialize();
        }

        private void BackButtonOnClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private HighScoresPageViewModel _viewModel;
    }
}
