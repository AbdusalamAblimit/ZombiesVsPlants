using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;

namespace ZombiesVsPlants.Zombies
{
    class BucketheadZombie : Zombie
    {
        public BucketheadZombie(Street street, Floor floor)
            : base(street, floor)
        {
            Power = 3;
            Hp = 18;
            Speed = 5;
            Dir = Direction.LEFT;
            Type = "BucketheadZombie";

            loadImage();
        }


        public BucketheadZombie(int x, int y)
        {
            X = x;
            Y = y;
            Hp = 3;
            Speed = 2;
            Dir = Direction.LEFT;
            Type = "BucketheadZombie";

            loadImage();
        }

        public override void Attacked()
        {
            if (this.RolesStatus != Enums.RoleStatus.DEAD)
            if (Type == "BucketheadZombie" && Hp <= 4)
            {
                Type = "Zombie";
                loadImage();
            }
        }
    }
}
