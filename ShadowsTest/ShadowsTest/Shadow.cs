using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShadowsTest
{
    class Shadow
    {
        private Light light;
        private Platform platform;
        private VertexPositionColor[] tri1, tri2, tri3;
        private Vector2 p1, p2, p1f, p2f;
        private const float Length = 250;

        public Light Light
        {
            get { return light; }
        }

        public Shadow(Light light, Platform platform)
        {
            tri1 = new VertexPositionColor[3];
            tri2 = new VertexPositionColor[3];
            tri3 = new VertexPositionColor[3];

            this.light = light;
            this.platform = platform;

            FindShadow();
        }

        public void FindShadow()
        {
            FindPoints(platform, light);

            tri1[0] = new VertexPositionColor(new Vector3(p1.X, p1.Y, 0), Color.Black);
            tri1[1] = new VertexPositionColor(new Vector3(p1f.X, p1f.Y, 0), Color.Transparent);
            tri1[2] = new VertexPositionColor(new Vector3(p2f.X, p2f.Y, 0), Color.Transparent);
            
            tri2[0] = new VertexPositionColor(new Vector3(p2.X, p2.Y, 0), Color.Transparent);
            tri2[1] = new VertexPositionColor(new Vector3(p1f.X, p1f.Y, 0), Color.Transparent);
            tri2[2] = new VertexPositionColor(new Vector3(p2f.X, p2f.Y, 0), Color.Transparent);

            tri3[0] = new VertexPositionColor(new Vector3(p1.X, p1.Y, 0), Color.Black);
            tri3[1] = new VertexPositionColor(new Vector3(p2.X, p2.Y, 0), Color.Black);
            tri3[2] = new VertexPositionColor(new Vector3(p2f.X, p2f.Y, 0), Color.Transparent);


        }

        public void Draw(BasicEffect basicEffect, GraphicsDevice graphicsDevice, VertexBuffer vertexBuffer)
        {
            vertexBuffer.SetData<VertexPositionColor>(tri1);
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            vertexBuffer.SetData<VertexPositionColor>(tri2);
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            vertexBuffer.SetData<VertexPositionColor>(tri3);
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }
        }

        private void FindPoints(Platform platform, Light light)
        {
            if (light != null)
            {
                float offset = platform.Rect.Width / 2;

                if ((platform.MidPoint.X + offset < light.GlobalPosition.X || platform.MidPoint.X - offset > light.GlobalPosition.X) && (platform.MidPoint.Y + offset < light.GlobalPosition.Y || platform.MidPoint.Y - offset > light.GlobalPosition.Y))
                {
                    p1 = FindFurthestPoint(platform.Points, light, platform);

                    if (p1 == platform.PointNE)
                    {
                        p2 = platform.PointSW;
                    }
                    else if (p1 == platform.PointNW)
                    {
                        p2 = platform.PointSE;
                    }
                    else if (p1 == platform.PointSW)
                    {
                        p2 = platform.PointNE;
                    }
                    else if (p1 == platform.PointSE)
                    {
                        p2 = platform.PointNW;
                    }
                }
                else
                {
                    if (platform.MidPoint.X + offset >= light.GlobalPosition.X && platform.MidPoint.X - offset <= light.GlobalPosition.X)
                    {
                        if (platform.MidPoint.Y > light.GlobalPosition.Y)
                        {
                            p1 = platform.PointNW;
                            p2 = platform.PointNE;
                        }
                        else
                        {
                            p1 = platform.PointSW;
                            p2 = platform.PointSE;
                        }
                    }
                    else
                    {
                        if (platform.MidPoint.X > light.GlobalPosition.X)
                        {
                            p1 = platform.PointNW;
                            p2 = platform.PointSW;
                        }
                        else
                        {
                            p1 = platform.PointNE;
                            p2 = platform.PointSE;
                        }
                    }
                }

                if(p1.Y < p2.Y)
                {
                    Vector2 temp = p1;
                    p1 = p2;
                    p2 = temp;
                }

                float phiSlope = ((p1.Y - light.GlobalPosition.Y) / (p1.X - light.GlobalPosition.X));
                Vector2 phiPoint = p1;
                float alphaSlope = ((p2.Y - light.GlobalPosition.Y) / (p2.X - light.GlobalPosition.X));
                Vector2 alphaPoint = p2;
                float orthoSlope = 1 /((platform.MidPoint.Y - light.GlobalPosition.Y) / (platform.MidPoint.X - light.GlobalPosition.X));
                Vector2 orthoPoint;

                if (platform.MidPoint.X != light.GlobalPosition.X && platform.MidPoint.Y != light.GlobalPosition.Y)
                {
                    if (platform.MidPoint.X > light.GlobalPosition.X)
                    {
                        orthoPoint = new Vector2((float)(platform.MidPoint.X + Length * Math.Cos(Math.Atan(1 / orthoSlope))), (float)(platform.MidPoint.Y + Length * Math.Sin(Math.Atan(1 / orthoSlope))));
                    }
                    else
                    {
                        orthoPoint = new Vector2((float)(platform.MidPoint.X - Length * Math.Cos(Math.Atan(1 / orthoSlope))), (float)(platform.MidPoint.Y - Length * Math.Sin(Math.Atan(1 / orthoSlope))));
                    }

                    if (p1.X != light.GlobalPosition.X && p2.X != light.GlobalPosition.X)
                    {
                        p1f = new Vector2((orthoSlope * orthoPoint.X + phiSlope * phiPoint.X + orthoPoint.Y - phiPoint.Y) / (orthoSlope + phiSlope), phiSlope * (((orthoSlope * orthoPoint.X + phiSlope * phiPoint.X + orthoPoint.Y - phiPoint.Y) / (orthoSlope + phiSlope)) - phiPoint.X) + phiPoint.Y);
                        p2f = new Vector2((orthoSlope * orthoPoint.X + alphaSlope * alphaPoint.X + orthoPoint.Y - alphaPoint.Y) / (orthoSlope + alphaSlope), alphaSlope * (((orthoSlope * orthoPoint.X + alphaSlope * alphaPoint.X + orthoPoint.Y - alphaPoint.Y) / (orthoSlope + alphaSlope)) - alphaPoint.X) + alphaPoint.Y);
                    }
                    else
                    {
                        if(p1.X == light.GlobalPosition.X)
                        {
                            if (platform.MidPoint.Y > light.GlobalPosition.Y)
                            {
                                p1f = new Vector2(p1.X, -orthoSlope * (p1.X - orthoPoint.X) + orthoPoint.Y);
                                p2f = new Vector2((orthoSlope * orthoPoint.X + alphaSlope * alphaPoint.X + orthoPoint.Y - alphaPoint.Y) / (orthoSlope + alphaSlope), alphaSlope * (((orthoSlope * orthoPoint.X + alphaSlope * alphaPoint.X + orthoPoint.Y - alphaPoint.Y) / (orthoSlope + alphaSlope)) - alphaPoint.X) + alphaPoint.Y);
                            }
                        }
                        else
                        {
                            p1f = new Vector2((orthoSlope * orthoPoint.X + phiSlope * phiPoint.X + orthoPoint.Y - phiPoint.Y) / (orthoSlope + phiSlope), phiSlope * (((orthoSlope * orthoPoint.X + phiSlope * phiPoint.X + orthoPoint.Y - phiPoint.Y) / (orthoSlope + phiSlope)) - phiPoint.X) + phiPoint.Y);
                            p2f = new Vector2(p2.X, -orthoSlope * (p2.X - orthoPoint.X) + orthoPoint.Y);
                        }
                    }
                }
                else
                {
                    if(platform.MidPoint.X == light.GlobalPosition.X)
                    {
                        if(platform.MidPoint.Y > light.GlobalPosition.Y)
                        {
                            phiSlope = ((p1.Y - light.GlobalPosition.Y) / (p1.X - light.GlobalPosition.X));
                            alphaSlope = ((p2.Y - light.GlobalPosition.Y) / (p2.X - light.GlobalPosition.X));
                            p1f = new Vector2((1 / phiSlope) * (p1.Y + Length - light.GlobalPosition.Y) + light.GlobalPosition.X, p1.Y + Length);
                            p2f = new Vector2((1 / alphaSlope) * (p2.Y + Length - light.GlobalPosition.Y) + light.GlobalPosition.X, p2.Y + Length);
                        }
                        else
                        {
                            phiSlope = ((p1.Y - light.GlobalPosition.Y) / (p1.X - light.GlobalPosition.X));
                            alphaSlope = ((p2.Y - light.GlobalPosition.Y) / (p2.X - light.GlobalPosition.X));
                            p1f = new Vector2((1 / phiSlope) * (p1.Y - Length - light.GlobalPosition.Y) + light.GlobalPosition.X, p1.Y - Length);
                            p2f = new Vector2((1 / alphaSlope) * (p2.Y - Length - light.GlobalPosition.Y) + light.GlobalPosition.X, p2.Y - Length);
                        }
                    }
                    else
                    {
                        if (platform.MidPoint.X > light.GlobalPosition.X)
                        {
                            phiSlope = ((p1.Y - light.GlobalPosition.Y) / (p1.X - light.GlobalPosition.X));
                            alphaSlope = ((p2.Y - light.GlobalPosition.Y) / (p2.X - light.GlobalPosition.X));
                            p1f = new Vector2(p1.X + Length, phiSlope * (p1.X + Length - light.GlobalPosition.X) + light.GlobalPosition.Y);
                            p2f = new Vector2(p2.X + Length, alphaSlope * (p2.X + Length - light.GlobalPosition.X) + light.GlobalPosition.Y);
                        }
                        else
                        {
                            phiSlope = ((p1.Y - light.GlobalPosition.Y) / (p1.X - light.GlobalPosition.X));
                            alphaSlope = ((p2.Y - light.GlobalPosition.Y) / (p2.X - light.GlobalPosition.X));
                            p1f = new Vector2(p1.X - Length, phiSlope * (p1.X - Length - light.GlobalPosition.X) + light.GlobalPosition.Y);
                            p2f = new Vector2(p2.X - Length, alphaSlope * (p2.X - Length - light.GlobalPosition.X) + light.GlobalPosition.Y);
                        }
                    }
                }
            }
        }

        private Vector2 FindFurthestPoint(List<Vector2> points, Light light, Platform platform)
        {
            float maxValue = 0;
            Vector2 maxPoint = new Vector2();

            foreach (Vector2 point in points)
            {
                float theta = AngleFromPointToPoint(light.GlobalPosition, platform.MidPoint);
                float alpha = AngleFromPointToPoint(light.GlobalPosition, point);
                float phi = Math.Abs(theta - alpha);
                float d = (float)Math.Sqrt(Math.Pow(point.Y - light.GlobalPosition.Y, 2) + Math.Pow(point.X - light.GlobalPosition.X, 2));

                if (maxValue < d * Math.Sin(phi))
                {
                    maxValue = (float)(d * Math.Sin(phi));
                    maxPoint = point;
                }
            }
            return maxPoint;
        }

        public static float AngleFromPointToPoint(Vector2 p1, Vector2 p2)
        {
            if (p1.X < p2.X)
            {
                return (float)(Math.Atan((p1.Y - p2.Y) / (p1.X - p2.X)));
            }
            return (float)(Math.Atan((p1.Y - p2.Y) / (p1.X - p2.X)) + Math.PI);
        }

        public bool WithinShadow(Vector2 point)
        {
            float q1 = ((p1.Y - p2.Y) / (p1.X - p2.X)) * (point.X - p1.X) + p1.Y;
            float q2 = ((p1f.Y - p2f.Y) / (p1f.X - p2f.X)) * (point.X - p1f.X) + p1f.Y;
            float q3 = ((p1.Y - p1f.Y) / (p1.X - p1f.X)) * (point.X - p1.X) + p1.Y;
            float q4 = ((p2.Y - p2f.Y) / (p2.X - p2f.X)) * (point.X - p2.X) + p2.Y;

            if(light.GlobalPosition.Y > platform.MidPoint.Y)
            {
                if(light.GlobalPosition.X > platform.MidPoint.X && light.GlobalPosition.Y < platform.Rect.Bottom)
                {
                    if (point.Y >= q1 && point.Y >= q2 && point.Y <= q3 && point.Y >= q4)
                    {
                        return true;
                    }
                }
                else if(p1.Y != p2.Y)
                {
                    if(point.Y <= q1 && point.Y >= q2 && point.Y <= q3 && point.Y >= q4)
                    {
                        return true;
                    }
                }
                else
                {
                    if (point.Y <= q1 && point.Y >= q2 && point.Y <= q3 && point.Y <= q4)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (light.GlobalPosition.X < platform.MidPoint.X && light.GlobalPosition.Y > platform.Rect.Top)
                {
                    if (point.Y >= q1 && point.Y >= q2 && point.Y <= q3 && point.Y >= q4)
                    {
                        return true;
                    }
                }
                else if (p1.Y != p2.Y)
                {
                    if (point.Y >= q1 && point.Y <= q2 && point.Y <= q3 && point.Y >= q4)
                    {
                        return true;
                    }
                }
                else
                {
                    if (point.Y >= q1 && point.Y <= q2 && point.Y >= q3 && point.Y >= q4)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool WithinShadow(Rectangle rectangle)
        {
            Vector2[] points = { new Vector2(rectangle.Location.X, rectangle.Location.Y), new Vector2(rectangle.Location.X + rectangle.Width, rectangle.Location.Y), new Vector2(rectangle.Location.X, rectangle.Location.Y + rectangle.Height), new Vector2(rectangle.Location.X + rectangle.Width, rectangle.Location.Y + rectangle.Height) };

            foreach(Vector2 point in points)
            {
                if(WithinShadow(point))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
