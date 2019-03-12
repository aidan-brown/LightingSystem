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
        private List<Vector2> points;
        private Rectangle rect;
        private Shadow shadow;
        private Texture2D texture;

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
        public List<Vector2> Points
        {
            get { return points; }
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

            shadow = new Shadow(pSW, pNE, 500);

            points = new List<Vector2>();
            points.Add(pNW);
            points.Add(pNE);
            points.Add(pSW);
            points.Add(pSE);
        }

        public Platform()
        { }

        public void Update(Light light)
        {
            if (isInLight)
            {
                Vector2 p1 = new Vector2(), p2 = new Vector2();

                p1 = FindFurthestPoint(points, light);

                if (p1 == pNE)
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

        private Vector2 FindFurthestPoint(List<Vector2> points, Light light)
        {
            float maxValue = 0;
            Vector2 maxPoint = new Vector2();

            foreach (Vector2 point in points)
            {
                float theta = Shadow.AngleFromPointToPoint(light.GlobalPosition, MidPoint);
                float alpha = Shadow.AngleFromPointToPoint(light.GlobalPosition, point);
                float phi = Math.Abs(theta - alpha);
                float d = (float)Math.Sqrt(Math.Pow(point.Y - light.GlobalPosition.Y, 2) + Math.Pow(point.X - light.GlobalPosition.X, 2));

                if(maxValue < d * Math.Sin(phi))
                {
                    maxValue = (float)(d * Math.Sin(phi));
                    maxPoint = point;
                }
            }
            return maxPoint;
        }
    }
}
