using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;
using ZombiesVsPlants.API;
using ZombiesVsPlants.Lands;
using ZombiesVsPlants.Enums;
using System.Threading;
using System.Drawing;

namespace ZombiesVsPlants.Plants
{
    class Plant : BaseRole
    {
        //临时种植该植物的草地
        private Land land;
        private string type;
        private bool GrowSoil;

        public Plant(Street s, Floor f):base(s,f)
        {

        }

        public Plant()
        {

        }

        public virtual void Instance(Street street, Floor floor)
        {
            this.X = (int)street;
            this.Y = (int)floor;
            this.Street = street;
            this.Floor = floor;

            //默认所有植物方向向右，不同的在自己的构造函数重定义
            Dir = Direction.RIGHT;
        }

        public virtual void Dead()
        {
            lock(Map.plants_lock)
            Map.delete(Map.Plants,this);
            this.land.makeEmpty();
        }

        public override void Run()
        {
            Thread t = new Thread(new ThreadStart(RunThread));
            t.Start();
        }

        public virtual void RunThread()
        {
            int time = 0;
            int gameTime = Controller.GameTime;
            while (Controller.gameStatus != GameStatus.OVER
               && gameTime == Controller.GameTime)
            {
                if (Controller.gameStatus != GameStatus.STOP)
                {
                    switch (RolesStatus)
                    {
                        case RoleStatus.NOTHING:
                            break;
                        case RoleStatus.NORMAL:
                            PlantAction(time);
                            break;
                        case RoleStatus.DEAD:
                            Dead();
                            return;
                    }
                    time++;
                    //更新图片
                    Images_num = (Images_num + 1) % Images.Count;
                    Map.Update();
                }
                Thread.Sleep(APIS.PlantDanceSpeed);
            }
        }

        public virtual void PlantAction(int time)
        {
            
        }

        public override void loadImage()
        {
            Images = new Resources().PlantType(type);
            Width = ((Image)Images[0]).Width;
            Height = ((Image)Images[0]).Height;    
            X = X - Width / 2 + APIS.PlantWidth / 2 - 5;
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            if (Images_num >= Images.Count) return;
            lock (Map.draw_lock)
            if (Images != null)
                g.DrawImage((Image)Images[Images_num], (int)X, (int)Y);
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public Land Land
        {
            get { return land; }
            set { land = value; }
        }
    }
}
