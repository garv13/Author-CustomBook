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
        public string ChapterName { get; set; }
        //for file
        //[JsonProperty(PropertyName = "containerName")]
        //public string ContainerName { get; set; }

        //[JsonProperty(PropertyName = "resourceName")]
        public string ResourceName { get; set; }

        //[JsonProperty(PropertyName = "sasQueryString")]
        //public string SasQueryString { get; set; }

        //[JsonProperty(PropertyName = "imageUri")]
        public string ImageUri { get; set; }

        ////for cover image
        //[JsonProperty(PropertyName = "containerName2")]
        //public string ContainerName2 { get; set; }

        //[JsonProperty(PropertyName = "resourceName2")]
        public string ResourceName2 { get; set; }

        //[JsonProperty(PropertyName = "sasQueryString2")]
        //public string SasQueryString2 { get; set; }

        //[JsonProperty(PropertyName = "imageUri2")]
        public string ImageUri2 { get; set; }


    }
}
