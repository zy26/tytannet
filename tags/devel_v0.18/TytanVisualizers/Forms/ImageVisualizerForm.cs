using System.Drawing;
using System.Windows.Forms;

namespace Pretorianie.Tytan.Forms
{
    public partial class ImageVisualizerForm : Form
    {
        private readonly int initialWidth;
        private readonly int initialHeight;

        public ImageVisualizerForm()
        {
            InitializeComponent();
        }

        public ImageVisualizerForm(Image img, int multiplier)
        {
            InitializeComponent();

            Text = string.Format("{0} [{1}x{2}]", img.GetType().Name, img.Width, img.Height);
            Width = initialWidth = multiplier * img.Width;
            Height = initialHeight = multiplier * img.Height;
            BackgroundImage = img;
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        public ImageVisualizerForm(ImageList.ImageCollection images, Size size, int multiplier)
        {
            InitializeComponent();

            Text = string.Format("{0} - items: {3} [{1}x{2}]", images.GetType().Name, size.Width, size.Height, images.Count);
            Width = multiplier * size.Width;
            Height = multiplier * size.Height;

            // add tab control with images:
            TabControl tabs = new TabControl();

            tabs.Dock = DockStyle.Fill;

            foreach (Image img in images)
            {
                TabPage page = new TabPage(string.Format("{0} [{1}x{2}]", img.GetType().Name, img.Width, img.Height));

                page.BackgroundImage = img;
                page.BackgroundImageLayout = ImageLayout.Stretch;

                tabs.TabPages.Add(page);
            }

            Controls.Add(tabs);
        }

        protected override void OnDoubleClick(System.EventArgs e)
        {
            Width = initialWidth;
            Height = initialHeight;
            base.OnDoubleClick(e);
        }
    }
}