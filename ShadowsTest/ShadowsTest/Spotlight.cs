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
    class Spotlight : Light
    {
        private float length, width;
        private float alpha, tanTheta, tanThetaPlusAlpha, tanThetaMinusAlpha, intercept;
        private float rotation;
        private Texture2D t;

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

        public Spotlight(Vector2 init, float rot, int l, int w, Texture2D _t) : base(init)
        {
            rotation = (float)(rot * (Math.PI / 180));
            length = l;
            width = w;
            t = _t;
        }

        public override void Update()
        {
            rotation = Shadow.AngleFromPointToPoint(GlobalPosition, new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));
            if(Keyboard.GetState().IsKeyDown(Keys.W))
            {
                globalPosition.Y -= 3;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.S))
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

            alpha = (float)Math.Atan(Width / Length);
            tanTheta = (float)Math.Tan(Rotation);
            tanThetaPlusAlpha = (float)Math.Tan(Rotation + alpha);
            tanThetaMinusAlpha = (float)Math.Tan(Rotation - alpha);
            intercept = (float)(Length / Math.Cos(Math.PI / 2 + Rotation));
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture: t, destinationRectangle: new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, (int)width, (int)length), origin: new Vector2(0, t.Height/2), rotation: rotation);
        }

        public override bool IsWithinLight(Vector2 point)
        {
            float q1 = ((-1 / tanTheta) * (point.X - GlobalPosition.X)) + intercept + GlobalPosition.Y;
            float q2 = tanThetaPlusAlpha * (point.X - GlobalPosition.X) + GlobalPosition.Y;
            float q3 = tanThetaMinusAlpha * (point.X - GlobalPosition.X) + GlobalPosition.Y;

            if(rotation != 0 && rotation != (Math.PI))
            {
                if(rotation > 0 && rotation < Math.PI)
                {
                    if((tanThetaPlusAlpha > 0 && tanThetaPlusAlpha < Math.PI/2) || (tanThetaPlusAlpha > Math.PI && tanThetaPlusAlpha < (3 * Math.PI) / 2))
                    {
                        if ((tanThetaMinusAlpha > 0 && tanThetaMinusAlpha < Math.PI / 2) || (tanThetaMinusAlpha > Math.PI && tanThetaMinusAlpha < (3 * Math.PI) / 2))
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
                        if ((tanThetaMinusAlpha > 0 && tanThetaMinusAlpha < Math.PI / 2) || (tanThetaMinusAlpha > Math.PI && tanThetaMinusAlpha < (3 * Math.PI) / 2))
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
                    if ((tanThetaPlusAlpha > 0 && tanThetaPlusAlpha < Math.PI / 2) || (tanThetaPlusAlpha > Math.PI && tanThetaPlusAlpha < (3 * Math.PI) / 2))
                    {
                        if ((tanThetaMinusAlpha > 0 && tanThetaMinusAlpha < Math.PI / 2) || (tanThetaMinusAlpha > Math.PI && tanThetaMinusAlpha < (3 * Math.PI) / 2))
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
                        if ((tanThetaMinusAlpha > 0 && tanThetaMinusAlpha < Math.PI / 2) || (tanThetaMinusAlpha > Math.PI && tanThetaMinusAlpha < (3 * Math.PI) / 2))
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
                if(rotation == Math.PI)
                {
                    if(point.Y <= length - GlobalPosition.X && point.Y >= q2 && point.Y >= q3)
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
                    if (point.Y >= length + GlobalPosition.X && point.Y <= q2 && point.Y <= q3)
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

        public override bool IsWithinLight(Platform platform)
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
                
                if(IsWithinLight(point))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
