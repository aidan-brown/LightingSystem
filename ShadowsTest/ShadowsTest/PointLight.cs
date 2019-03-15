using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShadowsTest
{
    class PointLight : Light
    {
        //Radius of the point light
        private float radius;

        //Property of the light's radius
        public float Radius
        {
            get { return radius; }
        }

        /// <summary>
        /// Constructs the point light given a global position, radius, and texture
        /// </summary>
        /// <param name="globalPos"></param>
        /// <param name="radius"></param>
        /// <param name="sprite"></param>
        public PointLight(Vector2 globalPos, float radius, Texture2D lightMask) : base(globalPos, lightMask)
        {
            this.radius = radius;
        }

        /// <summary>
        /// Update function for the light
        /// </summary>
        public override void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                globalPosition.Y -= 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                globalPosition.Y += 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                globalPosition.X += 3;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                globalPosition.X -= 3;
            }
        }

        /// <summary>
        /// Draw function for the light
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture: sprite, position: GlobalPosition, color: Color.White, scale: new Vector2(2* radius / sprite.Width, 2 * radius / sprite.Height), origin: new Vector2(sprite.Width/2, sprite.Height/2));
            
            //draw light mask where there should be torches etc...
            spriteBatch.Draw(texture: lightMask, position: GlobalPosition, color: Color.White, scale: new Vector2(2 * radius / lightMask.Width, 2 * radius / lightMask.Height), origin: new Vector2(lightMask.Width / 2, lightMask.Height / 2));
        }

        /// <summary>
        /// Checks to see if the point is within the light
        /// </summary>
        /// <param name="point"></param>
        public override bool IsWithinLight(Vector2 point)
        {
            float d = (float)Math.Sqrt(Math.Pow(point.Y - GlobalPosition.Y, 2) + Math.Pow(point.X - GlobalPosition.X, 2));
            return d <= radius;
        }

        /// <summary>
        /// Checks to see if the platform is within the light
        /// </summary>
        /// <param name="platform"></param>
        public override bool IsWithinLight(Platform platform)
        {
            foreach(Vector2 point in platform.Points)
            {
                if(IsWithinLight(point))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the rectangle is within the light
        /// </summary>
        /// <param name="rect"></param>
        public override bool IsWithinLight(Rectangle rect)
        {
            Vector2[] points = { new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y), new Vector2(rect.X, rect.Y + rect.Height), new Vector2(rect.X + rect.Width, rect.Y + rect.Height) };
            foreach(Vector2 point in points)
            {
                if(IsWithinLight(point))
                {
                    return true;
                }
            }
            return false;
        }

        public override float GetLength()
        {
            return radius;
        }
    }
}
