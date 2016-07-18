using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamerAuthor
{
    class Book
    {
        public string Id { get; set; }
        public string PublisherId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public bool IsReady { get; set; }
        public string PageNo { get; set; }
        public string ChapterCost { get; set; }
        public int Chapters { get; set; }
        public string ResourceName { get; set; }
        public string ImageUri { get; set; }
        public string ResourceName2 { get; set; }
        public string ImageUri2 { get; set; }
        public string ChapterName { get; set; }

    }
}
