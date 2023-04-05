using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombiesVsPlants.Missions;
using ZombiesVsPlants.Extends;
using ZombiesVsPlants.Enums;
using ZombiesVsPlants.API;
using WindowsFormsApplication3;
using ZombiesVsPlants.Lands;
using ZombiesVsPlants.Plants;

namespace ZombiesVsPlants
{
    class Controller
    {
        private Map map;
        //游戏状态
        public static GameStatus gameStatus;
        public static int GameTime;
        public static bool allZombieCreated = false;
        public Controller()
        {            
            gameStatus = GameStatus.STOP;
        }

        public void setMap(Map map)
        {
            this.map = map;
        }

        public void viewChange(System.Drawing.Graphics g)
        {
            map.Draw(g);
        }

        public void MissionStart(string p)
        {
            Map.Clear(); 
            switch (p)
            {
                case "第一关":
                    map.initMission(new Mission1());
                    break;
                case "第二关":
                    map.initMission(new Mission2());
                    break;
                case "第三关":
                    map.initMission(new Mission3());
                    break;
                case "第四关":
                    map.initMission(new Mission4());
                    break;
            }
        }

        public void Restart()
        {
            if (Map.Mission != null)
                Map.Clear();
            map.initMission();
        }

        public bool planting(int x, int y)
        {
            PlantsBox pb = map.Pb;
            if (pb.IsAcitive == false)
                return false;

           

            pb.setXY(x - pb.Width / 2, y - pb.Height /
                     2);

            BoxForPlant bfp = APIS.AdjustPonint(x, y);
            Plant p = PlantOnLand(bfp, pb.Type);
            //Plant p = pb.Planting();
            if (p == null)
                return false;
            APIS.SoundPlay("plant");
            lock(map.sunshine_lock)
            map.SunShine -= map.SunCost;
            map.addPlant(p);
            lock(map.plantscards_lock)
            for(int i=0;i<map.Plantscards.Count;++i)
                if(((PlantCard)map.Plantscards[i]).Type==pb.Type)
                {
                    map.Pc = (PlantCard)map.Plantscards[i];
                    break;
                }

            map.Pc.loading();//装填弹药
            pb.Destroy();

            return true;
        }

        public Plant PlantOnLand(BoxForPlant bfp, string type)
        {
            
            Street s = bfp.S;
            Floor f = bfp.F;
            for (int i = 0; i < map.Lands.Count; i++)
            {
                Land land = (Land)map.Lands[i];
                if (s == land.Street && f == land.Floor)
                {
                    Plant plant = land.GrowPlant(type);
                    return plant;
                }
            }
            return null;
        }

        public void PbDestroy()
        {
            map.Pb.Destroy();
        }

        internal void PlantBoxMove(int x, int y)
        {
            PlantsBox pb = map.Pb;
            if (pb == null || pb.IsAcitive == false)
                return;
            pb.setXY(x - pb.Width / 2, y - pb.Height
                    / 2);
        }

        internal void setShadow(int p, int p_2)
        {
            BoxForPlant bfp = APIS.AdjustPonint(p, p_2);
            //判断指定位置是否有陆地可供种植
            if (hasLandStreet(bfp.S))
            {
                map.Pb.ShadowX = (int)bfp.S;
            }
            if (hasLandFloor(bfp.F))
            {
                map.Pb.ShadowY = (int)bfp.F;
            }
            map.Update();
        }

        public bool hasLandStreet(Street s)
        {
            for (int i = 0; i < map.Lands.Count; i++)
            {
                Land land = (Land)map.Lands[i];
                if (s == land.Street && land.IsEmpty)
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasLandFloor(Floor f)
        {
            for (int i = 0; i < map.Lands.Count; i++)
            {
                Land land = (Land)map.Lands[i];
                if (f == land.Floor && land.IsEmpty)
                {
                    return true;
                }
            }
            return false;
        }

        public void MoveShovel(int x, int y)
        {
            if (map.Shovel != null)
                map.Shovel.Move(x, y);
        }

        internal void isClick(int x, int y)
        {
            bool isClick = false;
            if (!map.Pb.IsAcitive)
            {
                isClick = clickCard(x, y) || clickSun(x, y);
                //如果植物卡牌或者阳光被点击或者等待植物被种植，则铲子无法被激活使用
                if (!isClick && map.Shovel != null)
                {
                    clickShovel(x, y);
                }
                else if (map.Shovel != null)
                    map.Shovel.Resume();
            }
        }

        //判断是否进入卡牌或者阳光的位置（作为更改鼠标的先决条件）
        internal bool isEnter(int x, int y)
        {
            bool isEnter = enterCardOrSun(x, y);
            return isEnter;
        }

        //鼠标是否进入卡牌或者阳光的范围的逻辑计算
        public bool enterCardOrSun(int x, int y)
        {
            //初始化鼠标坐标
            map.MouseX = x;
            map.MouseY = y;
            //判断
            lock(map.suns_lock)
            for (int i = 0; i < map.Suns.Count; i++)
            {
                Sun sun = (Sun)map.Suns[i];
                if (sun.isContact(x, y))
                {
                    return true;
                }
            }
            lock(map.plantscards_lock)
            for (int i = 0; i < map.Plantscards.Count; i++)
            {
                PlantCard pc = (PlantCard)map.Plantscards[i];
                if (pc.isContact(x, y))
                {
                    //通知模型层应该绘制对植物的介绍
                    map.IsIntroduce = true;
                    map.Pc = pc;
                    return true;
                }
            }
            map.IsIntroduce = false;
         
            return false;
        }

        //是否点击铲子
        public void clickShovel(int x, int y)
        {
            bool isDeletePlant = map.Shovel.isClick(x, y);
            if (isDeletePlant)
            {
                clickPlant(x, y);
            }
        }

        //是否点击了阳光
        private bool clickSun(int x, int y)
        {
            lock(map.suns_lock)
            for (int i = 0; i < map.Suns.Count; i++)
            {
                Sun sun = (Sun)map.Suns[i];
                if (sun.isClick(x, y))
                {
                    return true;
                }
            }
            return false;
        }

        //是否点击卡牌
        public bool clickCard(int x, int y)
        {
            for (int i = 0; i < map.Plantscards.Count; i++)
            {
                PlantCard pc = (PlantCard)map.Plantscards[i];
                if (pc.isClick(x, y))
                    return true;
            }
            return false;
        }

        //点中铲子后，是否点击植物，如果是，移除植物，并初始化铲子
        public void clickPlant(int x, int y)
        {
            for (int i = 0; i < map.Plants.Count; i++)
            {
                Plant plant = (Plant)map.Plants[i];
                //是否点击此植物
                if (new APIS().isEnter(plant, x, y))
                {
                    plant.Land.Clear();
                    plant.RolesStatus = RoleStatus.DEAD;

                    return;
                }
            }
        }

        internal Map Map
        {
            get { return map; }
            set { map = value; }
        }
    }
}
