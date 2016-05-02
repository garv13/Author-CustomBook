using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace StreamerAuthor
{
    class Dashboard
    {
        public string title { get; set; }
        public string Id { get; set; }

        public BitmapImage image { get; set; }

        public string downloads { get; set; }
        public string status { get; set; }

    }
}
