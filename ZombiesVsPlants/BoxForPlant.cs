using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication3
{
    class BoxForPlant
    {
        private Street s;

        internal Street S
        {
            get { return s; }
            set { s = value; }
        }
        private Floor f;

        public Floor F
        {
            get { return f; }
            set { f = value; }
        }

        public BoxForPlant(Street s,Floor f)
        {
            this.s = s;
            this.f = f;
        }
    }
}
