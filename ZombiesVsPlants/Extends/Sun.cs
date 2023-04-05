using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombiesVsPlants.API;
using System.Drawing;
using System.Threading;
using ZombiesVsPlants.Enums;

namespace ZombiesVsPlants.Extends
{
    class Sun : BaseRole
    {
        private int finalX, finalY;
        private int sunNum;
        private int MoveY;
        private int MoveSpeed = 5;
        private int timeWait;
        private int threadTime;
        private bool isClicked;

        public int SunNum
        {
            get { return sunNum; }
            set { sunNum = value; }
        }

        public Sun(int x, int y, int finalX, int finalY)
        {
            this.X = x;
            this.Y = y;
            this.finalX = finalX;
            this.finalY = finalY;
            this.sunNum = 25;
            this.RolesStatus = RoleStatus.MOVE;

            loadImage();
        }

        public override void loadImage()
        {
            Images = new Resources().Sun();
            this.Width = ((Image)Images[0]).Width;
            this.Height = ((Image)Images[0]).Height;
        }

        public override void Run()
        {
            Thread t = new Thread(new ThreadStart(RunThread));
            t.Start();
        }

        public void RunThread()
        {
            threadTime = APIS.SunFlowSpeed;
            int gameTime = Controller.GameTime;
            while (Controller.gameStatus != GameStatus.OVER
               && gameTime == Controller.GameTime)
            {
                if (Controller.gameStatus == GameStatus.START)
                {
                    switch (RolesStatus)
                    {
                        case RoleStatus.MOVE:
                            //移动
                            if (Y < finalY)
                                Y += MoveSpeed;
                            break;
                        case RoleStatus.DEAD:
                            moveToDispear();
                            if (X <= APIS.SunBoardX + 20 && X >= APIS.SunBoardX - 40 &&
                                Y <= APIS.SunBoardY + 20 && Y >= APIS.SunBoardY - 40)
                            {
                                lock(Map.sunshine_lock)
                                    Map.SunShine += 25;
                                lock (Map.suns_lock) 
                                    Map.delete(Map.Suns, this);
                                return;
                            }
                            break;
                        case RoleStatus.DISPEAR:
                            Disapear();
                            break;
                    }
                    //更新图片
                    Images_num = (Images_num + 1) % Images.Count;                 
                }
                Thread.Sleep(threadTime);

                //Mm.update();
            }
        }

        public void Disapear()
        {
            lock(Map.suns_lock)
            Map.delete(Map.Suns, this);
        }

        public void Dead()
        {
            MoveSpeed = 16;
            threadTime = 20;
            RolesStatus = RoleStatus.DEAD;
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            lock (Map.draw_lock)
                if (Images != null)
            {
                g.DrawImage((Image)Images[Images_num], X, Y, Width, Height);
            }
        }

        //是否被点击
        public bool isClick(int x, int y)
        {
            if (isContact(x, y))
            {
                isClicked = true;
                Dead();
                return isClicked;
            }
            return false;
        }

        //计算指定坐标是否接触本对象
        public bool isContact(int x, int y)
        {
            if (x > X && x < (X + Width) && y > Y && y < Y
                + Height && RolesStatus != RoleStatus.DEAD)
            {
                return true;
            }
            return false;
        }

        public void moveToDispear()
        {
            if (X < APIS.SunBoardX)
            {
                Y -= MoveSpeed;
                X += MoveSpeed * (APIS.SunBoardX - X) / (Y - APIS.SunBoardY);
            }
            else if (X > APIS.SunBoardX)
            {
                Y -= MoveSpeed;
                X -= MoveSpeed * (X - APIS.SunBoardX) / (Y - APIS.SunBoardY);
            }
            else
                Y -= MoveSpeed;
            return;
        }
    }
}
