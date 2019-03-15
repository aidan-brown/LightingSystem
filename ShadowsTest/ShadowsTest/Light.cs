using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShadowsTest
{
    abstract class Light
    {
        //Stores the global and screen position respectively
        protected Vector2 globalPosition, screenPosition;

        //Stores the sprite of the light
        protected Texture2D sprite;

        //Property of the light's global position
        public Vector2 GlobalPosition
        {
            get { return globalPosition; }
        }

        //Property of the light's screen position
        public Vector2 ScreenPosition
        {
            get { return screenPosition; }
        }

        /// <summary>
        /// Constructs the light given a initial global position and texture
        /// </summary>
        /// <param name="gPos"></param>
        protected Light(Vector2 gPos, Texture2D s)
        {
            globalPosition = gPos;
            sprite = s;
        }

        /// <summary>
        /// Checks to see if the point is within the light
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract bool IsWithinLight(Vector2 point);

        /// <summary>
        /// Checks to see if the platform is within the light
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public abstract bool IsWithinLight(Platform platform);

        /// <summary>
        /// Checks to see if the rectangle is within the light
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public abstract bool IsWithinLight(Rectangle rect);

        /// <summary>
        /// Update function for the light
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Draw function for the light
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
