using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.Engine
{
    public class ImageFunctions
    {
        /// <summary>
        /// http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale
        /// http://stackoverflow.com/questions/2265910/convert-an-image-to-grayscale
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the gray scale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                   new float[] {.3f, .3f, .3f, 0, 0},
                   new float[] {.59f, .59f, .59f, 0, 0},
                   new float[] {.11f, .11f, .11f, 0, 0},
                   new float[] {0, 0, 0, 1, 0},
                   new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //draw the original image on the new image
            //using the gray scale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        /// <summary>
        /// superior image quality
        /// http://danbystrom.se/2009/01/05/imagegetthumbnailimage-and-beyond/ - superior image quality
        /// http://www.dotnetperls.com/getthumbnailimage
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Bitmap GetThumbnailImageHighQuality(Bitmap img, int width, int height)
        {
            Bitmap bmpResized = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmpResized))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                /* =============================================
                 * don't use these, these reduces the results.
                 * =============================================
                 * g.CompositingMode = CompositingMode.SourceCopy;
               g.CompositingQuality = CompositingQuality.HighQuality;                        
               g.SmoothingMode = SmoothingMode.HighQuality;
               g.PixelOffsetMode = PixelOffsetMode.HighQuality;*/
                g.DrawImage(
                    img,
                    new Rectangle(Point.Empty, bmpResized.Size),
                    new Rectangle(Point.Empty, img.Size),
                    GraphicsUnit.Pixel);
            }
            return bmpResized;
        }


        public static Size GetThumbnailSize(Image original, int ThumbnailSize)
        {
            // Maximum size of any dimension.
            int maxPixels = ThumbnailSize/*64*/;

            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
    }

}
