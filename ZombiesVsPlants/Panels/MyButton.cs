using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ZombiesVsPlants.Panels
{
    class MyButton : Panel
    {
        private string text;
        private Image image;

        public MyButton(string text)
        {
            this.text = text;
            this.image = Res.UI_Images.Button;
            this.BackColor = Color.Transparent;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint, true);
            this.Paint += new PaintEventHandler(MyButton_Paint);
        }

        public void MyButton_Paint(Object o, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(image, 0, 0, 113, 41);
            //字体
            Brush brush1 = new SolidBrush(Color.Black);
            Font font1 = new Font("Arial", 8);
            //取字体尺寸
            SizeF sizeF = g.MeasureString(text, new Font("宋体", 9));
            g.DrawString(text, font1, brush1,
                    new Rectangle(56 - (int)sizeF.Width / 2, 20 - (int)sizeF.Height / 2,
                    (int)sizeF.Width + 4, (int)sizeF.Height));
        }

        public string MyText
        {
            get { return text; }
            set { text = value; }
        }
    }
}
