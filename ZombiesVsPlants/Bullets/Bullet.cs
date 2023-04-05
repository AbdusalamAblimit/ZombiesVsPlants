using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using ZombiesVsPlants.Zombies;
using ZombiesVsPlants.API;
using ZombiesVsPlants.Enums;
using WindowsFormsApplication3;

namespace ZombiesVsPlants.Bullets
{
    class Bullet : BaseRole
    {
        private int ConduitLength = 90;
        private int speed;
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Bullet(Street s, Floor f, Direction dir)
            : base(s, f)
        {
            X = (int)s + 30;
            Y = (int)f;
            Dir = dir;

            //System.Windows.Forms.MessageBox.Show("产生子弹");
        }

        public override void loadImage()
        {
            Images = new Resources().BulletType(type);
            Width = ((Image)Images[0]).Width;
            Height = ((Image)Images[0]).Height;
        }

        public virtual void Move()
        {
            switch (Dir)
            {
                case Direction.UP:
                    Y += Speed;
                    break;
                case Direction.DOWN:
                    Y -= Speed;
                    break;
                case Direction.LEFT:
                    X -= Speed;
                    break;
                case Direction.RIGHT:
                    X += Speed;
                    break;
            }
        }

        public override void Run()
        {
            Thread t = new Thread(new ThreadStart(this.RunThread));
            t.Start();
        }

        public virtual void Attack()
        {
            Enemy.Hp--;
            Enemy.Attacked();
            if (Enemy.Hp <= 0)
                Enemy.Dead();
        }

        public virtual void RunThread()
        {
            while (Controller.gameStatus != GameStatus.OVER)
            {
                if (Controller.gameStatus == GameStatus.START)
                {
                    switch (RolesStatus)
                    {
                        case RoleStatus.MOVE:
                            Move();
                            if (X > 2000)
                                RolesStatus = RoleStatus.DEAD;
                            break;
                        case RoleStatus.DEAD:
                            Dead();
                            return;
                        case RoleStatus.ATTACK:
                            Dead();
                            Attack();                           
                            return;
                    }
                    if (contactEnemy())
                    {
                        RolesStatus = RoleStatus.ATTACK;
                    }
                    //更新图片               
                    Images_num = (Images_num + 1) % Images.Count;

                    // Form update              
                    Map.Update();
                }
                Thread.Sleep(APIS.BulletMoveSpeed);
            }
        }

        public virtual void Dead()
        {
            Map.Update();
            Thread.Sleep(60);
            lock (Map.bullets_lock)
                Map.delete(Map.Bullets, this);
        }

        private bool contactEnemy()
        {
            lock(Map.zombies_lock)
            for (int j = 0; j < Map.Zombies.Count; j++)
            {
                Zombie z = (Zombie)Map.Zombies[j];
                if (new APIS().isHit(this,z) && RolesStatus != RoleStatus.DEAD)
                {
                    Enemy = z;
                    return true;
                }
            }
            return false;
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            lock (Map.draw_lock)
                if (Images != null)
                g.DrawImage((Image)Images[Images_num], (int)X + 10, (int)Y);
        }
    }
}
