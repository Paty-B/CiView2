using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Viewer.View
{
    class ImageRequest
    {
        private static ImageRequest _instance;
        private Assembly _assembly;
        private Dictionary<string, BitmapImage> _dictionary;

        private ImageRequest()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _dictionary = new Dictionary<string, BitmapImage>();
        }

        public static ImageRequest Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ImageRequest();
                }
                return _instance;
            }
        }

        public BitmapImage GetImage(string imageName)
        {
            BitmapImage bitmapImage;
            if (!_dictionary.TryGetValue(imageName, out bitmapImage))
            {
                bitmapImage = new BitmapImage();
                Stream stream = _assembly.GetManifestResourceStream("Viewer.View.img." + imageName);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                _dictionary.Add(imageName, bitmapImage);
            }
            return bitmapImage;
        }
    }
}
