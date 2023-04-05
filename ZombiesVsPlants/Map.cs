using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using ZombiesVsPlants.Panels;
using ZombiesVsPlants.Zombies;
using ZombiesVsPlants.Bullets;
using ZombiesVsPlants.Extends;
using ZombiesVsPlants.Enums;
using ZombiesVsPlants.Missions;
using ZombiesVsPlants.Cleaners;
using ZombiesVsPlants.API;
using WindowsFormsApplication3;
using ZombiesVsPlants.Plants;
using ZombiesVsPlants.Lands;

namespace ZombiesVsPlants
{
    class Map : BaseObject
    {
        //私有成员
        private SunBoard sunBoard;
        private Shovel shovel;
        //植物临时箱
        private PlantsBox pb;
        private CardsBar cb;
        //临时存储的卡牌ID
        private PlantCard pc;
        private int mouseX, mouseY;
        private bool isIntroduce;
        //角色数组
        private ArrayList zombies;
        public object zombies_lock =  new object();

        private ArrayList plants;
        public object plants_lock = new object();

        private ArrayList plantscards;
        public object plantscards_lock = new object();

        private ArrayList bullets;
        public object bullets_lock = new object();

        private ArrayList suns;
        public object suns_lock = new object();

        private ArrayList cleaners;
        public object cleaners_lock = new object();

        //private ArrayList plantcards;
        private ArrayList lands;
        public object lands_lock = new object();
        //图片
        private Image backgroundImage;
        private Image noticeImage;
        //阳光
        private int sunshine;
        public object sunshine_lock = new object();
        private int sunCost;
        public object sunCost_lock = new object();
        private bool isDayTime;
        public object isDayTime_lock = new object();
        //关卡编号
        private Mission mission;
        //主界面
        public GamePanel p;

        Random random;

       static public object draw_lock = new object();


        public Map(GamePanel p)
        {
            this.p = p;
            this.Map = this;
            random = new Random();
            initMap();
        }

        public void initMap()
        {
            //集合组
            plantscards = new ArrayList();
            zombies = new ArrayList();
            plants = new ArrayList();
            bullets = new ArrayList();
            suns = new ArrayList();
            lands = new ArrayList();
            cleaners = new ArrayList();
            //植物临时箱
            pb = new PlantsBox(this);
            cb = new CardsBar(this);

            sunshine = 1000;
        }

        public void initMission(Mission mission)
        {
            if (this.mission != null)
                Controller.GameTime++;
            Controller.GameTime++;
            this.mission = mission;
            mission.initMission(this);
        }

        public void initMission()
        {
            if (this.mission == null)
            {
                System.Windows.Forms.MessageBox.Show("没有选择关卡");
                return;
            }
            Controller.GameTime++;
            this.mission.initMission(this);
        }

        public bool initPlantBox(string type)
        {
             for (int i = 0; i < this.Plantscards.Count; ++i)
                if (((PlantCard)this.Plantscards[i]).Type == type)
                {
                    if (((PlantCard)this.Plantscards[i]).WaitTime != 0)
                        return false;
                    break;
                }
            pb.setType(type);
            pb.Run();
            return true;
        }

        public void addSun(Sun s)
        {
            lock(suns_lock)
                suns.Add(s);
            s.Map = this;
            s.Run();
        }

        public void addBullet(Bullet b)
        {
            lock (bullets_lock)
                bullets.Add(b);
            b.Map = this;
            b.Run();
        }

        public void addZombie(Zombie z)
        {
            lock (zombies_lock)
                zombies.Add(z);
            
            z.Map = this;
            z.Run();
        }

        public void addCard(PlantCard pc)
        {
            lock (plantscards_lock)
                plantscards.Add(pc);
            pc.Map = this;
        }

        public void initCard(PlantCard pc)
        {
            cb.initCard(pc);
        }

        public void addPlant(Plant p)
        {
            lock (plants_lock)
                plants.Add(p);
            p.Map = this;
            p.Run();
        }

