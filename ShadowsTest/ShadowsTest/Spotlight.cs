using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShadowsTest
{
    class Spotlight
    {
        int length, width;
        Vector2 pos, p1, p2;
        float rotation;

        public Spotlight(Vector2 init, float rot, int l, int w)
        {
            pos = init;
            rotation = rot;
            length = l;
            width = w;

            FindPoints();
        }

        private void FindPoints()
        {
            float x1 = (float)((length * Math.Cos(rotation)) - (width * Math.Sin(90 - rotation)));
            float x2 = (float)((length * Math.Cos(rotation)) + (width * Math.Sin(90 - rotation)));
            float y1 = (float)((length * Math.Sin(rotation)) + (width * Math.Cos(90 - rotation)));
            float y2 = (float)((length * Math.Sin(rotation)) - (width * Math.Cos(90 - rotation)));

            p1 = new Vector2(x1, y1);
            p2 = new Vector2(x2, y2);
        }

        public void Update()
        {
            Console.WriteLine("Pos: ({0}, {1})", pos.X, pos.Y);
            Console.WriteLine("P1: ({0}, {1})", p1.X, p1.Y);
            Console.WriteLine("P2: ({0}, {1})", p2.X, p2.Y);
        }
    }
}
