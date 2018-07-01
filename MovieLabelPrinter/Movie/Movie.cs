using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Movie data

namespace Movie
{
    // MovieInfo class that represents movie object received from web server
    // All properties assumed of type string because no other assumptions can be made based on requirements

    public class MovieInfo
    {
        public string id { get; set; }
        public string title { get; set; }
        public string year { get; set; }
        public string rating { get; set; }
        public string length { get; set; }
        public string seasons { get; set; }
        public string synopsis { get; set; }
        public List<string> stars { get; set; }
        public List<string> genres { get; set; }
        public List<string> creator { get; set; }

        public MovieInfo()
        {
            id = "";
            title = "";
            year = "";
            rating = "";
            length = "";
            seasons = "";
            synopsis = "";
            stars = new List<string>();
            genres = new List<string>();
            creator = new List<string>();
        }
    }
}