        public void addCleaner(Cleaner c)
        {
            lock (cleaners_lock)
                cleaners.Add(c);
            c.Map = this;
            c.Run();
        }

        public void Clear()
        {
            //Controller.gameStatus = GameStatus.OVER;
            lock (sunshine_lock)
                sunshine = 50;
            lock (plants_lock)
                Clear(plants);
            lock (zombies_lock)
                Clear(zombies);
            lock (suns_lock)
                Clear(suns);
            lock(plantscards_lock)
                plantscards.Clear();
            lock (cleaners_lock)
                Clear(cleaners);
            lock (bullets_lock)
                Clear(bullets);
            lock(lands_lock)
                for(int i=0;i<lands.Count;i++)
                {
                    ((Land)lands[i]).IsEmpty = true;
                }
        }

        public void Clear(ArrayList array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                ((BaseRole)array[i]).RolesStatus = RoleStatus.DISPEAR;
            }
            array.Clear();
        }


        public void delete(ArrayList array,BaseRole r)
        {
            array.Remove(r);
        }

        public void Update()
        {
            p.Invalidate();
        }

        public override void Draw(Graphics g)
        {
            lock(draw_lock)
            if (backgroundImage != null)
                g.DrawImage(backgroundImage, -(int)(APIS.GamePanelX * 1.0), 0, 1400, 600);
            lock(plants_lock)
                for (int i = 0; i < plants.Count; i++)
                {
                    Plant p = (Plant)plants[i];
                    p.Draw(g);
                }
            lock(zombies_lock)
                for (int i = 0; i < zombies.Count; i++)
                {
                    Zombie zombie = (Zombie)zombies[i];
                    zombie.Draw(g);
                }

            lock(cleaners_lock)
            for (int i = 0; i < cleaners.Count; i++)
            {
                Cleaner c = (Cleaner)cleaners[i];
                c.Draw(g);
            }

            lock(plantscards_lock)
            for (int i = 0; i < plantscards.Count; i++)
            {
                PlantCard pc = (PlantCard)plantscards[i];
                pc.Draw(g);
            }
            
            lock(bullets_lock)
            for (int i = 0; i < bullets.Count; i++)
            {
                Bullet b = (Bullet)bullets[i];
                b.Draw(g);
            }
               
            lock(suns_lock)
            for (int i = 0; i < suns.Count; i++)
            {
                Sun sun = (Sun)suns[i];
                sun.Draw(g);
            }
            
            if (pb.IsAcitive != false)
                pb.Draw(g);
            if (shovel != null)
                shovel.Draw(g);
            if (sunBoard != null)
                sunBoard.Draw(g);
            lock (draw_lock)
            if (noticeImage != null)
            {
                g.DrawImage(noticeImage, 900 / 2 - noticeImage.Width / 2 + 120,
                    600 / 2 - noticeImage.Height / 2, noticeImage.Width, noticeImage.Height);
            }
            
            //绘制介绍区域
            if (isIntroduce)
            {
                showIntroduce(g);
            }
        }

