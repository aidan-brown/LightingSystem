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
        private float radius;
        private Texture2D sprite;

        public float Radius
        {
            get { return radius; }
        }

        public PointLight(Vector2 gPos, float radius, Texture2D sprite) : base(gPos)
        {
            this.radius = radius;
            this.sprite = sprite;
        }

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: sprite, position: GlobalPosition, color: Color.White, scale: new Vector2(2* radius / sprite.Width, 2 * radius / sprite.Height), origin: new Vector2(sprite.Width/2, sprite.Height/2));
        }

        public override bool IsWithinLight(Vector2 point)
        {
            float d = (float)Math.Sqrt(Math.Pow(point.Y - GlobalPosition.Y, 2) + Math.Pow(point.X - GlobalPosition.X, 2));
            return d <= radius;
        }

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
    }
}
