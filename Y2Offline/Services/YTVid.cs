using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;


namespace Y2Offline.Services
{
    public class YTVid
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Id { get; set; }
        public ImageSource Thumbnail { get; set; }

        public DateTime PublishedAt { get; set; }

        public string Description { get; set; }
    }
}