        //显示每个卡牌的说明
        private void showIntroduce(Graphics g)
        {
            int textY = mouseY + 30;
            int height = 50, textHeight = 20;
            Pen pen = new Pen(new SolidBrush(Color.Black));
            Brush brush = new SolidBrush(Color.WhiteSmoke);
            Brush textBrush = new SolidBrush(Color.Black);
            Font font = new Font("宋体", 9);

            string name = APIS.ChineseType(pc.Type);
            string coolTime = "冷却时间:" + pc.LoadTime + "秒";
            string introduce = APIS.PlantIntroduction(pc.Type);
            if (pc.WaitTime != 0)
            {
                height += textHeight;
            }
            if (pc.NeedSun > SunShine)
            {
                height += textHeight;
            }
            height += 20 + 20 * (introduce.Length / 8);
            lock (draw_lock)
            {
                g.FillRectangle(brush, mouseX, mouseY + 30, 120, height);
                g.DrawRectangle(pen, mouseX, mouseY + 30, 120, height);
            }
            //绘制名称
            textY = showText(name,
                textY + 10, font, textBrush, g);
            textY = showText(coolTime,
                textY, font, textBrush, g);
            //绘制说明
            textY = showText(introduce,
                textY, font, textBrush, g);
            //显示当前卡牌情况
            if (pc.WaitTime != 0)
            {
                Brush loadbrush = new SolidBrush(Color.Red);
                textY = showText("装填弹药中..",
                    textY, font, loadbrush, g);
            }
            if (pc.NeedSun > SunShine)
            {
                Brush loadbrush = new SolidBrush(Color.Red);
                textY = showText("阳光不足",
                    textY, font, loadbrush, g);
            }
        }

        //显示文本
        private int showText(string s, int textY, Font font, Brush brush, Graphics g)
        {
            int time = s.Length / 8;
            int startNum = 0;
            SizeF sizeF;
            lock (draw_lock)
                sizeF = g.MeasureString(s + "", font);
            string text, nextText;
            if (s.Length <= 8)
            {
                lock (draw_lock)
                    g.DrawString(s + "", font, brush, new Rectangle(mouseX + 60 - (int)sizeF.Width / 2,
                    textY, (int)sizeF.Width + 1, (int)sizeF.Height));
                textY += 20;
            }
            else
            {
                lock (draw_lock)
                {
                    nextText = s.Substring(startNum + 8, s.Length - 8);
                    text = s.Substring(startNum, 8);
                    SizeF sizeF2 = g.MeasureString(text + "", font);
                    g.DrawString(text + "", font, brush, new Rectangle(mouseX + 60 - (int)sizeF2.Width / 2,
                        textY, (int)sizeF2.Width + 1, (int)sizeF2.Height));
                    textY = showText(nextText, textY + 20, font, brush, g);
                }
            }

            return textY;
        }

        public int SunShine
        {
            get { return sunshine; }
            set { sunshine = value; }
        }
        public ArrayList Plants
        {
            get { return plants; }
            set { plants = value; }
        }

        public ArrayList Zombies
        {
            get { return zombies; }
            set { zombies = value; }
        }

        internal Shovel Shovel
        {
            get { return shovel; }
            set { shovel = value; }
        }

        public ArrayList Cleaners
        {
            get { return cleaners; }
            set { cleaners = value; }
        }
        
        internal PlantCard Pc
        {
            get { return pc; }
            set { pc = value; }
        }

        public int SunCost
        {
            get { return sunCost; }
            set { sunCost = value; }
        }
        
        public ArrayList Plantscards
        {
            get { return plantscards; }
            set { plantscards = value; }
        }

        public ArrayList Suns
        {
            get { return suns; }
            set { suns = value; }
        }

        public ArrayList Bullets
        {
            get { return bullets; }
            set { bullets = value; }
        }

        public Image NoticeImage
        {
            get { return noticeImage; }
            set { noticeImage = value; }
        }

        public Image BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }

        public bool IsDayTime
        {
            get { return isDayTime; }
            set { isDayTime = value; }
        }

        public ArrayList Lands
        {
            get { return lands; }
            set { lands = value; }
        }

        internal SunBoard SunBoard
        {
            get { return sunBoard; }
            set { sunBoard = value; }
        }

        internal PlantsBox Pb
        {
            get { return pb; }
            set { pb = value; }
        }

        public bool IsIntroduce
        {
            get { return isIntroduce; }
            set { isIntroduce = value; }
        }

        public int MouseX
        {
            get { return mouseX; }
            set { mouseX = value; }
        }

        public int MouseY
        {
            get { return mouseY; }
            set { mouseY = value; }
        }

        internal Mission Mission
        {
            get { return mission; }
            set { mission = value; }
        }
    }
}
