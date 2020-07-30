using System;
using System.Windows;

namespace Hits.Helpers
{
    public static class BitmapImage
    {
        public static System.Windows.Media.Imaging.BitmapSource GetBitmapSource(System.Drawing.Bitmap _image)
        {
            System.Drawing.Bitmap bitmap = _image;
            System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            return bitmapSource;
        }
    }
}
