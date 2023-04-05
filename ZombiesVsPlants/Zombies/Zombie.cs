using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;
using ZombiesVsPlants.API;
using ZombiesVsPlants.Enums;
using System.Drawing;
using System.Threading;
using ZombiesVsPlants.Plants;

namespace ZombiesVsPlants.Zombies
{
    class Zombie : BaseRole
    {
        private int power;
        private string type;
        private int threadTime;
        bool isDead=false;

        public Zombie(Street street, Floor floor)
            : base(street, floor)
        {
            Y = Y - APIS.ZombieHeight + APIS.GlassHeight;
            RolesStatus = RoleStatus.MOVE;
        }

        public Zombie()
        {

        }

        public override void loadImage()
        {
            Images = new Resources().ZombieType(Type);
            Width = ((Image)Images[0]).Width;
            Height = ((Image)Images[0]).Height;
        }

        public void loadAttackImage()
        {
            Images = new Resources().ZombieAttack(type);
            Width = ((Image)Images[0]).Width;
            Height = ((Image)Images[0]).Height;
        }
        public Thread t;
        public override void Run()
        {
            t = new Thread(new ThreadStart(RunThread));
            t.Start();
        }

        public override void Attack()
        {
            if (Enemy == null || Enemy.Hp <= 0)
            {
                if (Enemy != null)
                {
                    Enemy.RolesStatus = RoleStatus.DEAD;
                    Enemy.Dead();
                }

                //System.Windows.Forms.MessageBox.Show("僵尸找不到攻击对象");
                RolesStatus = RoleStatus.MOVE;
                loadImage();
                return;
            }
            lock (Map.p.sounds_queue_lock)
                if (Enemy.Hp % 2 == 0)
                    Map.p.sounds_queue.Enqueue("chomp1");
                else
                    Map.p.sounds_queue.Enqueue("chomp2");
            Enemy.Hp -= power;
            Enemy.Attacked();
        }

        public virtual void Move()
        {
            if(this.RolesStatus != RoleStatus.DEAD)
            lock (Map.cleaners_lock)

                for (int i = 0; i < Map.Cleaners.Count; i++)
                {
                    Cleaners.Cleaner c = (Cleaners.Cleaner)Map.Cleaners[i];
                    if (c.RolesStatus == RoleStatus.MOVE && c.Floor ==this.Floor && c.X>this.X-30)
                    {
                        this.RolesStatus = RoleStatus.DEAD;
                        this.Dead();
                        return;
                    }
                }
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
           // Speed = 30;
           
            if (X < 50)
            {
                Controller.gameStatus = GameStatus.OVER;
                Map.Mission.LoseMovie();
            }
        }

        public virtual void RunThread()
        {
            //线程初始化
            Images_num = 0;
            threadTime = APIS.ZombieRunSpeed;
            int time = 0;
            int gameTime = Controller.GameTime;
            while (Controller.gameStatus != GameStatus.OVER
               && gameTime == Controller.GameTime)
            {
                //判断是否游戏暂停
                if (Controller.gameStatus != GameStatus.STOP)
                {
                    switch (RolesStatus)
                    {
                        case RoleStatus.NORMAL:
                            break;
                        case RoleStatus.MOVE:
                            if(time % 4 == 0)                           
                                Move();
                            break;
                        case RoleStatus.ATTACK:
                            if(time % 40 == 0)                         
                                Attack();
                            break;
                        case RoleStatus.DISPEAR:
                            Dispear();
                            return;
                        case RoleStatus.DEAD:
                            if (time % 4 == 0)                           
                                Move();
                            
                            if (Images_num == Images.Count - 1)
                            {
                                lock (Map.zombies_lock)
                                {
                                    Map.delete(Map.Zombies, this);

                                    if (Map.Zombies.Count == 0 && Controller.allZombieCreated && Controller.gameStatus == GameStatus.START)
                                    {

                                        new Thread (Map.Mission.EndMovie).Start(); 
                                    }
                                }
                                return;
                            }
                            break;
                    }
                    if (RolesStatus!= RoleStatus.DEAD&& RolesStatus != RoleStatus.DISPEAR&& contactEnemy())
                    {
                        RolesStatus = RoleStatus.ATTACK;
                        loadAttackImage();
                    }
                    if (RolesStatus != RoleStatus.DEAD && RolesStatus != RoleStatus.DISPEAR && Enemy != null && Enemy.Hp == 0 && 
                        Enemy.RolesStatus != RoleStatus.DEAD)
                    {
                        loadImage();
                        RolesStatus = RoleStatus.MOVE;
                        Enemy.RolesStatus = RoleStatus.DEAD;
                        ((Plant)Enemy).Dead();
                    }
                    time++;                  
                    //更新图片                 
                    Images_num = (Images_num + 1) % Images.Count;
                   
                    // Form update
                    Map.Update();
                }
                Thread.Sleep(threadTime);
            }
        }

        private bool contactEnemy()
        {
            lock(Map.plants_lock)
            for (int i = 0; i < Map.Plants.Count; i++)
            {
                Plant p = (Plant)Map.Plants[i];
                if(new APIS().isHit(p,this) && RolesStatus != RoleStatus.DEAD)
                {
                    Enemy = p;
                    return true;
                }
            }
            return false;
        }
        private Plant getEnemy()
        {
            lock (Map.plants_lock)
            for (int i = 0; i < Map.Plants.Count; i++)
            {
                Plant p = (Plant)Map.Plants[i];
                if (new APIS().isHit(p, this) && RolesStatus != RoleStatus.DEAD)
                {
                    Enemy = p;
                    return p;
                }
            }
            return null;
        }

        public override void Dispear()
        {
            lock(Map.zombies_lock)
            Map.delete(Map.Zombies,this);
            isDead = true;
        }

        public override void Dead()
        {
            Images_num = 0;
            Images = new Resources().ZombieDead(type);
            RolesStatus = RoleStatus.DEAD;
            isDead = true;
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            lock(Map.draw_lock)
            if (Images != null)
                g.DrawImage((Image)Images[Images_num], X, Y);
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public int Power
        {
            get { return power; }
            set { power = value; }
        }
    }
}
