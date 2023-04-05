using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombiesVsPlants.API;
using System.Drawing;
using System.Threading;
using System.IO;
using ZombiesVsPlants;


namespace ZombiesVsPlants.Extends
{
    class PlantCard : BaseObject
    {
        private int needSun;
        private string type;

        public int NeedSun
        {
            get { return needSun; }
            set { needSun = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private Image image;

        public Image Image
        {
            get { return image; }
            set { image = value; }
        }
        private double waitTime, loadTime;

        public double LoadTime
        {
            get { return loadTime; }
            set { loadTime = value; }
        }

        public double WaitTime
        {
            get { return waitTime; }
            set { waitTime = value; }
        }
        private bool isChoose;

        public PlantCard(string type)
        {
            this.waitTime = 0;
            this.loadTime = new APIS().CoolDown(type);
            this.isChoose = false;
            this.type = type;
            this.needSun = new APIS().NeedSun(type);

            loadImage();
        }

        public void loadImage()
        {
            
            this.image = (Image)Res.RCards.ResourceManager.GetObject(Type);
            Width = image.Width;
            Height = image.Height;
        }

        public void loading()
        {
            loadImage();
            // time
            waitTime = loadTime;
            //装载线程
            Thread t = new Thread(new ThreadStart(loadingThread));
            t.Start();
        }

        public void loadingThread()
        {
            int gameTime = Controller.GameTime;
            while (Controller.gameStatus != GameStatus.OVER
               && gameTime == Controller.GameTime && waitTime != 0)
            {
                //判断是否游戏暂停
                if (Controller.gameStatus == GameStatus.START)
                {
                    waitTime = waitTime - 0.5;                 

                    Map.Update();
                }
                Thread.Sleep(500);
            }

            //初始化
            isChoose = false;
        }

        public void Draw(Graphics g)
        {
            if (needSun > Map.SunShine || waitTime != 0)
                new APIS().TransparentImage(X, Y, image, g);
            else
                lock (Map.draw_lock)
                    g.DrawImage(image, X, Y,
                    APIS.CardWidth, APIS.CardHeight);
            if (waitTime != 0)
            {
                //画刷，字体
                Brush brush = new SolidBrush(Color.Black);
                Font font = new Font("Arial", 15);
                //取字体尺寸
                SizeF sizeF = g.MeasureString(waitTime.ToString("F1"), new Font("宋体", 9));
                //绘制字体
                lock (Map.draw_lock)
                    g.DrawString(waitTime.ToString("F1"), font, brush,
                    new Rectangle(X + APIS.CardWidth / 2 - (int)sizeF.Width, Y + 
                        APIS.CardHeight / 2 - (int)sizeF.Height , APIS.CardWidth,
                        APIS.CardHeight));

            }
            //
            //  绘制所需阳光数据
            //
            //画刷，字体
            Brush brush1 = new SolidBrush(Color.Black);
            Font font1 = new Font("Arial", 10);
            //取字体尺寸
            SizeF sizeF1 = g.MeasureString(waitTime.ToString("F1"), new Font("宋体", 9));
            lock (Map.draw_lock)
                g.DrawString(needSun.ToString(), font1, brush1,
                    new Rectangle(X + APIS.CardWidth/2+10,Y+
                        APIS.CardHeight/4*3 , APIS.CardWidth,
                        APIS.CardHeight/2));
        }

        //是否被点击（如果被点击进行判断并执行）
        public bool isClick(int x,int y)
        {
            if (isContact(x,y) && needSun <= Map.SunShine)
            {
                //界面变化
                //loading();//绘制装填时间
                //p.waitToPlant(type);
                if (Map.Pb.IsAcitive == true)
                    return false;
                else
                    Map.initPlantBox(type);
                Map.SunCost = needSun;
                Map.Pc = this;
                return true;
            }
            return false;
        }

        //计算指定坐标是否接触本对象
        public bool isContact(int x,int y)
        {
            if (x > this.X && x < (this.X + APIS.CardWidth) && y > this.Y && y < this.Y
                + APIS.CardHeight)
            {
                return true;
            }
            return false;
        }
    }
}
