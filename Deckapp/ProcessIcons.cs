using System.Diagnostics;
using System.Drawing;
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
    }
}
