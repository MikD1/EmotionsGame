using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace EmotionsGame
{
    public sealed partial class FinishDetailsPage : Page
    {
        public FinishDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            GameResult gameResult = (GameResult)e.Parameter;
            EmotionTitle.Text = gameResult.Emotion.ToString();
            ShowPlayer(gameResult.Player1, Player1PhotoView, Player1ScoresView);
            ShowPlayer(gameResult.Player2, Player2PhotoView, Player2ScoresView);
        }

        private void ShowPlayer(PlayerResult player, Image photoView, StackPanel scoresView)
        {
            if (player == null)
            {
                photoView.Source = null;
                scoresView.Children.Clear();
            }
            else
            {
                WriteableBitmap photo = PlayerPhotoHelper.CropFrame(player);
                photoView.Source = photo;

                foreach (KeyValuePair<EmotionVariants, float> score in player.AllScores)
                {
                    scoresView.Children.Add(new TextBlock
                    {
                        Text = $"{score.Key.ToString()}: {score.Value}",
                        FontSize = 28,
                        Foreground = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255))
                    });
                }
            }
        }

        private void BackButtonOnClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
