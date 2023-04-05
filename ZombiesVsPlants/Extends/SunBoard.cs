using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ZombiesVsPlants
{
    class SunBoard : BaseObject
    {
        private Map map;
        private int startX, startY;
        private Image image;

        public SunBoard(Map map)
        {
            this.map = map;
            this.startX = 620;
            this.startY = 0;

            init();
        }

        public void init()
        {
            image = (Image)Res.UI_Images.SunBack;
        }

        public void Draw(Graphics g)
        {
            //绘制背景
            lock(Map.draw_lock)
            g.DrawImage(image, startX, startY, image.Width, image.Height);
            //画刷，字体
            System.Drawing.Brush brush = new SolidBrush(Color.Black);
            Font font = new Font("Arial", 15);
            //取字体尺寸
            SizeF sizeF = g.MeasureString(map.SunShine+"", new Font("宋体", 9));
            //绘制字体
            g.DrawString(map.SunShine + "", font, brush, startX + 80 - sizeF.Width / 2 - 8,
                (34 / 2 - sizeF.Height / 2 - 5));
        }
    }
}
