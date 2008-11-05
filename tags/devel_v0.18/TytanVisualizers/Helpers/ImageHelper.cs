namespace Pretorianie.Tytan.Helpers
{
    /// <summary>
    /// Helper class that wraps common operations over images.
    /// </summary>
    static class ImageHelper
    {
        public const int MinimalImageSize = 200;

        /// <summary>
        /// Returns the factor or enlarging the image, so it has at least MinimalImageSize.
        /// </summary>
        public static int AdjustSize(int width)
        {
            int multi = 1;

            // find the multiply value to have an image at least 100x100:
            if (width > 0)
            {
                while (width * multi < MinimalImageSize)
                    multi++;
            }

            return multi;
        }
    }
}
