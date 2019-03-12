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
        private VertexPositionColor[] tri1, tri2, tri3;
        private Vector2 p1, p2, p1f, p2f;
        private float length, angleToP1, angleToP2;

        public Shadow(Vector2 p1, Vector2 p2, float l)
        {
            this.p1 = p1;
            this.p2 = p2;
            length = l;
            FindOtherPoints(null, null);

            tri1 = new VertexPositionColor[3];
            tri2 = new VertexPositionColor[3];
            tri3 = new VertexPositionColor[3];
        }

        public void FindShadow(Vector2 p1, Vector2 p2, Light light, Platform platform)
        {
            this.p1 = p1;
            this.angleToP1 = AngleFromPointToPoint(p1, light.GlobalPosition);
            this.p2 = p2;
            this.angleToP2 = AngleFromPointToPoint(p2, light.GlobalPosition);
            FindOtherPoints(platform, light);

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

        private void FindOtherPoints(Platform platform, Light light)
        {
            if (light != null)
            {
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
                        orthoPoint = new Vector2((float)(platform.MidPoint.X + length * Math.Cos(Math.Atan(1 / orthoSlope))), (float)(platform.MidPoint.Y + length * Math.Sin(Math.Atan(1 / orthoSlope))));
                    }
                    else
                    {
                        orthoPoint = new Vector2((float)(platform.MidPoint.X - length * Math.Cos(Math.Atan(1 / orthoSlope))), (float)(platform.MidPoint.Y - length * Math.Sin(Math.Atan(1 / orthoSlope))));
                    }

                    p1f = new Vector2((orthoSlope * orthoPoint.X + phiSlope * phiPoint.X + orthoPoint.Y - phiPoint.Y) / (orthoSlope + phiSlope), phiSlope * (((orthoSlope * orthoPoint.X + phiSlope * phiPoint.X + orthoPoint.Y - phiPoint.Y) / (orthoSlope + phiSlope)) - phiPoint.X) + phiPoint.Y);
                    p2f = new Vector2((orthoSlope * orthoPoint.X + alphaSlope * alphaPoint.X + orthoPoint.Y - alphaPoint.Y) / (orthoSlope + alphaSlope), alphaSlope * (((orthoSlope * orthoPoint.X + alphaSlope * alphaPoint.X + orthoPoint.Y - alphaPoint.Y) / (orthoSlope + alphaSlope)) - alphaPoint.X) + alphaPoint.Y);
                }
                else
                {
                    if(platform.MidPoint.X == light.GlobalPosition.X)
                    {
                        if(platform.MidPoint.Y > light.GlobalPosition.Y)
                        {
                            p1 = platform.PointNW;
                            p2 = platform.PointNE;
                            phiSlope = ((p1.Y - light.GlobalPosition.Y) / (p1.X - light.GlobalPosition.X));
                            alphaSlope = ((p2.Y - light.GlobalPosition.Y) / (p2.X - light.GlobalPosition.X));
                            p1f = new Vector2((float)(p1.X - length * Math.Cos(AngleFromPointToPoint(p1, light.GlobalPosition))), (float)(p1.Y + length * Math.Sin(AngleFromPointToPoint(p1, light.GlobalPosition))));
                            p2f = new Vector2((float)(p2.X + length * Math.Cos(AngleFromPointToPoint(p2, light.GlobalPosition))), (float)(p2.Y + length * Math.Sin(AngleFromPointToPoint(p2, light.GlobalPosition))));
                        }
                        else
                        {
                            p1 = platform.PointSW;
                            p2 = platform.PointSE;
                            phiSlope = ((p1.Y - light.GlobalPosition.Y) / (p1.X - light.GlobalPosition.X));
                            alphaSlope = ((p2.Y - light.GlobalPosition.Y) / (p2.X - light.GlobalPosition.X));
                            p1f = new Vector2((float)(p1.X - length * Math.Cos(AngleFromPointToPoint(p1, light.GlobalPosition))), (float)(p1.Y - length * Math.Sin(AngleFromPointToPoint(p1, light.GlobalPosition))));
                            p2f = new Vector2((float)(p2.X + length * Math.Cos(AngleFromPointToPoint(p2, light.GlobalPosition))), (float)(p2.Y - length * Math.Sin(AngleFromPointToPoint(p2, light.GlobalPosition))));
                        }
                    }
                    else
                    {
                        if (platform.MidPoint.X > light.GlobalPosition.X)
                        {
                            p1 = platform.PointNW;
                            p2 = platform.PointSW;
                            phiSlope = ((p1.Y - light.GlobalPosition.Y) / (p1.X - light.GlobalPosition.X));
                            alphaSlope = ((p2.Y - light.GlobalPosition.Y) / (p2.X - light.GlobalPosition.X));
                            p1f = new Vector2((float)(p1.X + length * Math.Cos(AngleFromPointToPoint(p1, light.GlobalPosition))), (float)(p1.Y - length * Math.Sin(AngleFromPointToPoint(p1, light.GlobalPosition))));
                            p2f = new Vector2((float)(p2.X + length * Math.Cos(AngleFromPointToPoint(p2, light.GlobalPosition))), (float)(p2.Y + length * Math.Sin(AngleFromPointToPoint(p2, light.GlobalPosition))));
                        }
                        else
                        {
                            p1 = platform.PointNE;
                            p2 = platform.PointSE;
                            phiSlope = ((p1.Y - light.GlobalPosition.Y) / (p1.X - light.GlobalPosition.X));
                            alphaSlope = ((p2.Y - light.GlobalPosition.Y) / (p2.X - light.GlobalPosition.X));
                            p1f = new Vector2((float)(p1.X - length * Math.Cos(AngleFromPointToPoint(p1, light.GlobalPosition))), (float)(p1.Y - length * Math.Sin(AngleFromPointToPoint(p1, light.GlobalPosition))));
                            p2f = new Vector2((float)(p2.X - length * Math.Cos(AngleFromPointToPoint(p2, light.GlobalPosition))), (float)(p2.Y + length * Math.Sin(AngleFromPointToPoint(p2, light.GlobalPosition))));
                        }
                    }
                }
            }
        }

        public static float AngleFromPointToPoint(Vector2 p1, Vector2 p2)
        {
            if (p1.X < p2.X)
            {
                return (float)(Math.Atan((p1.Y - p2.Y) / (p1.X - p2.X)));
            }
            return (float)(Math.Atan((p1.Y - p2.Y) / (p1.X - p2.X)) + Math.PI);
        }
    }
}
