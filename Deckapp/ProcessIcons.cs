using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Deckapp
{
    internal class ProcessIcons
    {
        public static void appIcons()
        {
            foreach (var s in MainWindow.Sessions)
            {
                Process p = Process.GetProcessById((int)s.ProcessID);
                string filename = p.MainModule.FileName;
                Icon appIcon = Icon.ExtractAssociatedIcon(filename);


            }
        }
        public static BitmapImage getIconFromProsses(int id)
        {
            Process process = Process.GetProcessById(id);
            string filename = process.MainModule.FileName;
            Icon appIcon = Icon.ExtractAssociatedIcon(filename);
            Bitmap resized = new Bitmap(appIcon.ToBitmap(), new Size(64, 64));
            toGrayscale(resized);
            resized.Save("resized.png");
            return BitmapToImageSource(appIcon.ToBitmap());
        }

        //Ich habe keine ahnung was hier passiert 
        static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        public static void toGrayscale(Bitmap original)
        {
            byte[] bytes = new byte[original.Width * original.Height];

            for (int x = 0; x < original.Width; x++){
                for (int y = 0; y < original.Height; y++)
                {
                    Color color = original.GetPixel(x, y);
                    bytes[x * original.Width +  y] = color.A;
                }
            }
        }
    }
}
