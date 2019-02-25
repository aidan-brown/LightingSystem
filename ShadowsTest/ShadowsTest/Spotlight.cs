using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShadowsTest
{
    class Spotlight
    {
        private float length, width;
        private float alpha, tanTheta, tanThetaPlusAlpha, tanThetaMinusAlpha, intercept;
        private Vector2 pos;
        private float rotation;

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

        public Spotlight(Vector2 init, float rot, int l, int w)
        {
            pos = init;
            rotation = (float)(rot * (Math.PI / 180));
            length = l;
            width = w;
        }

        public void Update()
        {
            alpha = (float)Math.Atan(Width / Length);
            tanTheta = (float)Math.Tan(Rotation);
            tanThetaPlusAlpha = (float)Math.Tan(Rotation + alpha);
            tanThetaMinusAlpha = (float)Math.Tan(Rotation + alpha);
            intercept = (float)(Length / Math.Cos(Math.PI / 2 + Rotation));
        }

        public static bool WithinSpotlight(Spotlight light, Vector2 point)
        {
            if (light.Rotation != 0 && light.Rotation != Math.PI)
            {
                if(light.Rotation > Math.PI)
                {
                    if(light.Rotation + light.Alpha < Math.PI / 2 && light.Rotation + light.Alpha > (3 * Math.PI) / 2)
                    {
                        if(light.Rotation - light.Alpha < Math.PI / 2 && light.Rotation - light.Alpha > (3 * Math.PI) / 2)
                        {
                            if((point.Y <= -(1/light.TanTheta) * (point.X - light.Position.X) + light.Intercept + light.Position.Y) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
                            if ((point.Y <= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Intercept + light.Position.Y) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
                        if (light.Rotation - light.Alpha < Math.PI / 2 && light.Rotation - light.Alpha > (3 * Math.PI) / 2)
                        {
                            if ((point.Y <= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Intercept + light.Position.Y) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
                            if ((point.Y <= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
                    if (light.Rotation + light.Alpha < Math.PI / 2 && light.Rotation + light.Alpha > (3 * Math.PI) / 2)
                    {
                        if (light.Rotation - light.Alpha < Math.PI / 2 && light.Rotation - light.Alpha > (3 * Math.PI) / 2)
                        {
                            if ((point.Y >= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
                            if ((point.Y >= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
                        if (light.Rotation - light.Alpha < Math.PI / 2 && light.Rotation - light.Alpha > (3 * Math.PI) / 2)
                        {
                            if ((point.Y >= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
                            if ((point.Y >= -(1 / light.TanTheta) * (point.X - light.Position.X) + light.Position.Y + light.Intercept) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
                if(light.Rotation == 0)
                {
                    if ((point.X <= light.Position.X + light.Length) && (point.Y <= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y >= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
                    if ((point.X >= light.Position.X - light.Length) && (point.Y >= light.TanThetaPlusAlpha * (point.X - light.Position.X) + light.Position.Y) && (point.Y <= light.TanThetaMinusAlpha * (point.X - light.Position.X) + light.Position.Y))
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
}
