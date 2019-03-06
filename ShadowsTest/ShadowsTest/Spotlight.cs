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
    class Spotlight
    {
        private float length, width;
        private float alpha, tanTheta, tanThetaPlusAlpha, tanThetaMinusAlpha, intercept;
        private Vector2 pos;
        private float rotation;
        private Texture2D t;

        public Vector2 Position
        {
            get { return pos; }
        }
        public float Rotation
        {
            get { return rotation; }
        }
        public float Width
        {
            get { return width; }
        }
        public float Length
        {
            get { return length; }
        }
        public float Alpha
        {
            get { return alpha; }
        }
        public float TanTheta
        {
            get { return tanTheta; }
        }
        public float TanThetaPlusAlpha
        {
            get { return tanThetaPlusAlpha; }
        }
        public float TanThetaMinusAlpha
        {
            get { return tanThetaMinusAlpha; }
        }
        public float Intercept
        {
            get { return intercept; }
        }

        public Spotlight(Vector2 init, float rot, int l, int w, Texture2D _t)
        {
            pos = init;
            rotation = (float)(rot * (Math.PI / 180));
            length = l;
            width = w;
            t = _t;
        }

        public void Update()
        {
            //rotation = (float)Math.Atan((Mouse.GetState().Position.Y - pos.Y) / (Mouse.GetState().Position.X - pos.X));
            if(Keyboard.GetState().IsKeyDown(Keys.W))
            {
                pos.Y -= 1;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.S))
            {
                pos.Y += 1;
            }

            alpha = (float)Math.Atan(Width / Length);
            tanTheta = (float)Math.Tan(Rotation);
            tanThetaPlusAlpha = (float)Math.Tan(Rotation + alpha);
            tanThetaMinusAlpha = (float)Math.Tan(Rotation - alpha);
            intercept = (float)(Length / Math.Cos(Math.PI / 2 + Rotation));
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(t, pos, Color.Black);
        }

        public static bool WithinSpotlight(Spotlight light, Vector2 point)
        {
            float q1 = ((-1 / light.tanTheta) * (point.X - light.Position.X)) + light.intercept + light.Position.Y;
            float q2 = light.tanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y;
            float q3 = light.tanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y;

            if(light.rotation != 0 && light.rotation != (Math.PI))
            {
                if(light.rotation > 0 && light.rotation < Math.PI)
                {
                    if((light.tanThetaPlusAlpha > 0 && light.tanThetaPlusAlpha < Math.PI/2) || (light.tanThetaPlusAlpha > Math.PI && light.tanThetaPlusAlpha < (3 * Math.PI) / 2))
                    {
                        if ((light.tanThetaMinusAlpha > 0 && light.tanThetaMinusAlpha < Math.PI / 2) || (light.tanThetaMinusAlpha > Math.PI && light.tanThetaMinusAlpha < (3 * Math.PI) / 2))
                        {
                            if(point.Y >= q1 && point.Y <= q2 && point.Y <= q3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (point.Y >= q1 && point.Y <= q2 && point.Y >= q3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if ((light.tanThetaMinusAlpha > 0 && light.tanThetaMinusAlpha < Math.PI / 2) || (light.tanThetaMinusAlpha > Math.PI && light.tanThetaMinusAlpha < (3 * Math.PI) / 2))
                        {
                            if (point.Y >= q1 && point.Y >= q2 && point.Y <= q3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (point.Y >= q1 && point.Y >= q2 && point.Y >= q3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if ((light.tanThetaPlusAlpha > 0 && light.tanThetaPlusAlpha < Math.PI / 2) || (light.tanThetaPlusAlpha > Math.PI && light.tanThetaPlusAlpha < (3 * Math.PI) / 2))
                    {
                        if ((light.tanThetaMinusAlpha > 0 && light.tanThetaMinusAlpha < Math.PI / 2) || (light.tanThetaMinusAlpha > Math.PI && light.tanThetaMinusAlpha < (3 * Math.PI) / 2))
                        {
                            if (point.Y <= q1 && point.Y <= q2 && point.Y <= q3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (point.Y <= q1 && point.Y <= q2 && point.Y >= q3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if ((light.tanThetaMinusAlpha > 0 && light.tanThetaMinusAlpha < Math.PI / 2) || (light.tanThetaMinusAlpha > Math.PI && light.tanThetaMinusAlpha < (3 * Math.PI) / 2))
                        {
                            if (point.Y <= q1 && point.Y >= q2 && point.Y <= q3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (point.Y <= q1 && point.Y >= q2 && point.Y >= q3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                if(light.rotation == Math.PI)
                {
                    if(point.Y <= light.length - light.pos.X && point.Y >= q2 && point.Y >= q3)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (point.Y >= light.length + light.pos.X && point.Y <= q2 && point.Y <= q3)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool WithinSpotlight(Spotlight light, Platform platform)
        {
            Vector2 point;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        point = platform.PointNW;
                        break;

                    case 1:
                        point = platform.PointNE;
                        break;

                    case 2:
                        point = platform.PointSW;
                        break;

                    case 3:
                        point = platform.PointSE;
                        break;

                    default:
                        point = new Vector2(0, 0);
                        break;
                }
                
                if(WithinSpotlight(light, point))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
