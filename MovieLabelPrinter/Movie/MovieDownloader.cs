using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net;

// Movie downloader

namespace Movie
{
    // MovieDownloader class: gets movie data from web server based on movie Id

    public class MovieDownloader
    {
        const string _UrlTemplate = "https://tup-ops-eng-coding-exercise.s3.amazonaws.com/data/{0}.json";

        public MovieInfo DownloadMovieData(string id)
        {
            using (var webClient = new WebClient())
            {
                string url = String.Format(_UrlTemplate, id);

                string movieJson = webClient.DownloadString(url);

                return Deserialize(movieJson);
            }
        }

        protected MovieInfo Deserialize(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Deserialize<MovieInfo>(json);
        }
    }
}
