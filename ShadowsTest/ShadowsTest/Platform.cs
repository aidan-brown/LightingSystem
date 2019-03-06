using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ShadowsTest
{
    class Platform
    {
        List<Vector2> points;
        Rectangle rect, shadowRect;
        Shadow shadow;
        Texture2D texture;
        float lightRot;

        Vector2 pNW, pNE, pSW, pSE;

        public Vector2 PointNW
        {
            get { return pNW; }
        }
        public Vector2 PointNE
        {
            get { return pNE; }
        }
        public Vector2 PointSW
        {
            get { return pSW; }
        }
        public Vector2 PointSE
        {
            get { return pSE; }
        }
        public Vector2 MidPoint
        {
            get { return new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2); }
        }

        public bool isInLight;

        public Platform(Rectangle r, Texture2D t, GraphicsDevice gd)
        {
            rect = r;
            texture = t;

            pNW = new Vector2(r.X, r.Y);
            pNE = new Vector2(r.X + r.Width, r.Y);
            pSW = new Vector2(r.X, r.Y + r.Height);
            pSE = new Vector2(r.X + r.Width, r.Y + r.Height);

            shadow = new Shadow(pSW, pNE, 150);

            points = new List<Vector2>();
            points.Add(pNW);
            points.Add(pNE);
            points.Add(pSW);
            points.Add(pSE);
        }

        public Platform()
        { }

        public void Update(Spotlight light)
        {
            if (isInLight)
            {
                Vector2 p1 = new Vector2(), p2 = new Vector2();

                int i = FindFurthestPoint(points, light);
                p1 = points.ElementAt(i);
                
                if(p1 == pNE)
                {
                    p2 = pSW;
                }
                else if(p1 == pNW)
                {
                    p2 = pSE;
                }
                else if (p1 == pSW)
                {
                    p2 = pNE;
                }
                else if (p1 == pSE)
                {
                    p2 = pNW;
                }
                shadow.FindShadow(p1, p2, light, this);
            }
        }

        public void Draw(SpriteBatch sb, BasicEffect basicEffect, GraphicsDevice graphicsDevice, VertexBuffer vertexBuffer)
        { 
            if(isInLight)
            {
                shadow.Draw(basicEffect, graphicsDevice, vertexBuffer);
            }
            sb.Draw(texture, rect, Color.White);
        }

        private int FindFurthestPoint(List<Vector2> points, Spotlight light)
        {
            int maxDIndex = 0;
            double maxD = 0;
            Vector2 point;
            for (int i = 0; i < points.Count; i++)
            {
                point = points.ElementAt(i);
                if (point.X != light.Position.X || point.Y != light.Position.Y)
                {
                    double m = Shadow.AngleFromPointToPoint(point, light.Position);
                    Vector2 poi = new Vector2((float)((Math.Pow(m, 2) * light.Position.X + point.X + m * point.Y - m * light.Position.Y) / (Math.Pow(m, 2) - 1)), (float)((point.X / m) - ((Math.Pow(m, 2) * light.Position.X + point.X + m * point.Y - m * light.Position.Y) / (Math.Pow(m, 3) - m)) + point.Y));

                    double q1 = Math.Sqrt(Math.Pow(poi.Y - point.Y, 2) + Math.Pow(poi.X - point.X, 2));
                    if (maxD < q1)
                    {
                        maxD = q1;
                        maxDIndex = i;
                    }
                }
                else
                {
                    return i;
                }
            }

            return maxDIndex;
        }
    }
}
