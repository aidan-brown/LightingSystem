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
        protected Vector2 globalPosition, screenPosition;

        public Vector2 GlobalPosition
        {
            get { return globalPosition; }
        }

        public Vector2 ScreenPosition
        {
            get { return screenPosition; }
        }

        protected Light(Vector2 gPos)
        {
            globalPosition = gPos;
        }

        public abstract bool IsWithinLight(Vector2 point);

        public abstract bool IsWithinLight(Platform platform);

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
