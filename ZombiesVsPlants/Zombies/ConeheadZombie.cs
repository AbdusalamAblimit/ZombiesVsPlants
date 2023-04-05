using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;

namespace ZombiesVsPlants.Zombies
{
    class ConeheadZombie : Zombie
    {
        public ConeheadZombie(Street street, Floor floor)
            : base(street, floor)
        {
            Power = 3;
            Hp = 10;
            Speed = 5;
            Dir = Direction.LEFT;
            Type = "ConeheadZombie";

            loadImage();
        }


        public ConeheadZombie(int x, int y)
        {
            X = x;
            Y = y;
            Hp = 3;
            Speed = 2;
            Dir = Direction.LEFT;
            Type = "ConeheadZombie";

            loadImage();
        }

        public override void Attacked()
        {
            if (this.RolesStatus != Enums.RoleStatus.DEAD)
            if (Type == "ConeheadZombie" && Hp <= 4)
            {
                Type = "Zombie";
                loadImage();
            }
        }
        //public override void Dead()
        //{
        //RolesStatus = MyEnum.RoleStatus.DISPEAR;
        //Dispear();


        //NormalZombie zombie = new NormalZombie(this.Street, this.Floor);
        //zombie.X = X;
        //zombie.Y = Y;
        // Map.addZombie(zombie);
        //t.Abort();

        //}
    }
}
