using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            alpha = (float)Math.Atan(Width / Length);
            tanTheta = (float)Math.Tan(Rotation);
            tanThetaPlusAlpha = (float)Math.Tan(Rotation + alpha);
            tanThetaMinusAlpha = (float)Math.Tan(Rotation + alpha);
            intercept = (float)(Length / Math.Cos(Math.PI / 2 + Rotation));
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(t, pos, Color.Black);
        }

        public static bool WithinSpotlight(Spotlight light, Vector2 point)
        {
            float q1 = ((-1 / light.tanTheta) * (point.X - light.Position.X)) + light.intercept + light.Position.Y;
            float q2 = light.tanThetaPlusAlpha * (point.Y - light.Position.X) + light.Position.Y;
            float q3 = light.tanThetaMinusAlpha * (point.Y - light.Position.X) + light.Position.Y;


        }

        public static bool WithinSpotlight(Spotlight light, Platform platform)
        {
            Vector2 point;
            for (int i = 0; i < 4; i++)
            {
                switch(i)
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

                if (light.Rotation != 0 && light.Rotation != Math.PI)
                {
                    if (light.Rotation > Math.PI)
                    {
                        if (light.Rotation + light.Alpha < Math.PI / 2 && light.Rotation + light.Alpha > (3 * Math.PI) / 2)
                        {
                            if (light.Rotation - light.Alpha < Math.PI / 2 && light.Rotation - light.Alpha > (3 * Math.PI) / 2)
                            {
                                if ((point.Y <= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Intercept + light.Position.Y) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if ((point.Y <= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Intercept + light.Position.Y) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (light.Rotation - light.Alpha < Math.PI / 2 && light.Rotation - light.Alpha > (3 * Math.PI) / 2)
                            {
                                if ((point.Y <= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Intercept + light.Position.Y) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if ((point.Y <= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (light.Rotation + light.Alpha < Math.PI / 2 && light.Rotation + light.Alpha > (3 * Math.PI) / 2)
                        {
                            if (light.Rotation - light.Alpha < Math.PI / 2 && light.Rotation - light.Alpha > (3 * Math.PI) / 2)
                            {
                                if ((point.Y >= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if ((point.Y >= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (light.Rotation - light.Alpha < Math.PI / 2 && light.Rotation - light.Alpha > (3 * Math.PI) / 2)
                            {
                                if ((point.Y >= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if ((point.Y >= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (light.Rotation == 0)
                    {
                        if ((point.X <= light.Position.X + light.Length) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if ((point.X >= light.Position.X - light.Length) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
