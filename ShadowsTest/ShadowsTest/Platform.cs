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
        private List<Light> lights;
        private List<Shadow> shadows;
        private Rectangle rect;
        private Texture2D texture;
        private bool isInLight;

        public static List<Shadow> GlobalShadows = new List<Shadow>();

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
        public Rectangle Rect
        {
            get { return rect; }
        }

        public Platform(Rectangle r, Texture2D t, GraphicsDevice gd)
        {
            rect = r;
            texture = t;

            pNW = new Vector2(r.X, r.Y);
            pNE = new Vector2(r.X + r.Width, r.Y);
            pSW = new Vector2(r.X, r.Y + r.Height);
            pSE = new Vector2(r.X + r.Width, r.Y + r.Height);

            points = new List<Vector2>();
            points.Add(pNW);
            points.Add(pNE);
            points.Add(pSW);
            points.Add(pSE);

            lights = new List<Light>();
            shadows = new List<Shadow>();
        }

        public Platform()
        { }

        public void Update(List<Light> sceneLights)
        {
            bool hasChanged = false;
            foreach (Light light in sceneLights)
            {
                if (!hasChanged)
                {
                    hasChanged = IsInLight(light.IsWithinLight(this), light);
                }
                else
                {
                    IsInLight(light.IsWithinLight(this), light);
                }
            }
            if (hasChanged)
            {
                foreach (Light light in lights)
                {
                    if (isInLight && !DoesShadowsContainLight(light))
                    {
                        GlobalShadows.Add(new Shadow(light, this));
                        shadows.Add(new Shadow(light, this));
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        { 
            sb.Draw(texture, rect, Color.White);
        }

        public bool IsInLight(bool isInLight, Light light)
        {
            if(isInLight)
            {
                this.isInLight = true;
                if (!lights.Contains(light))
                {
                    lights.Add(light);
                    return true;
                }
            }
            else
            {
                if (lights.Contains(light))
                {
                    lights.RemoveAt(lights.IndexOf(light));
                    GlobalShadows.RemoveAt(IndexOfLightInGlobalShadows(light));
                    shadows.RemoveAt(IndexOfLightInShadows(light));
                    if (lights.Count <= 0)
                    {
                        this.isInLight = false;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool DoesShadowsContainLight(Light light)
        {
            foreach(Shadow shadow in shadows)
            {
                if(shadow.Light == light)
                {
                    return true;
                }
            }
            return false;
        }

        private static int IndexOfLightInGlobalShadows(Light light)
        {
            if (GlobalShadows.Count > 0)
            {
                for (int i = 0; i < GlobalShadows.Count; i++)
                {
                    if (GlobalShadows.ElementAt(i).Light == light)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private int IndexOfLightInShadows(Light light)
        {
            if (shadows.Count > 0)
            {
                for (int i = 0; i < shadows.Count; i++)
                {
                    if (shadows.ElementAt(i).Light == light)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public static void UpdateShadows()
        {
            foreach (Shadow shadow in GlobalShadows)
            {
                shadow.FindShadow();
            }
        }

        public static void DrawShadows(BasicEffect basicEffect, GraphicsDevice graphicsDevice, VertexBuffer vertexBuffer)
        {
            foreach (Shadow shadow in GlobalShadows)
            {
                shadow.Draw(basicEffect, graphicsDevice, vertexBuffer);
            }
        }
    }
}
