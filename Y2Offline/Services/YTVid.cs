using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Y2Offline.Services
{
    internal class YTVid
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Id { get; set; }
        public MemoryStream Thumbnail { get; set; }
    }
}
