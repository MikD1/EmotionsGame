using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EmotionsGame
{
    public sealed partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void StartButtonOnClick(object sender, RoutedEventArgs e)
        {
            GameOptions options = new GameOptions();
            options.EmotionVariant = _selectedEmotion;

            Frame.Navigate(typeof(GamePage), options);
        }

        private void AngerChecked(object sender, RoutedEventArgs e)
        {
            _selectedEmotion = EmotionVariants.Anger;
        }
        private void ContemptChecked(object sender, RoutedEventArgs e)
        {
            _selectedEmotion = EmotionVariants.Contempt;
        }
        private void DisgustChecked(object sender, RoutedEventArgs e)
        {
            _selectedEmotion = EmotionVariants.Disgust;
        }
        private void FearChecked(object sender, RoutedEventArgs e)
        {
            _selectedEmotion = EmotionVariants.Fear;
        }
        private void HappinessChecked(object sender, RoutedEventArgs e)
        {
            _selectedEmotion = EmotionVariants.Happiness;
        }
        private void NeutralChecked(object sender, RoutedEventArgs e)
        {
            _selectedEmotion = EmotionVariants.Neutral;
        }
        private void SadnessChecked(object sender, RoutedEventArgs e)
        {
            _selectedEmotion = EmotionVariants.Sadness;
        }
        private void SurpriseChecked(object sender, RoutedEventArgs e)
        {
            _selectedEmotion = EmotionVariants.Surprise;
        }

        private EmotionVariants _selectedEmotion = EmotionVariants.Happiness;
    }
}
