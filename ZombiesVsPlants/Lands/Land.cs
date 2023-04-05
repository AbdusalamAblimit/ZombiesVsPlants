using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication3;
using ZombiesVsPlants.Plants;

namespace ZombiesVsPlants.Lands
{
    class Land : BaseObject
    {
        private Street street;
        private Floor floor;
        private bool isEmpty;

        public Land(Street s, Floor f)
        {
            this.street = s;
            this.floor = f;
            isEmpty = true;
        }

        public void Clear()
        {
            isEmpty = true;
        }

        public virtual Plant GrowPlant(string type)
        {
            if (isEmpty && Controller.gameStatus == GameStatus.START)
            {
                //创建植物
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                Type classType = assembly.GetType("ZombiesVsPlants.Plants." + type);
                Plant p = (Plant)Activator.CreateInstance(classType);
                //初始化
                p.Instance(this.Street, this.Floor);

                isEmpty = false;
                p.Land = this;

                return p;
            }
            else
                return null;
        }
        public virtual void makeEmpty()
        {
            IsEmpty = true;
        }

        public Floor Floor
        {
            get { return floor; }
            set { floor = value; }
        }

        internal Street Street
        {
            get { return street; }
            set { street = value; }
        }

        public bool IsEmpty
        {
            get { return isEmpty; }
            set { isEmpty = value; }
        }
    }
}
