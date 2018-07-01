using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Movie;

// Movie label layout generator

namespace MovieLabel
{
    // MovieLabelLayoutGenerator: class that takes care of finding proper font sizes of each block to fit content on the label

    internal class MovieLabelLayoutGenerator
    {
        // Movie data

        MovieInfo _Movie;

        // List of blocks with additional layout info

        List<BlockLayoutInfo> _BlockLayouts = new List<BlockLayoutInfo>();

        // Layout formatting constants

        const string _FontFamily = "Serif";
        const int _BlockSpacingPercent = 5;
        const float _MinFontSize = 4;
        const float _MaxFontSize = 72;
        const float _MaxBaseFontSizePerSquareInch = 24;

        public MovieLabelLayoutGenerator(MovieInfo movie)
        {
            _Movie = movie;

            // In-place building of layout block info and blocks themselves

            _BlockLayouts.Add
                (
                    new BlockLayoutInfo
                    (
                        new MovieLabelTitleBlock(_Movie),
                        20,
                        FontStyle.Bold
                    )
                );

            _BlockLayouts.Add
                (
                    new BlockLayoutInfo
                    (
                        new MovieLabelStatsBlock(_Movie),
                        12,
                        FontStyle.Regular
                    )
                );

            _BlockLayouts.Add
                (
                    new BlockLayoutInfo
                    (
                        new MovieLabelSynopsisBlock(_Movie),
                        14,
                        FontStyle.Regular
                    )
                );

            _BlockLayouts.Add
                (
                    new BlockLayoutInfo
                    (
                        new MovieLabelStaffAndGenreBlock(_Movie),
                        10,
                        FontStyle.Regular
                    )
                );
        }

        // Main drawing routine to be called by consumer

        public void Draw(Graphics g, Rectangle drawingArea)
        {
            // Calculate layout dynamically

            CalculateLayout(g, drawingArea);

            // Draw all blocks one by one

            foreach (var blockLayout in _BlockLayouts)
            {
                blockLayout.Block.Draw(g, blockLayout.ActualFont, blockLayout.ActualBlockArea);
            }
        }

        // Layout calculation routine
        // Takes some reasonably large base font size (based on printing area in square inches), then iteratively
        // do "trial-and-error" tests whether all blocks fit into area; if not - base font size is gradually decreased
        // until either all blocks fit or minimum possible base font size is reached.
        // Font sizes for each block are calculated based on base font size and relative font size:
        // <Block font size> = <base font size> * <relative font size> / 10

        protected void CalculateLayout(Graphics g, Rectangle drawingArea)
        {
            // Set font size to some reasonably maximum value based on printing area

            float baseFontSize = Math.Min(_MaxFontSize, _MaxBaseFontSizePerSquareInch * drawingArea.Width * drawingArea.Height / 100000);

            // Calculate spacing between blocks

            int blockSpacing = drawingArea.Height * _BlockSpacingPercent / 100;

            // Do the "trial-and-error" starting from initial (biggest) base font size

            for (bool isFit = false; !isFit; baseFontSize = (float) Math.Truncate(baseFontSize * 9 / 10))
            {
                // Initialize layout pass

                int blockY = 0;

                // Estimate size of each block and sum up

                foreach (var blockLayout in _BlockLayouts)
                {
                    // Create block font based on base and relative font sizes

                    blockLayout.ActualFontSize = baseFontSize * blockLayout.RelativeFontSize / 10;
                    blockLayout.ActualFont = new Font(_FontFamily, blockLayout.ActualFontSize, blockLayout.FontStyle);

                    // Call block's measurement routine, round up the result and save

                    blockLayout.ActualBlockHeight = (int) Math.Ceiling( blockLayout.Block.MeasureHeight(g, blockLayout.ActualFont, drawingArea.Width) );

                    blockLayout.ActualBlockArea = new Rectangle
                                                        (
                                                            drawingArea.X + 0,
                                                            drawingArea.Y + blockY,
                                                            drawingArea.Width,
                                                            blockLayout.ActualBlockHeight
                                                        );

                    // Adjust current Y-position

                    blockY += blockLayout.ActualBlockHeight;

                    // Check if current block fits the printing area

                    isFit = (blockY <= drawingArea.Height);

                    // Add spacing before next block

                    blockY += blockSpacing;
                }

                // Check for absoulte minimum font size

                if (baseFontSize <= _MinFontSize)
                    isFit = true;
            }
        }
    }

    // BlockLayoutInfo: block info plus layout testing parameters

    internal class BlockLayoutInfo
    {
        // Constant part

        // Referenced label block object for measuring and printing

        public MovieLabelBlock Block { get; set; }

        // Relative font size; it assumed that base font size is 10, and it should be used for smallest font on the label

        public float RelativeFontSize { get; set; }

        // Font style of the block

        public FontStyle FontStyle { get; set; }


        // Dynamic (variable) part for "trial and error" font size downscaling

        // Actual font size (pt)

        public float ActualFontSize { get; set; }

        // Actual font itself; will be reused by printing routine after last (successful) measuring attempt

        public Font ActualFont { get; set; }

        // Actual block height

        public int ActualBlockHeight { get; set; }

        // Actual block area

        public Rectangle ActualBlockArea { get; set; }



        public BlockLayoutInfo(MovieLabelBlock block, float relativeFontSize, FontStyle fontStyle)
        {
            Block = block;
            RelativeFontSize = relativeFontSize;
            FontStyle = fontStyle;

            ActualFontSize = 0;
            ActualBlockHeight = 0;
        }
    }
}
