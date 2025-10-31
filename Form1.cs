using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace boo
{
    public partial class Form1 : Form
    {
        // values for drawing
        private bool isDrawing = false;
        private Point lastPoint = Point.Empty;
        private Pen drawPen = new Pen(Color.Red, 3); // you can make this customizable
        private bool drawMode = false; // True when Draw button is active
        private bool eraseMode = false;


        public Form1()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void showButton_Click(object sender, EventArgs e)
        {
            // Show the Open File dialog. If the user clicks OK, load the
            // picture that the user chose.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            // Clear the picture.
            pictureBox1.Image = null;
        }

        private void backgroundButton_Click(object sender, EventArgs e)
        {
            // Show the color dialog box. If the user clicks OK, change the
            // PictureBox control's background to the color the user chose.
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                pictureBox1.BackColor = colorDialog1.Color;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            // Close the form.
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // If the user selects the Stretch check box, 
            // change the PictureBox's
            // SizeMode property to "Stretch". If the user clears 
            // the check box, change it to "Normal".
            if (checkBox1.Checked)
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp|GIF Image|*.gif";
                    saveFileDialog.Title = "Save an Image File";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Create a bitmap the size of the PictureBox
                        Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                        // Draw the PictureBox (image + background) onto the bitmap
                        pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

                        // Determine format based on extension
                        System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
                        string ext = System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower();

                        switch (ext)
                        {
                            case ".jpg":
                            case ".jpeg":
                                format = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                            case ".bmp":
                                format = System.Drawing.Imaging.ImageFormat.Bmp;
                                break;
                            case ".gif":
                                format = System.Drawing.Imaging.ImageFormat.Gif;
                                break;
                            case ".png":
                            default:
                                format = System.Drawing.Imaging.ImageFormat.Png;
                                break;
                        }

                        // Save the bitmap to file
                        bmp.Save(saveFileDialog.FileName, format);
                        pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

                        // Optional: Assign the bitmap back to the PictureBox
                        pictureBox1.Image = bmp;
                    }
                }
            }
            else
            {
                MessageBox.Show("No image to save!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawMode && e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                lastPoint = e.Location;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawMode && isDrawing)
            {
                Bitmap bmp = (Bitmap)pictureBox1.Image;
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    if (eraseMode)
                    {
                        // Draw with background color to erase
                        g.DrawLine(new Pen(pictureBox1.BackColor, drawPen.Width), lastPoint, e.Location);
                    }
                    else
                    {
                        // Normal drawing
                        g.DrawLine(drawPen, lastPoint, e.Location);
                    }
                }
                lastPoint = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawMode && e.Button == MouseButtons.Left)
                isDrawing = false;
        }

        private void changePenColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                drawPen.Color = colorDialog1.Color;
        }

        private void EnsureBitmap()
        {
            if (pictureBox1.Image == null)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(pictureBox1.BackColor); // fill with background
                }
                pictureBox1.Image = bmp;
            }
        }


        private void drawButton_Click(object sender, EventArgs e)
        {
            drawMode = true;
            eraseMode = false; // back to normal drawing
            pictureBox1.Cursor = Cursors.Cross;
            EnsureBitmap(); // make sure PictureBox has an image
        }

        private void eraserButton_Click(object sender, EventArgs e)
        {
            eraseMode = true;
            drawMode = true; // must be in draw mode to use eraser
            pictureBox1.Cursor = Cursors.Default; // or Cursors.Cross if you prefer
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                drawPen.Color = colorDialog1.Color;
                eraseMode = false; // make sure we are in draw mode
                drawMode = true;
            }
        }

        private void strokeSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            drawPen.Width = (float)strokeSizeUpDown.Value;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}