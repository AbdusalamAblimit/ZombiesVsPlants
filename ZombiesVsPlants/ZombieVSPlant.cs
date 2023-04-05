using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZombiesVsPlants.Panels;

namespace ZombiesVsPlants
{
    public partial class ZombieVSPlant : Form
    {
        //面板集合
        private WelcomePanel wp;
        private GamePanel gp;
        //数据模型
        private Map map;
        //控制器
        private Controller c;
        //菜单栏
        private MainMenu myMenu;
        private MenuItem mnuFile;
        private MenuItem mnuAbout;
        private MenuItem mnuOpen;
        private MenuItem mnuSave;
        private MenuItem mnuExit;

        public ZombieVSPlant()
        {
            InitializeComponent();
            this.ControlBox = false;   // 设置不出现关闭按钮
            init();
        }

        public void init()
        {
            //initMenu();
            initController();
            initPanel();
            initMap();

            this.Name = "ZombieVsPlant";
        }

        private void initMenu()
        {
            //菜单栏
            myMenu = new MainMenu();
            mnuFile = new MenuItem("文件");
            mnuAbout = new MenuItem("关于");
            mnuOpen = new MenuItem("打开");
            mnuSave = new MenuItem("保存");
            mnuExit = new MenuItem("退出");
            mnuExit.Click += new EventHandler(mnuExit_Click);
            //往 Main Menu里面加入菜单
            myMenu.MenuItems.Add(mnuFile);
            myMenu.MenuItems.Add(mnuAbout);
            mnuFile.MenuItems.Add(mnuOpen);
            mnuFile.MenuItems.Add(mnuSave);
            mnuFile.MenuItems.Add(mnuExit);
            //设置菜单栏
            this.Menu = myMenu;
        }

        private void initMap()
        {
            map = new Map(gp);
            c.setMap(map);
        }

        private void initController()
        {
            c = new Controller();
        }

        private void initPanel()
        {
            gp = new GamePanel(c);
            gp.SetBounds(-120,0,1400,600);
            // 实例化第一个面板   
            wp = new WelcomePanel(this);
            wp.SetBounds(0,0,800,600);

            this.Controls.Add(wp);
        }

        internal void showMisson()
        {
            this.Controls.Remove(wp);
            this.Controls.Add(gp);
            this.Size = new Size(900,620);
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ZombieVSPlant_Load(object sender, EventArgs e)
        {

        }
    }
}
