using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Movie Cache

namespace Movie
{
    // MovieCache class: retrieves movie data based on Id either from web server or from local cache

    public class MovieCache 
    {
        Dictionary<string, MovieInfo> _Cache = new Dictionary<string, MovieInfo>();

        MovieDownloader _Downloader = new MovieDownloader();

        public MovieInfo Get(string id)
        {
            // Try to find movie in local cache

            var movie = _Cache.ContainsKey(id) ? _Cache[id] : null;

            if (movie == null)
            {
                // Download from web

                movie = _Downloader.DownloadMovieData(id);

                if (movie != null)
                    _Cache.Add(id, movie);
            }

            return movie;
        }
    }
}
