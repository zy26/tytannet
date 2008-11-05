using System.Drawing;
using System.Windows.Forms;

namespace Pretorianie.Tytan.Forms
{
    public partial class ColorVisualizerForm : Form
    {
        public ColorVisualizerForm(Color color)
        {
            InitializeComponent();
            colorPanel.BackColor = color;


            string name;

            // get the text-name of the color and put into form caption:
            if (color.IsKnownColor || color.IsSystemColor || color.IsNamedColor)
                name = color.ToKnownColor().ToString();
            else
                name = string.Format("A:{0} R:{1} G:{2} B:{3}", color.A, color.R, color.G, color.B);
            Text = string.Format("Color [{0}]", name);

            // extract other parameters:
            txtARGB.Text = string.Format("({0}, {1}, {2}, {3})", color.A, color.R, color.G, color.B);
            txtHSB.Text = string.Format("({0}, {1}, {2})", color.GetHue(), color.GetSaturation(), color.GetBrightness());
        }
    }
}