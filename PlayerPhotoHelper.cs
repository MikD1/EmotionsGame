using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Imaging;

namespace EmotionsGame
{
    public static class PlayerPhotoHelper
    {
        public static WriteableBitmap CropFrame(PlayerResult result)
        {
            Rect rect = new Rect();
            rect.X = Math.Max(result.FaceRectangle.Left - 100, 0);
            rect.Y = Math.Max(result.FaceRectangle.Top - 100, 0);
            rect.Width = result.FaceRectangle.Width + 200;
            rect.Height = result.FaceRectangle.Height + 200;

            if (rect.X + rect.Width > result.Frame.PixelWidth)
            {
                rect.Width = result.Frame.PixelWidth - rect.X;
            }

            if (rect.Y + rect.Height > result.Frame.PixelHeight)
            {
                rect.Height = result.Frame.PixelHeight - rect.Y;
            }

            WriteableBitmap bitmap = new WriteableBitmap(result.Frame.PixelWidth, result.Frame.PixelHeight);
            result.Frame.CopyToBuffer(bitmap.PixelBuffer);
            return bitmap.Crop(rect);
        }
    }
}
