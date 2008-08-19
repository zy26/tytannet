using System.Windows.Forms;
using System.Drawing;
using stdole;

namespace Pretorianie.Tytan.Core.CustomAddIn
{
    /// <summary>
    /// Class that wraps VisualStudio IDE specific image manipulations.
    /// </summary>
    public class CustomImageConverter : AxHost
    {
        /// <summary>
        /// Default costructor.
        /// </summary>
        public CustomImageConverter()
            : base("63109182-966B-4E3C-A8B2-8BC4A88D221C")
        {
        }

        /// <summary>
        /// Converts given image to StdOLE picture object.
        /// </summary>
        public IPictureDisp ConvertToIPictureDisp (Image picture)
        {
            if(picture != null)
                return GetIPictureDispFromPicture(picture) as IPictureDisp;

            return null;
        }
    }
}
