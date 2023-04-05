using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;

namespace ZombiesVsPlants.Zombies
{
    class NormalZombie : Zombie
    {
        public NormalZombie(Street street, Floor floor)
            : base(street, floor)
        {
            Power = 1;
            Hp = 3;
            Speed = 5;
            Dir = Direction.LEFT;
            Type = "Zombie";

            loadImage();
        }


        public NormalZombie(int x, int y)
        {
            X = x;
            Y = y;
            Hp = 1;
            Speed = 2;
            Dir = Direction.LEFT;
            Type = "Zombie";

            loadImage();
        }
    }
}
