using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using WindowsFormsApplication3;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using System.Collections;

namespace ZombiesVsPlants.API
{
    //游戏各种参数的定义以及各种工具函数
    class APIS
    {
        public static int GamePanelX = 0;
        public static int GlassHeight = 100;//用于僵尸调整坐标的草地高度
        public static int ZombieHeight = 154;   //僵尸的高度
        public static int CardHeight = 60;      //植物卡片的宽和高
        public static int CardWidth = 100;
        public static int PlantWidth = 80;//草地宽和高，用于植物调整坐标
        public static int PlantHeight = 100;
        public static int BulletWidth = 52;    //豌豆（子弹）的宽度，碰撞判定时用到
        public static int SunBoardX = 620;     //阳光槽的坐标
        public static int SunBoardY = 0;
        public static int ShovelX = 750;       //铲子槽的坐标
        public static int ShovelY = 0;
        //各线程刷新速率
        public static int SunFlowSpeed = 100;
        public static int SunOfSunFlower = 8000;
        public static int ZombieRunSpeed = 80;
        public static int PlantAttackSpeed = 5000;
        public static int BulletMoveSpeed = 30;
        public static int PlantDanceSpeed = 80;
        public static int ControllSpeed = 150;
        public static int PlantsBoxSpeed = 200;
        public static int ZombieAttackSpeed = 100;   
        public static int RepeaterSecondAttackTime = 250;
        public static int SunDispearSpeed = 50;

        public bool isHit(BaseRole a, BaseRole b)
        {
            if ((a.X + a.Width) > (b.X + b.Width / 2 + 20) &&
                (a.X + a.Width) < (b.X + b.Width) && a.Floor == b.Floor)
            {
                return true;
            }
            return false;
        }

        public void TransparentImage(int x, int y, Image image, Graphics g)
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
            lock (Map.draw_lock)
                g.DrawImage(bitmap, new Rectangle(x, y, bitmap.Width, bitmap.Height),
                0, 0, bitmap.Width, bitmap.Height,
                GraphicsUnit.Pixel, imgAttributes);
        }

        public int NeedSun(string PlantType)
        {
            switch (PlantType)
            {
                case "SunFlower":
                    return 50;
                case "Peashooter":
                    return 100;
                case "Repeater":
                    return 175;
                case "GatlingPea":
                    return 325;
                case "WallNut":
                    return 75;
                default:
                    return 0;
            }
        }

        public int CoolDown(string PlantType)
        {
            switch (PlantType)
            {
                case "SunFlower":
                    return 5;
                case "Peashooter":
                    return 5;
                case "Repeater":
                    return 10;
                case "GatlingPea":
                    return 18;
                case "WallNut":
                    return 15;
                default:
                    return 0;
            }
        }

        public bool isEnter(BaseObject e,int x,int y)
        {
            if (x > e.X && x < (e.X + e.Width) && y > e.Y && y < e.Y
                + e.Height)
            {
                return true;
            }
            return false;
        }

