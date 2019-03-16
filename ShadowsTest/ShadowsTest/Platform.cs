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
        //Stores the verticies of the platform
        private List<Vector2> points;

        //List of all lights that the platform is within
        private List<Light> lights;

        //List of all shadows produced by the platform
        private List<Shadow> shadows;

        //The rectangle for drawing the platform
        private Rectangle rect;

        //The texture of the platform
        private Texture2D texture;

        //Whether the platform is in light or not
        private bool isInLight;

        //A List of all the curent shadows in the game
        public static List<Shadow> GlobalShadows = new List<Shadow>();

        //Stores the corner verticies of the platform
        private Vector2 pNW, pNE, pSW, pSE;

        //Property to get the NW point of the platform
        public Vector2 PointNW
        {
            get { return pNW; }
        }

        //Property to get the NE point of the platform
        public Vector2 PointNE
        {
            get { return pNE; }
        }

        //Property to get the SW point of the platform
        public Vector2 PointSW
        {
            get { return pSW; }
        }

        //Property to get the SE point of the platform
        public Vector2 PointSE
        {
            get { return pSE; }
        }

        //Property to get the Midpoint of the platform
        public Vector2 MidPoint
        {
            get { return new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2); }
        }

        //Property to get the list of verticies
        public List<Vector2> Points
        {
            get { return points; }
        }

        //Property to get the rectangle of the platform
        public Rectangle Rect
        {
            get { return rect; }
        }

        /// <summary>
        /// Constructs the platform given the rectangle and texture
        /// </summary>
        /// <param name="r"></param>
        /// <param name="t"></param>
        /// <param name="gd"></param>
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

        /// <summary>
        /// Platform's update function that updates the platform every frame
        /// </summary>
        /// <param name="sceneLights"></param>
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
                        GlobalShadows.Add(new Shadow(light, this, light.GetLength()));
                        shadows.Add(new Shadow(light, this, light.GetLength()));
                    }
                }
            }
        }

        /// <summary>
        /// Platform's Draw method that is responsilbe for drawing the platform evrey frame
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        { 
            sb.Draw(texture, rect, Color.White);
        }

        /// <summary>
        /// Adds the light to the list of lights and removes the light if the platform is no longer within the lightw
        /// </summary>
        /// <param name="isInLight"></param>
        /// <param name="light"></param>
        /// <returns></returns>
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
                    GlobalShadows.RemoveAt(IndexOfGlobalShadows(light, this));
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

        /// <summary>
        /// Checks to see if the light is contained of the in the list of shadows
        /// </summary>
        /// <param name="light"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the index of the shadow in global shadows with the desired light and platform
        /// </summary>
        /// <param name="light"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        private static int IndexOfGlobalShadows(Light light, Platform platform)
        {
            if (GlobalShadows.Count > 0)
            {
                for (int i = 0; i < GlobalShadows.Count; i++)
                {
                    if (GlobalShadows.ElementAt(i).Light == light && GlobalShadows.ElementAt(i).Platform == platform)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the index of the shadow with the desired light
        /// </summary>
        /// <param name="light"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates all the shadows connected to this platform
        /// </summary>
        public static void UpdateShadows()
        {
            foreach (Shadow shadow in GlobalShadows)
            {
                shadow.FindShadow();
            }
        }

        
    }
}
