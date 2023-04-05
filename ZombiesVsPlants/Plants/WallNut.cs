using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;
using System.Threading;
using ZombiesVsPlants.Enums;
using ZombiesVsPlants.API;
using ZombiesVsPlants.Extends;

namespace ZombiesVsPlants.Plants
{
    class WallNut:Plant
    {
        public WallNut(Street s, Floor f)
          : base(s, f)
        {
            Hp = 15;
            Dir = Direction.RIGHT;
            Type = "WallNut";
            RolesStatus = RoleStatus.NORMAL;

            //加载图片数组
            loadImage();
        }

        public WallNut()
        {
        }

        public override void Instance(Street street, Floor floor)
        {
            base.Instance(street, floor);
            Hp = 15;
            Dir = Direction.RIGHT;
            Type = "WallNut";
            RolesStatus = RoleStatus.NORMAL;

            //加载图片数组
            loadImage();
        }

        public override void Run()
        {
            Thread t = new Thread(new ThreadStart(RunThread));
            t.Start();
        }

        

       

        public override void Attack()
        {

        }
        public override void Attacked()
        {
            if (this.RolesStatus == Enums.RoleStatus.DEAD)
                return;
            if(Hp<5 && Type ==  "Wallnut_cracked1")
            {
                Type = "Wallnut_cracked2";
                loadImage();
            }
            else if(Hp<10 && Type == "WallNut")
            {
                Type = "Wallnut_cracked1";
                loadImage();
            }
        }
    }
}
