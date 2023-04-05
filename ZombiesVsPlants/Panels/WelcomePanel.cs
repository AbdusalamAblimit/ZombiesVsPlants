using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ZombiesVsPlants.Panels
{
    class WelcomePanel : Panel
    {
        //私有属性
        private Image backgroundImage;
        private Button button;
        private Image ButtonDownImage;
        private Image ButtonUpImage;
        //主窗口
        private ZombieVSPlant form;

        public WelcomePanel(ZombieVSPlant form)
        {
            this.form = form;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint, true);
            this.BorderStyle = BorderStyle.Fixed3D;

            init();
        }

        public void init()
        {
            //Image
            backgroundImage = Resources.WelcomeBackImage();
            ButtonDownImage = Resources.Start_Over();
            ButtonUpImage = Resources.Start_Leave();
            //按钮初始化
            button = new Button();
            button.SetBounds(246, 544, 305, 32);
            button.BackgroundImage = ButtonUpImage;
            //增加控件
            this.Controls.Add(button);
            //添加事件委托
            button.MouseDown += new MouseEventHandler(button_MouseDown);
            button.MouseUp += new MouseEventHandler(button_MouseUp);
            this.Paint += new PaintEventHandler(WelcomePanel_Paint);

            API.APIS.MusicPlay("menumusic");

        }

        public void WelcomePanel_Paint(Object o, PaintEventArgs e)
        {
            Graphics g;
            lock (Map.draw_lock)
            {
                g = e.Graphics;
                g.DrawImage(backgroundImage, 0, 0, 800, 600);
            }
        }

        public void button_MouseUp(Object o, MouseEventArgs e)
        {
            button.BackgroundImage = ButtonUpImage;
            form.showMisson();
        }

        public void button_MouseDown(Object o, MouseEventArgs e)
        {
            button.BackgroundImage = ButtonDownImage;
        }
    }
}
