using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ZombiesVsPlants.Panels
{
    class GamePanel : Panel
    {
        //子面板
       public MenuDialog md;
        //控制器，与控制器关联
        public Controller c;
        //菜单按钮
        public MyButton button;
        public Queue sounds_queue;
        public object sounds_queue_lock = new object();
        //private
        public GamePanel(Controller c)
        {
            sounds_queue = new Queue();
            this.c = c;
            initSubPanel();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint, true);
            this.BorderStyle = BorderStyle.Fixed3D;   
            this.Paint += new PaintEventHandler(GamePanel_Paint);
            this.MouseClick += new MouseEventHandler(GamePanel_MouseClick);
            this.MouseMove += new MouseEventHandler(GamePanel_MouseMove);
        }

        public void initSubPanel()
        {
            md = new MenuDialog(this);
            md.SetBounds(340, 40, 413, 520);

            button = new MyButton("菜单");
            button.SetBounds(900,0, 113, 41);
            button.MouseClick += new MouseEventHandler(GamePanel_MouseClick);
            button.MouseMove += new MouseEventHandler(GamePanel_MouseMove);

            this.Controls.Add(md);
            this.Controls.Add(button);
        }

        public void MenuDialogClicked(Object o)
        {
            if (o is MyButton)
            {
                  
                 sounds_queue.Enqueue("buttonclick");
                //ZombiesVsPlants.API.APIS.SoundPlay("buttonclick");
                MyButton mb = (MyButton)o;
                bool isStart = false;
                //初始化地图
                switch (mb.MyText)
                {
                    case "重新开始":
                        c.Restart();
                        break;
                    case "第一关":
                    case "第二关":
                    case "第三关":
                    case "第四关":
                        c.MissionStart(mb.MyText);
                        md.Visible = false;
                        break;
                    case "退出":
                        Environment.Exit(0);
                        break;
                    case "菜单":
                        md.Visible = true;
                        break;
                }
                
                if (isStart)
                {
                    md.Visible = false;
                }
            }
            else if (o is Label)
            {
                md.Visible = false;
            }
        }

        public void MapClicked(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                c.isClick(e.X, e.Y);
                
                c.planting(e.X, e.Y);
            }
            else if (e.Button == MouseButtons.Right)
            {
                c.PbDestroy();
            }
        }

        public void GamePanel_Paint(Object o, PaintEventArgs e)
        {
            System.Drawing.Graphics g = e.Graphics;
            //视图改变
           // API.MyAPI.SoundPlay("safas");
          lock(sounds_queue_lock)
            if(sounds_queue.Count > 0)
            {
                API.APIS.SoundPlay((string)sounds_queue.Dequeue());
            }
            c.Map.Draw(g);
        }

        public void GamePanel_MouseClick(Object o, MouseEventArgs e)
        {
            MenuDialogClicked(o);
            MapClicked(e);
        }

        public void GamePanel_MouseMove(Object o, MouseEventArgs e)
        {
            //if(Controller.gameStatus==GameStatus.OVER)
            //{
             //  Controller.gameStatus = GameStatus.STOP;
            //   MenuDialogClicked(button);
            //}
            if (Controller.gameStatus == GameStatus.START)
            {
                c.PlantBoxMove(e.X, e.Y);
                c.setShadow(e.X, e.Y);
                c.MoveShovel(e.X, e.Y);
                if (c.isEnter(e.X, e.Y))
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                    this.Cursor = Cursors.Arrow;
                return;
            }
            if (o is MyButton)
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        
    }
}
