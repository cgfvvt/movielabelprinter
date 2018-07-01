using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;

using Movie;
using Printing;

// Movie label blocks

namespace MovieLabel
{
    // MovieLabelBlock: abstract class for portion of label (block); movie label is split into 4 blocks one beneath another

    internal abstract class MovieLabelBlock
    {
        protected MovieInfo _Movie;

        public MovieLabelBlock(MovieInfo movie)
        {
            _Movie = movie;
        }

        // Measure height based on given font (and its size) and width

        public abstract float MeasureHeight(Graphics g, Font font, int width);

        // Do drawing (printing) with given font and target printing area (rect)

        public abstract void Draw(Graphics g, Font font, Rectangle rect);
    }

    // Topmost block: movie title

    internal class MovieLabelTitleBlock : MovieLabelBlock
    {
        public MovieLabelTitleBlock(MovieInfo movie)
            : base(movie)
        {
        }

        public override float MeasureHeight(Graphics g, Font font, int width)
        {
            SizeF textSize = g.MeasureString(_Movie.title, font, width, StringFormat.GenericTypographic);

            return textSize.Height;
        }

        public override void Draw(Graphics g, Font font, Rectangle rect)
        {
            g.DrawString(_Movie.title, font, Brushes.Black, rect, StringFormat.GenericTypographic);
        }
    }

    // Second block: year, rating, length and seasons / parts
    // Block is split horizontally into two parts: 40% / 60%; the first column has a year and rating, the second column has length and/or season count

    internal class MovieLabelStatsBlock : MovieLabelBlock
    {
        const int _YearAndRatingFillFactor    = 40;
        const int _LengthAndSeasonsFillFactor = 60;

        public MovieLabelStatsBlock(MovieInfo movie)
            : base(movie)
        {
        }

        public override float MeasureHeight(Graphics g, Font font, int width)
        {
            SizeF textSize1 = g.MeasureString(_Movie.YearAndRating()   , font, width * _YearAndRatingFillFactor    / 100, StringFormat.GenericTypographic);
            SizeF textSize2 = g.MeasureString(_Movie.LengthAndSeasons(), font, width * _LengthAndSeasonsFillFactor / 100, StringFormat.GenericTypographic);

            return Math.Max(textSize1.Height, textSize2.Height);
        }

        public override void Draw(Graphics g, Font font, Rectangle rect)
        {
            // Split rectangle into two parts vertically: year/rating and length/seasons

            Rectangle rect1 = new Rectangle
                (
                    rect.X + 0 * rect.Width / 100,
                    rect.Y,
                    rect.Width * _YearAndRatingFillFactor / 100,
                    rect.Height
                );

            Rectangle rect2 = new Rectangle
                (
                    rect.X + _YearAndRatingFillFactor * rect.Width / 100,
                    rect.Y,
                    rect.Width * _LengthAndSeasonsFillFactor / 100,
                    rect.Height
                );

            // Draw portions of stats text

            g.DrawString(_Movie.YearAndRating()   , font, Brushes.Black, rect1, StringFormat.GenericTypographic);
            g.DrawString(_Movie.LengthAndSeasons(), font, Brushes.Black, rect2, StringFormat.GenericTypographic);
        }
    }

    // Third block: Synopsis

    internal class MovieLabelSynopsisBlock : MovieLabelBlock
    {
        public MovieLabelSynopsisBlock(MovieInfo movie)
            : base(movie)
        {
        }

        public override float MeasureHeight(Graphics g, Font font, int width)
        {
            SizeF textSize = g.MeasureString(_Movie.synopsis, font, width, StringFormat.GenericTypographic);

            return textSize.Height;
        }

        public override void Draw(Graphics g, Font font, Rectangle rect)
        {
            g.DrawString(_Movie.synopsis, font, Brushes.Black, rect, StringFormat.GenericTypographic);
        }
    }

    // Bottom block: Staff and/or Creator(s) and/or Genre(s); all text is packed into single string with line breaks

    internal class MovieLabelStaffAndGenreBlock : MovieLabelBlock
    {
        public MovieLabelStaffAndGenreBlock(MovieInfo movie)
            : base(movie)
        {
        }

        public override float MeasureHeight(Graphics g, Font font, int width)
        {
            SizeF textSize = g.MeasureString(_Movie.StaffAndGenre(), font, width, StringFormat.GenericTypographic);

            return textSize.Height;
        }

        public override void Draw(Graphics g, Font font, Rectangle rect)
        {
            g.DrawString(_Movie.StaffAndGenre(), font, Brushes.Black, rect, StringFormat.GenericTypographic);
        }
    }

    // Supplementary class for MovieInfo data formatting; supplies extension methods to MovieInfo

    internal static class MovieInfoPrintingUtils
    {
        // BeautifyLength: add optional "min" suffix if length is an integer

        public static string BeautifyLength(this MovieInfo movie)
        {
            Int16 movieLength;

            if (movie.length != null && Int16.TryParse(movie.length, out movieLength))
                return String.Format("{0} min", movie.length);

            return movie.length;
        }

        // BeautifySeasons: add optional "season(s)" suffix if seasons is an integer

        public static string BeautifySeasons(this MovieInfo movie)
        {
            Int16 seasonCount;

            if (movie.seasons != null && Int16.TryParse(movie.seasons, out seasonCount))
                return String.Format("{0} season{1}", movie.seasons, ((seasonCount > 1) ? "s" : "") );

            return movie.seasons;
        }

        // YearAndRating: compile year and rating into one string with line break delimiter (if both values are there)

        public static string YearAndRating(this MovieInfo movie)
        {
            return String.Format
                (
                    "{0}{2}{1}",
                    movie.year,
                    movie.rating,
                    ((movie.year != String.Empty && movie.rating != string.Empty) ? "\n" : "")
                );
        }

        // LengthAndSeasons: compile length and season into one string with line break delimiter (if both values are there)

        public static string LengthAndSeasons(this MovieInfo movie)
        {
            return String.Format
                (
                    "{0}{2}{1}",
                    movie.BeautifyLength(),
                    movie.BeautifySeasons(),
                    ((movie.BeautifyLength() != String.Empty) && (movie.BeautifySeasons() != String.Empty) ? "\n" : "")
                );
        }

        // StaffAndGenre: compile staff/creator/genre string from three distinct string lists
        // Add captions as well, using "s" ending for plural

        public static string StaffAndGenre(this MovieInfo movie)
        {
            StringBuilder result = new StringBuilder();

            if (movie.stars != null && movie.stars.Count > 0)
            {
                result.Append("Starring: ");
                result.Append(String.Join(", ", movie.stars));
                result.AppendLine();
                result.AppendLine();
            }

            if (movie.creator != null && movie.creator.Count > 0)
            {
                result.AppendFormat("Creator{0}: ", ((movie.creator.Count > 1) ? "s" : ""));
                result.Append(String.Join(", ", movie.creator));
                result.AppendLine();
                result.AppendLine();
            }

            if (movie.genres != null && movie.genres.Count > 0)
            {
                result.AppendFormat("Genre{0}: ", ((movie.genres.Count > 1) ? "s" : ""));
                result.Append(String.Join(", ", movie.genres));
            }

            return result.ToString();
        }
    }
}
