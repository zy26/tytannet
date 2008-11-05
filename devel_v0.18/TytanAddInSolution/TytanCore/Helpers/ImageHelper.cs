using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.VisualBasic.Compatibility.VB6;
using stdole;
using EnvDTE80;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class that provides additional support functionality for images used by Visual Studio.
    /// </summary>
    public sealed class ImageHelper
    {
        /// <summary>
        /// Replaces all occurrences of one color with another one. This might help in quick background color changes.
        /// </summary>
        public static Bitmap ReplaceColor(Image src, Color from, Color to)
        {
            if (src != null)
            {
                Bitmap result = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
                ColorMap map = new ColorMap();
                ImageAttributes attrs = new ImageAttributes();
                Graphics g = Graphics.FromImage(result);

                // set the color transformation:
                map.NewColor = to;
                map.OldColor = from;
                attrs.SetRemapTable(new ColorMap[] {map}, ColorAdjustType.Bitmap);

                g.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height), 0, 0, src.Width, src.Height,
                            GraphicsUnit.Pixel, attrs);
                g.Dispose();
                return result;
            }

            return null;
        }

        /// <summary>
        /// Sets a tab picture to a window from given image. Background color is (0,254,0).
        /// </summary>
        public static IPicture SetTabPicture (Window2 window, Image tabIcon)
        {
            if (window != null && tabIcon != null)
            {
                // repair the transparent-background color,
                // for Visual Studio 2005/2008 the colors are:
                // (0,254,0) and (255,0,255)...
                Bitmap tabNewIcon =
                    ReplaceColor(tabIcon, Color.FromArgb(255, 0, 254, 0), Color.FromArgb(255, 255, 0, 255));

                // make proper OLE picture:
                IPicture tabNewOleIcon = Support.ImageToIPicture(tabNewIcon) as IPicture;

                window.SetTabPicture(tabNewOleIcon);
                return tabNewOleIcon;
            }

            return null;
        }
    }
}
