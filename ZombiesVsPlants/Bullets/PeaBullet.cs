using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;
using ZombiesVsPlants.Enums;

namespace ZombiesVsPlants.Bullets
{
    class PeaBullet : Bullet
    {
        public PeaBullet(Street s, Floor f, Direction dir):base(s,f,dir)
        {
            Speed = 10;
            Type = "PeaBullet";
            RolesStatus = RoleStatus.MOVE;

            loadImage();
        }

        public override void Dead()
        {
            Images = new Resources().PeaBulletHit();
            base.Dead();
        }
        public override void Attack()
        {
            
            base.Attack();
            Random rd = new Random();
            lock (Map.p.sounds_queue_lock)
                Map.p.sounds_queue.Enqueue("splat" + Convert.ToString(rd.Next(1, 3)));
            
        }
    }
}
