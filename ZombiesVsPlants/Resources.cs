using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;

namespace ZombiesVsPlants
{
    class Resources
    {
        //
        //欢迎面板的图片资源
        //
        public static Image WelcomeBackImage()
        {
            Image image = Res.UI_Images.menu;
            //Image image = Image.FromFile("Images/interface/menu.png");
            return image;
        }
        public static Image Start_Over()
        {
           // Image image = Image.FromFile("Images/interface/start_over.png");
            Image image = Res.UI_Images.start_over;
            return image;
        }
        public static Image Start_Leave()
        {
            Image image = Res.UI_Images.start_leave;
            //Image image = Image.FromFile("Images/interface/start_leave.png");
            return image;
        }

        //根据植物名称获取植物图片
        public ArrayList BoxImage(string name)
        {
            ArrayList images = new ArrayList();
            Image img = (Image)Res.RImages.ResourceManager.GetObject(name);
            GifToJpg2(img, name, images);
            return images;
        }
        //角色类型的图片
        public ArrayList BulletType(string type)
        {
            ArrayList images = new ArrayList();
            Image img = (Image)Res.RImages.ResourceManager.GetObject(type);
            GifToJpg2(img, type, images);
            return images;
        }

        public Image CardType(string type)
        {
           // Image image = Image.FromFile(Path.Combine(@"Images\Card\Plants\",
              //  type + ".png"));
            Image image = (Image)Res.RCards.ResourceManager.GetObject(type);
            return image;
        }

        public ArrayList PlantType(string type)
        {
            ArrayList images = new ArrayList();
            Image img = (Image)Res.RImages.ResourceManager.GetObject(type);
            GifToJpg2(img, type, images);
            return images;
        }

        public ArrayList ZombieType(string type)
        {
            ArrayList images = new ArrayList();
            Image img = (Image)Res.RImages.ResourceManager.GetObject(type);
            GifToJpg2(img, type, images);
            return images;
        }

        public ArrayList ZombieAttack(string type)
        {
            ArrayList images = new ArrayList();
            type += "Attack";
            Image img = (Image)Res.RImages.ResourceManager.GetObject(type);
            GifToJpg2(img, type, images);
            return images;
        }

        public ArrayList ZombieDead(string type)
        {
            ArrayList images = new ArrayList();
            GifToJpg2((Image)Res.RImages.ResourceManager.GetObject( "ZombieLostHead"), "ZombieLostHead", images);
            GifToJpg2((Image)Res.RImages.ResourceManager.GetObject("ZombieDie"), "ZombieDie", images);
            return images;
        }

        //阳光
        public ArrayList Sun()
        {
            ArrayList images = new ArrayList();
            string name = "Sun";
            Image img = (Image)Res.RImages.ResourceManager.GetObject(name);
            GifToJpg2(img, name, images);
            //GifToJpg(@"Images\Sun.gif", images);
            return images;
        }

        public void GifToJpg2(Image img,string name, ArrayList images)
        {
            
            FrameDimension fd = new FrameDimension(img.FrameDimensionsList[0]);
            int framecount = img.GetFrameCount(fd);
            string path = "tempImages/"+name;

            //资源是否已存在
            bool exists = Directory.Exists(path);
            //创建新文件夹
            if (!exists)
                Directory.CreateDirectory(path);
            //MessageBox.Show(file+": "+"framecount:" + framecount);

            //保存各帧
            if (name == "BucketheadZombie")
                framecount = 12;
            if (name == "ZombieDie")
                framecount = 7;
            for (int i = 0; i < framecount; i++)
            {
                img.SelectActiveFrame(fd, i);
                exists = File.Exists(Path.Combine(path, "frame_" + i + ".Png"));
                if (!exists)
                {
                    img.Save(Path.Combine(path, "frame_" + i + ".Png"), ImageFormat.Png);
                }

                Image image = Image.FromFile(Path.Combine(path, "frame_" + i + ".Png"));
                images.Add(image);
            }
        }

        internal ArrayList PeaBulletHit()
        {
            ArrayList images = new ArrayList();
            string name = "PeaBulletHit";
            Image img = (Image)Res.RImages.ResourceManager.GetObject(name);
            GifToJpg2(img, name, images);
            //GifToJpg(@"Images\Plants\PeaBulletHit.gif", images);
            return images;
        }

        public ArrayList PrepareGrowPlants()
        {
            ArrayList images = new ArrayList();
            string name = "PrepareGrowPlants";
            Image img = (Image)Res.UI_Images.ResourceManager.GetObject(name);
            GifToJpg2(img, name, images);
            //GifToJpg(@"Images\PrepareGrowPlants.gif", images);
            return images;
        }

        public static Image NormalBackground()
        {
            Image image = (Image)Res.UI_Images.ResourceManager.GetObject("background1");
            return image;
        }
    }
}
