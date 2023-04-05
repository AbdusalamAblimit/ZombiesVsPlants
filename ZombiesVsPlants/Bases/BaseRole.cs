using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;
using ZombiesVsPlants.Enums;

namespace ZombiesVsPlants
{
    //可移动，可攻击角色的基类
    class BaseRole : BaseObject
    {
        private int speed;
        private int hp;
        private Direction dir;
        private Street street;
        private Floor floor;
        private RoleStatus rolesStatus;
        //敌人
        private BaseRole enemy;

        public BaseRole(Street s, Floor f)
            : base((int)s, (int)f)
        {
            street = s;
            floor = f;
        }

        public BaseRole()
        {

        }

        public virtual void Run()
        {

        }

        public virtual void Dead()
        {

        }

        public virtual void Dispear()
        {

        }

        public virtual void Attack()
        {
            Enemy.Hp--;
            if (Enemy.Hp == 0)
            {
                Enemy.RolesStatus = RoleStatus.DEAD;
                RolesStatus = RoleStatus.MOVE;
            }
        }
        public virtual void Attacked()
        {

        }

        

        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }

        internal RoleStatus RolesStatus
        {
            get { return rolesStatus; }
            set { rolesStatus = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Direction Dir
        {
            get { return dir; }
            set { dir = value; }
        }

        internal BaseRole Enemy
        {
            get { return enemy; }
            set { enemy = value; }
        }

        public Floor Floor
        {
            get { return floor; }
            set { floor = value; }
        }

        public Street Street
        {
            get { return street; }
            set { street = value; }
        }
    }
}
