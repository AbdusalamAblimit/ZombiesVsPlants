using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Threading;
using System.Drawing.Imaging;
using ZombiesVsPlants.API;

namespace ZombiesVsPlants
{
    class PlantsBox : BaseRole
    {
        private Map map;
        private bool isAcitive;
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private ArrayList images;
        private int Images_num;

        public bool IsAcitive
        {
            get { return isAcitive; }
            set { isAcitive = value; }
        }
        private int x, y;
        private int width, height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public PlantsBox(string type, int sun,Map _map)
        {
            this.Map = _map;
            this.isAcitive = true;
            this.type = type;
            this.ShadowX = 0;
            this.ShadowY = 0;

            images = new Resources().BoxImage(type);
            this.width = ((Image)images[0]).Width;
            this.height = ((Image)images[0]).Height;
        }

        public void Run()
        {
            dance();
        }

        public PlantsBox(Map _map)
        {
            this.isAcitive = false;
            this.map = _map;
            this.Map= _map;
        }


        public void setXY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void dance()
        {
            Thread t = new Thread(new ThreadStart(danceThread));
            t.Start();
        }

        public void danceThread()
        {
            int gameTime = Controller.GameTime;
            while (Controller.gameStatus != GameStatus.OVER
               && gameTime == Controller.GameTime && isAcitive)
            {
                //判断是否游戏暂停
                if (Controller.gameStatus == GameStatus.START)
                {
                    //更新图片
                    Images_num = (Images_num + 1) % images.Count;

                    map.Update();
                }
                Thread.Sleep(APIS.PlantsBoxSpeed);
            }
        }

        public void setType(string type)
        {
            this.isAcitive = true;
            this.type = type;

            images = new Resources().BoxImage(type);
            this.width = ((Image)images[0]).Width;
            this.height = ((Image)images[0]).Height;

            dance();
        }

        public void Destroy()
        {
            isAcitive = false;
        }

        //影子坐标
        private int shadowX, shadowY;

        public int ShadowY
        {
            get { return shadowY; }
            set { shadowY = value; }
        }

        public int ShadowX
        {
            get { return shadowX; }
            set { shadowX = value; }
        }

        public void Draw(Graphics g)
        {
            lock (Map.draw_lock)
                if (images != null)
            {
                Image image = (Image)images[Images_num];
                g.DrawImage(image, x, y, width, height);
            }
            //绘制影子
            if (shadowX != 0 && shadowY != 0)
            {
                Preview(ShadowX, ShadowY, g);
            }
        }

        public void Preview(int x, int y, Graphics g)
        {
            //Mouse.GetPosition(this);
            if (images != null)
            {
                Bitmap bitmap = TransparentImage(x, y, (Image)images[Images_num], g);
            }
        }

        //使图片透明化
        private Bitmap TransparentImage(int x, int y, Image image, Graphics g)
        {
            Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
            float[][] ptsArray ={ 
            new float[] {1, 0, 0, 0, 0},
            new float[] {0, 1, 0, 0, 0},
            new float[] {0, 0, 1, 0, 0},
            new float[] {0, 0, 0, 0.2f, 0}, //注意：此处为0.1f，图像为强透明
            new float[] {0, 0, 0, 0, 1}};
            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            ImageAttributes imgAttributes = new ImageAttributes();
            //设置图像的颜色属性
            imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default,
            ColorAdjustType.Bitmap);
            lock(Map.draw_lock)
            g.DrawImage(bitmap, new Rectangle(x, y, bitmap.Width, bitmap.Height),
            0, 0, bitmap.Width, bitmap.Height,
            GraphicsUnit.Pixel, imgAttributes);

            return bitmap;
        }
    }
}
