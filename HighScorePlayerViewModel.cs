using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace EmotionsGame
{
    public class HighScorePlayerViewModel
    {
        public static async Task<HighScorePlayerViewModel> CreateAsycn(StorageFile file)
        {
            HighScorePlayerViewModel result = new HighScorePlayerViewModel();

            string[] parts = file.DisplayName.Split('-');
            if (parts.Length != 3)
            {
                return null;
            }

            result.Emotion = parts[1];
            result.Score = parts[2];

            BitmapImage bitmap = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
            bitmap.SetSource(stream);
            result.Photo = bitmap;

            return result;
        }

        public ImageSource Photo { get; private set; }
        public string Emotion { get; set; }
        public string Score { get; set; }
    }
}
