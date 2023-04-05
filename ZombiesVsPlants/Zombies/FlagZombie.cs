using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;

namespace ZombiesVsPlants.Zombies
{
    class FlagZombie : Zombie
    {
        public FlagZombie(Street street, Floor floor)
            : base(street, floor)
        {
            Power = 3;
            Hp = 6;
            Speed = 5;
            Dir = Direction.LEFT;
            Type = "FlagZombie";

            loadImage();
        }


        public FlagZombie(int x, int y)
        {
            X = x;
            Y = y;
            Hp = 3;
            Speed = 2;
            Dir = Direction.LEFT;
            Type = "FlagZombie";

            loadImage();
        }

        public override void Attacked()
        {
            if(this.RolesStatus != Enums.RoleStatus.DEAD)
            if (Type == "FlagZombie" && Hp <= 3)
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
