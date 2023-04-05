using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ZombiesVsPlants
{
    
    //一个抽象类，是大部分类的基类，定义了坐标，大小，图片等基本的属性
    class BaseObject
    {
        private ArrayList images;
        private int images_num;
        private int x;
        private int y;
        private int width, height;
        //地图
        private Map map;

        public BaseObject(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public BaseObject()
        {

        }

        public virtual void loadImage()
        {

        }

        public virtual void Draw(System.Drawing.Graphics g)
        {

        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Images_num
        {
            get { return images_num; }
            set { images_num = value; }
        }

        public ArrayList Images
        {
            get { return images; }
            set { images = value; }
        }

        internal Map Map
        {
            get { return map; }
            set { map = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        ~BaseObject()
        {
            Console.WriteLine("xigou");
        }
    }
}
