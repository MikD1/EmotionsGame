using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace EmotionsGame
{
    public sealed partial class FinishPage : Page
    {
        public FinishPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _gameResult = (GameResult)e.Parameter;
            PlayerResult winner = _gameResult.GetWinner();

            WriteableBitmap photo = PlayerPhotoHelper.CropFrame(winner);
            ShowWinner(photo, _gameResult.Emotion, winner.Score);
            await SaveWinner(photo, _gameResult.Emotion, winner.Score);
        }

        private void ShowWinner(WriteableBitmap photo, EmotionVariants emotion, float score)
        {
            EmotionTitle.Text = emotion.ToString();
            ScoreView.Text = score.ToString();
            ImgWinner.Source = photo;
        }
        private async Task SaveWinner(WriteableBitmap photo, EmotionVariants emotion, float score)
        {
            string filename = $"{DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss")}-{emotion.ToString()}-{score.ToString()}.jpeg";

            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync(Config.ResultsFolder, CreationCollisionOption.OpenIfExists);
            StorageFile file = await folder.CreateFileAsync(filename);

            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await photo.ToStreamAsJpeg(stream);
            }
        }

        private void ButtonInfoOnClick(object sender, RoutedEventArgs e)
        {
            if (_gameResult != null)
            {
                Frame.Navigate(typeof(FinishDetailsPage), _gameResult);
            }
        }
        private void ButtonNextOnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartPage));
        }

        private GameResult _gameResult;
    }
}