        public static BoxForPlant AdjustPonint(int x, int y)
        {
            Street s;
            Floor f;
            //
            //  判断街道数
            //
            if (x > (int)Street.FIRST && x < (int)Street.SECOND)
                s = Street.FIRST;
            else if (x > (int)Street.SECOND && x < (int)Street.THIRD)
                s = Street.SECOND;
            else if (x > (int)Street.THIRD && x < (int)Street.FOURTH)
                s = Street.THIRD;
            else if (x > (int)Street.FOURTH && x < (int)Street.FIFTH)
                s = Street.FOURTH;
            else if (x > (int)Street.FIFTH && x < (int)Street.SIXTH)
                s = Street.FIFTH;
            else if (x > (int)Street.SIXTH && x < (int)Street.SEVENTH)
                s = Street.SIXTH;
            else if (x > (int)Street.SEVENTH && x < (int)Street.EIGHTH)
                s = Street.SEVENTH;
            else if (x > (int)Street.EIGHTH && x < (int)Street.NINTH)
                s = Street.EIGHTH;
            else if (x > (int)Street.NINTH && x < (int)Street.TENTH)
                s = Street.NINTH;
            else
                s = Street.NULL;
            //
            //  判断Y所在楼层数
            //
            if (y > (int)Floor.FIRST && y < (int)Floor.SECOND)
                f = Floor.FIRST;
            else if (y > (int)Floor.SECOND && y < (int)Floor.THIRD)
                f = Floor.SECOND;
            else if (y > (int)Floor.THIRD && y < (int)Floor.FOURTH)
                f = Floor.THIRD;
            else if (y > (int)Floor.FOURTH && y < (int)Floor.FIFTH)
                f = Floor.FOURTH;
            else if (y > (int)Floor.FIFTH && y < (int)Floor.SIXTH)
                f = Floor.FIFTH;
            else
                f = Floor.NULL;


            return new BoxForPlant(s, f);

        }

        internal static string PlantIntroduction(string type)
        {
            if (type.Equals("SunFlower"))
            {
                return "向日葵是你收集额外阳光必不可少的植物。为什么不多种一些呢？";
            }
            else if (type.Equals("Repeater"))
            {
                return "这个和第一个不一样的地方就是脑袋后面的叶子比较多，他是发2个豆豆的。";
            }
            else if (type.Equals("Peashooter"))
            {
                return "豌豆射手可谓你的第一道防线，他们朝来犯的僵尸射击豌豆。";
            }
            else
                return "机枪射手是名退役军人，传说它在部队时杀敌上千，它可以每次发射四颗豌豆。";
        }

        internal static string ChineseType(string type)
        {
            if (type.Equals("SunFlower"))
            {
                return "向日葵";
            }
            else if (type.Equals("Repeater"))
            {
                return "双枪豌豆";
            }
            else if (type.Equals("Peashooter"))
            {
                return "豌豆射手";
            }
            else if (type.Equals("Peashooter"))
            {
                return "机枪射手";
            }
            else if(type.Equals("WallNut"))
            {
                return "坚果墙";
            }
            else
                return "";
        }
        static Queue dvq = new Queue();
        static Queue<long> tm = new Queue<long>();
        static long now_tm = 0;
        internal static void SoundPlay(string sound_name)
        {
            
            var dv = new Microsoft.DirectX.DirectSound.Device();
            dv.SetCooperativeLevel(new System.Windows.Forms.Form(), CooperativeLevel.Normal);
            //var s = Res.Sounds.ResourceManager.GetStream("111");
            SecondaryBuffer buf = new SecondaryBuffer(Res.Sounds.ResourceManager.GetStream(sound_name), dv);
            buf.Play(0, BufferPlayFlags.Default);
            dvq.Enqueue(dv);
            dvq.Enqueue(buf);
            now_tm++;
            tm.Enqueue(now_tm);
            while (tm.First() + 20 < now_tm)
            {
                tm.Dequeue();
                dvq.Dequeue();
                dvq.Dequeue();
            }

        }



        static System.Media.SoundPlayer player;
        internal static void MusicPlay(string sound_name)
        {
            player = new System.Media.SoundPlayer(Res.Sounds.ResourceManager.GetStream(sound_name));
            player.Play();
        }


       internal  static Bitmap PicSized(Bitmap originBmp, double iSize)
        {
            int w = Convert.ToInt32(originBmp.Width * iSize);
            int h = Convert.ToInt32(originBmp.Height * iSize);
            Bitmap resizedBmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(resizedBmp);
            //设置高质量插值法  
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度  
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //消除锯齿
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            lock(Map.draw_lock)
            g.DrawImage(originBmp, new Rectangle(0, 0, w, h), new Rectangle(0, 0, originBmp.Width, originBmp.Height), GraphicsUnit.Pixel);
            g.Dispose();
            return resizedBmp;
        }

    }
        

    
}
