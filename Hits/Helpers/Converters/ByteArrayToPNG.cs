using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hits.Helpers.Converters
{
    public class ByteArrayToPNG : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(@"C:\Hits\Hits\Views\Images\blankUser.png");
                System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                return bitmapSource;
            }

            System.Windows.Media.Imaging.BitmapImage biImg = new System.Windows.Media.Imaging.BitmapImage();
            MemoryStream ms = new MemoryStream((byte[])value);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            return biImg as ImageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(value as BitmapSource));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
