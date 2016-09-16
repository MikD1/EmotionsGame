using System;
using System.Threading.Tasks;
using Windows.Foundation;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            PlayerResult winner = e.Parameter as PlayerResult;
            if (winner != null)
            {
                WriteableBitmap photo = CropWinnerFrame(winner);
                ShowWinner(photo, winner.Emotion, winner.Score);
                SaveWinner(photo, winner.Emotion, winner.Score);
            }
        }

        private void ShowWinner(WriteableBitmap photo, EmotionVariants emotion, float score)
        {
            EmotionTitle.Text = emotion.ToString();
            ScoreView.Text = score.ToString();
            ImgWinner.Source = photo;
        }
        private async Task SaveWinner(WriteableBitmap photo, EmotionVariants emotion, float score)
        {
            string filename = $"{DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss")}-{emotion.ToString()}-{score.ToString()}";

            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync(Config.ResultsFolder, CreationCollisionOption.OpenIfExists);
            StorageFile file = await folder.CreateFileAsync(filename);

            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await photo.ToStreamAsJpeg(stream);
            }
        }

        private WriteableBitmap CropWinnerFrame(PlayerResult winner)
        {
            Rect rect = new Rect();
            rect.X = Math.Max(winner.FaceRectangle.Left - 100, 0);
            rect.Y = Math.Max(winner.FaceRectangle.Top - 100, 0);
            rect.Width = winner.FaceRectangle.Width + 200;
            rect.Height = winner.FaceRectangle.Height + 200;

            if (rect.X + rect.Width > winner.Frame.PixelWidth)
            {
                rect.Width = winner.Frame.PixelWidth - rect.X;
            }

            if (rect.Y + rect.Height > winner.Frame.PixelHeight)
            {
                rect.Height = winner.Frame.PixelHeight - rect.Y;
            }

            WriteableBitmap bitmap = new WriteableBitmap(winner.Frame.PixelWidth, winner.Frame.PixelHeight);
            winner.Frame.CopyToBuffer(bitmap.PixelBuffer);
            return bitmap.Crop(rect);
        }

        private void ButtonNextOnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartPage));
        }
    }
}
