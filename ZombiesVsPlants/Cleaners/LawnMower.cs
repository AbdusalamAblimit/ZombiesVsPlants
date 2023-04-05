using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;

namespace ZombiesVsPlants.Cleaners
{
    class LawnMower : Cleaner
    {
        public LawnMower(Street street, Floor floor)
            : base(street, floor)
        {
            this.Type = "LawnMower";

            loadImage();
        }

        public override void loadImage()
        {
            base.loadImage();
        }
    }
}
