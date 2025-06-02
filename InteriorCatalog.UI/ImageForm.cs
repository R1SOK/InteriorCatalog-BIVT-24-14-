using System.Drawing;
using System.Windows.Forms;

namespace InteriorCatalog.UI
{
    public class ImageForm : Form
    {
        public ImageForm(string imagePath)
        {
            Text = "Изображение мебели";
            Width = 600;
            Height = 600;
            this.BackColor = Color.White;

            var image = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Image.FromFile(imagePath)
            };

            Controls.Add(image);
        }
    }
}
