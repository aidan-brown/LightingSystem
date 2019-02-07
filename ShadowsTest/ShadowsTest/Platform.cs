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
        Rectangle rect, shadowRect;
        Texture2D texture;
        float lightRot;
        SpriteBatch sb;

        public bool isInLight;

        public Platform(Rectangle r, Texture2D t, SpriteBatch s)
        {
            rect = r;
            texture = t;
            sb = s;
        }

        public void Update(float light)
        {
            if (isInLight)
            {
                shadowRect = new Rectangle(rect.X + texture.Width / 2, rect.Y + texture.Height / 2, rect.Width * 4, (int)Math.Sqrt(Math.Pow(rect.Width, 2) + Math.Pow(rect.Height, 2)));
                lightRot = light;
            }
        }

        public void Draw()
        { 
            if(isInLight)
            {
                sb.Draw(texture, destinationRectangle: shadowRect, color: Color.Black, origin: new Vector2(0, rect.Height / 2));
            }
            sb.Draw(texture, rect, Color.White);
        }
    }
}
