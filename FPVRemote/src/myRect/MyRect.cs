using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPVRemote.myRect
{
    class MyRect
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public int xMax {
            get { return this.x + this.w - 1; }
        }
        public int yMax
        {
            get { return this.y + this.h - 1; }
        }


        public MyRect(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }


        public bool contains(int x, int y) {
            return (x >= this.x && x < this.x + this.w
                && y >= this.y && y < this.y + this.h);
        }
    }
}
