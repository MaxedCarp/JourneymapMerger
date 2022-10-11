using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneymapMerger
{
    class CoordsManager
    {
        private int x, y;

        public CoordsManager(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public CoordsManager(CoordsManager other)
        {
            this.x = other.x;
            this.y = other.y;
        }
        public int GetX()
        {
            return this.x;
        }
        public int GetY()
        {
            return this.y;
        }
        public void SetX(int x)
        {
            this.x = x;
        }
        public void SetY(int y)
        {
            this.y = y;
        }
    }
}
