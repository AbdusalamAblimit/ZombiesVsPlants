using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombiesVsPlants.Lands;
using ZombiesVsPlants.API;
using System.Threading;
using WindowsFormsApplication3;
using ZombiesVsPlants.Zombies;
using ZombiesVsPlants.Cleaners;
using ZombiesVsPlants.Extends;
using ZombiesVsPlants.Plants;

namespace ZombiesVsPlants.Missions
{
    class Mission3 : Mission
    {
        private int ZombieNum = 12;
        public Mission3() : base()
        {
            suns = 300;
        }

        public override void initMission(Map map)
        {
            base.initMission(map);

            //初始化剩余界面信息
            LevelStart();
        }

        public override void loadBackImage()
        {
            //初始化背景图片
            Map.BackgroundImage = Resources.NormalBackground();
        }

        public override void initMap()
        {
            //初始化时间
            Map.IsDayTime = true;
            //初始化铲子
            Map.Shovel = new Shovel(map);
            //
            Map.SunBoard = new SunBoard(Map);
            Map.SunShine = suns;
            //草地环境
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Land land = new Grass(LandX(i), LandY(j));
                    Map.Lands.Add(land);
                }
            }
        }

        public override void initCleaners()
        {
            Map.addCleaner(new LawnMower(Street.FIRST, Floor.FIRST));
            Map.addCleaner(new LawnMower(Street.FIRST, Floor.SECOND));
            Map.addCleaner(new LawnMower(Street.FIRST, Floor.THIRD));
            Map.addCleaner(new LawnMower(Street.FIRST, Floor.FOURTH));
            Map.addCleaner(new LawnMower(Street.FIRST, Floor.FIFTH));
        }

        public override void initCards()
        {
            Map.initCard(new PlantCard("SunFlower"));
            Map.initCard(new PlantCard("Peashooter"));
            Map.initCard(new PlantCard("Repeater"));
           
        }

        //关卡的开头动画
        public override void MissionZombie()
        {
            Controller.gameStatus = GameStatus.BEGINMOVIE;

            for (int i = 0; i < 5; i++)
            {
                Zombie z;
                if (i < 3)
                     z = new NormalZombie(ro.Next(1000, 1180), (i + 1) * 80);
                else if (i < 4)
                    z = new ConeheadZombie(ro.Next(1000, 1180), (i + 1) * 80);
                else
                    z = new BucketheadZombie(ro.Next(1000, 1180), (i + 1) * 80);
                z.RolesStatus = ZombiesVsPlants.Enums.RoleStatus.NORMAL;
                Map.addZombie(z);
            }
            
        }
        public override void beginMovie()
        {
            base.beginMovie();
        }

        public override void CreateZombies()
        {
            //选择卡牌完成后的游戏界面初始化      
            //提示开始
            int i = 0, validNum = Controller.GameTime;
            while (i < ZombieNum && Controller.gameStatus != GameStatus.OVER
                && validNum == Controller.GameTime)
            {
                if (Controller.gameStatus == GameStatus.START)
                {
                    int random = ro.Next(1, 100);

                    if (i < 3)
                        Map.addZombie(new NormalZombie(Street.TENTH, LandY(ro.Next(0, 5))));
                    else if (i < 8)
                    {
                        if (random < 60)
                            Map.addZombie(new ConeheadZombie(Street.TENTH, LandY(ro.Next(0, 5))));
                        else
                            Map.addZombie(new NormalZombie(Street.TENTH, LandY(ro.Next(0, 5))));
                    }
                    else
                    {
                        if (random < 50)
                            Map.addZombie(new ConeheadZombie(Street.TENTH, LandY(ro.Next(0, 5))));
                        else
                            Map.addZombie(new BucketheadZombie(Street.TENTH, LandY(ro.Next(0, 5))));
                    }
                    
                    lock(map.p.sounds_queue_lock)
                    this.map.p.sounds_queue.Enqueue("groan" + Convert.ToString(ro.Next(1, 7)));
                    i++;
                    Thread.Sleep(10000);
                }
            }
            Controller.allZombieCreated = true;
        }
    }
}
