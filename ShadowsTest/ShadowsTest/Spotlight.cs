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
        private Vector2 pos, p1, p2;
        private float rotation;

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

        public Spotlight(Vector2 init, float rot, int l, int w)
        {
            pos = init;
            rotation = (float)(rot * (Math.PI / 180));
            length = l;
            width = w;

            FindPoints();
        }

        private void FindPoints()
        {
            float x1 = (float)((length * Math.Cos(rotation)) - (width * Math.Sin((Math.PI/2) - rotation)));
            float x2 = (float)((length * Math.Cos(rotation)) + (width * Math.Sin((Math.PI / 2) - rotation)));
            float y1 = (float)((length * Math.Sin(rotation)) + (width * Math.Cos((Math.PI / 2) - rotation)));
            float y2 = (float)((length * Math.Sin(rotation)) - (width * Math.Cos((Math.PI / 2) - rotation)));

            p1 = new Vector2(x1, y1);
            p2 = new Vector2(x2, y2);
        }

        public void Update()
        {
            Console.WriteLine("Pos: ({0}, {1})", pos.X, pos.Y);
            Console.WriteLine("P1: ({0}, {1})", p1.X, p1.Y);
            Console.WriteLine("P2: ({0}, {1})", p2.X, p2.Y);
        }

        public static bool WithinSpotlight(Spotlight light, Vector2 point)
        {
            float alpha = (float)Math.Atan(light.Width / light.Length);
            float tanTheta = (float)Math.Tan(light.Rotation);
            float tanThetaPAlpha = (float)Math.Tan(light.Rotation + alpha);
            float tanThetaMAlpha = (float)Math.Tan(light.Rotation + alpha);
            float intercept = (float)(light.Length / Math.Cos(Math.PI / 2 + light.Rotation));
            if (light.Rotation != 0 && light.Rotation != Math.PI)
            {
                if(light.Rotation > Math.PI)
                {
                    if(light.Rotation + alpha < Math.PI / 2 && light.Rotation + alpha > (3 * Math.PI) / 2)
                    {
                        if(light.Rotation - alpha < Math.PI / 2 && light.Rotation - alpha > (3 * Math.PI) / 2)
                        {
                            if((point.Y <= -(1/tanTheta) * point.X + intercept) && (point.Y <= tanThetaPAlpha * point.X) && (point.Y >= tanThetaMAlpha * point.X))
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
                            if ((point.Y <= -(1 / tanTheta) * point.X + intercept) && (point.Y <= tanThetaPAlpha * point.X) && (point.Y <= tanThetaMAlpha * point.X))
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
                        if (light.Rotation - alpha < Math.PI / 2 && light.Rotation - alpha > (3 * Math.PI) / 2)
                        {
                            if ((point.Y <= -(1 / tanTheta) * point.X + intercept) && (point.Y >= tanThetaPAlpha * point.X) && (point.Y >= tanThetaMAlpha * point.X))
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
                            if ((point.Y <= -(1 / tanTheta) * point.X + intercept) && (point.Y >= tanThetaPAlpha * point.X) && (point.Y <= tanThetaMAlpha * point.X))
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
                    if (light.Rotation + alpha < Math.PI / 2 && light.Rotation + alpha > (3 * Math.PI) / 2)
                    {
                        if (light.Rotation - alpha < Math.PI / 2 && light.Rotation - alpha > (3 * Math.PI) / 2)
                        {
                            if ((point.Y >= -(1 / tanTheta) * point.X + intercept) && (point.Y <= tanThetaPAlpha * point.X) && (point.Y >= tanThetaMAlpha * point.X))
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
                            if ((point.Y >= -(1 / tanTheta) * point.X + intercept) && (point.Y <= tanThetaPAlpha * point.X) && (point.Y <= tanThetaMAlpha * point.X))
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
                        if (light.Rotation - alpha < Math.PI / 2 && light.Rotation - alpha > (3 * Math.PI) / 2)
                        {
                            if ((point.Y >= -(1 / tanTheta) * point.X + intercept) && (point.Y >= tanThetaPAlpha * point.X) && (point.Y >= tanThetaMAlpha * point.X))
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
                            if ((point.Y >= -(1 / tanTheta) * point.X + intercept) && (point.Y >= tanThetaPAlpha * point.X) && (point.Y <= tanThetaMAlpha * point.X))
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
                    if ((point.X <= light.Length) && (point.Y <= tanThetaPAlpha * point.X) && (point.Y >= tanThetaMAlpha * point.X))
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
                    if ((point.X >= -light.Length) && (point.Y >= tanThetaPAlpha * point.X) && (point.Y <= tanThetaMAlpha * point.X))
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
