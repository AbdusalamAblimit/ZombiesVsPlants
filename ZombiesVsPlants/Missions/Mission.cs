using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;
using System.Threading;
using System.Collections;
using ZombiesVsPlants.Zombies;
using ZombiesVsPlants.API;

namespace ZombiesVsPlants.Missions
{
    class Mission
    {
        
        
        private ArrayList beginNoticeImage;
        private int valid;
        public Map map;
        //随即发生器
        protected Random ro;
        public bool allZombiesCreated = false;

        public int suns=0;

        public Mission()
        {
            ro = new Random();
            
        }

        public virtual void Next()
        {

        }

        public virtual void initMission(Map map)
        {
            this.map = map;
           
            Controller.allZombieCreated = false;
            loadBackImage();
        }

        //初始化开头动画到生成僵尸逻辑，按顺序进行
        public virtual void beginMovie()
        {
            APIS.MusicPlay("beginsound");
            MissionZombie();
            while (APIS.GamePanelX < 380)
            {
                Thread.Sleep(20);
                APIS.GamePanelX += 20;
                showZombies(20, true);
                Map.Update();
            }
            Thread.Sleep(5000);
            while (APIS.GamePanelX > 0)
            {
                Thread.Sleep(20);
                APIS.GamePanelX -= 20;
                showZombies(20, false);
                Map.Update();
            }
            
            Map.Clear();
            Map.Clear(Map.Zombies);
            

        }

        public virtual void EndMovie()
        {
            var img = Res.UI_Images.Win;
            int num = 0, time = 200;
            ArrayList imgs = new ArrayList();
            imgs.Add(img);
            for (int i = 0; i < 50; ++i)
            {
                imgs.Add(APIS.PicSized((System.Drawing.Bitmap)imgs[i], 1.009));
            }

            APIS.MusicPlay("winmusic");
            while (num < 50)
            {
                map.NoticeImage = (System.Drawing.Bitmap)imgs[num];

                num = (num + 1);

                map.Update();
               // map.p.GamePanel_Paint(new object(),);
                Thread.Sleep(time);

            }
            map.NoticeImage = null;
             Controller.gameStatus = GameStatus.OVER;
            Thread.Sleep(2000);
            // map.p.MenuDialogClicked(map.p.button);




        }
        public virtual void LoseMovie()
        {
            
            var img = Res.UI_Images.ZombiesWon;
            int num = 0, time = 100;
            ArrayList imgs = new ArrayList();
            imgs.Add(img);
            for (int i = 0; i < 50; ++i)
            {
                imgs.Add(APIS.PicSized((System.Drawing.Bitmap)imgs[i], 1.009));
            }

            APIS.MusicPlay("losemusic");
            Thread.Sleep(4000);
            APIS.MusicPlay("scream");
            while (num < 50)
            {
                map.NoticeImage = (System.Drawing.Bitmap)imgs[num];

                num = (num + 1);

                map.Update();

                Thread.Sleep(time);

            }
            map.NoticeImage = null;
             // Controller.gameStatus = GameStatus.OVER;
            Thread.Sleep(2000);
            // map.p.MenuDialogClicked(map.p.button);



        }

        public virtual void initCards()
        {

        }
        public virtual void initCleaners()
        {

        }
        public virtual void initMap()
        {

        }    
        public virtual void beginNotice()
        {
            beginNoticeImage = new Resources().PrepareGrowPlants();
            int num = 0, time = 500;
            APIS.MusicPlay("readysetplant");
            while (num < beginNoticeImage.Count)
            {
                map.NoticeImage = (System.Drawing.Image)beginNoticeImage[num];

                num = (num + 1);

                map.Update();
                Thread.Sleep(time);
            }
            map.NoticeImage = null;
            Controller.gameStatus = GameStatus.START;
            Thread.Sleep(2000);
            APIS.MusicPlay("MainMusic1");

        }
        //每个关卡共用的一些方法，如对地图的初始化以及僵尸的生成逻辑
        public virtual void CreateZombies()
        {

        }

        /// <summary>
        /// 每一关的僵尸种类显示
        /// </summary>
        public virtual void MissionZombie()
        {
            Controller.gameStatus = GameStatus.BEGINMOVIE;
            for (int i = 0; i < 5; i++)
            {
                Zombie z = new NormalZombie(ro.Next(1000, 1180), (i+1)*80);
                z.RolesStatus = ZombiesVsPlants.Enums.RoleStatus.NORMAL;
                Map.addZombie(z);
            }
        }

        public void showZombies(int step, bool isleft)
        {
            lock(map.zombies_lock)
            for (int i = 0; i < Map.Zombies.Count; i++)
            {
                Zombie z = (Zombie)Map.Zombies[i];
                if (isleft)
                    z.X -= step;
                else
                    z.X += step;
            }
        }
        /// <summary>
        /// 每一关的僵尸种类显示
        /// </summary>


        public virtual void loadBackImage()
        {

        }

        public virtual void LevelStart()
        {
            Thread t = new Thread(new ThreadStart(LevelThread));
            t.Start();
        }

        public virtual void LevelThread()
        {
            beginMovie();
            initCards();
            initCleaners();
            initMap();
            //游戏一开始的通知
            beginNotice();
            //生成僵尸
            CreateZombies();
        }

        public Street LandX(int x)
        {
            if (x == 0)
                return Street.FIRST;
            else if (x == 1)
                return Street.SECOND;
            else if (x == 2)
                return Street.THIRD;
            else if (x == 3)
                return Street.FOURTH;
            else if (x == 4)
                return Street.FIFTH;
            else if (x == 5)
                return Street.SIXTH;
            else if (x == 6)
                return Street.SEVENTH;
            else if (x == 7)
                return Street.EIGHTH;
            else if (x == 8)
                return Street.NINTH;
            else
                return Street.NULL;
        }

        public Floor LandY(int y)
        {
            if (y == 0)
                return Floor.FIRST;
            else if (y == 1)
                return Floor.SECOND;
            else if (y == 2)
                return Floor.THIRD;
            else if (y == 3)
                return Floor.FOURTH;
            else if (y == 4)
                return Floor.FIFTH;
            else
                return Floor.NULL;
        }

        public int Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        internal Map Map
        {
            get { return map; }
            set { map = value; }
        }

        public int Valid1
        {
            get { return valid; }
            set { valid = value; }
        }
    }
}
