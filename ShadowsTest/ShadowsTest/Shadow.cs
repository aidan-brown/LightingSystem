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

        public void FindShadow(Vector2 p1, Vector2 p2, Spotlight light, Platform platform)
        {
            this.p1 = p1;
            this.angleToP1 = AngleFromPointToPoint(p1, light.Position);
            this.p2 = p2;
            this.angleToP2 = AngleFromPointToPoint(p2, light.Position);
            FindOtherPoints(platform, light);

            tri1[0] = new VertexPositionColor(new Vector3(p1.X, p1.Y, 0), Color.Black);
            tri1[1] = new VertexPositionColor(new Vector3(p1f.X, p1f.Y, 0), Color.TransparentBlack);
            tri1[2] = new VertexPositionColor(new Vector3(p2f.X, p2f.Y, 0), Color.TransparentBlack);
            
            tri2[0] = new VertexPositionColor(new Vector3(p2.X, p2.Y, 0), Color.TransparentBlack);
            tri2[1] = new VertexPositionColor(new Vector3(p1f.X, p1f.Y, 0), Color.TransparentBlack);
            tri2[2] = new VertexPositionColor(new Vector3(p2f.X, p2f.Y, 0), Color.TransparentBlack);

            tri3[0] = new VertexPositionColor(new Vector3(p1.X, p1.Y, 0), Color.Black);
            tri3[1] = new VertexPositionColor(new Vector3(p2.X, p2.Y, 0), Color.Black);
            tri3[2] = new VertexPositionColor(new Vector3(p2f.X, p2f.Y, 0), Color.TransparentBlack);


            Console.WriteLine("P1: {0}, P2: {1}, P1f: {2}, P2f: {3}", this.p1, this.p2, this.p1f, this.p2f);
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

        private void FindOtherPoints(Platform platform, Spotlight light)
        {
            if (light != null)
            {
                float phiSlope = ((p1.Y - light.Position.Y) / (p1.X - light.Position.X));
                Vector2 phiPoint = p1;
                float alphaSlope = ((p2.Y - light.Position.Y) / (p2.X - light.Position.X));
                Vector2 alphaPoint = p2;
                float orthoSlope = 1 /((platform.MidPoint.Y - light.Position.Y) / (platform.MidPoint.X - light.Position.X));
                Vector2 orthoPoint = new Vector2((float)(platform.MidPoint.X + length * Math.Cos(1/orthoSlope)), (float)(platform.MidPoint.Y + length * Math.Sin(1/orthoSlope)));
                    
                

                p1f = new Vector2((orthoSlope * orthoPoint.X + phiSlope * phiPoint.X + orthoPoint.Y - phiPoint.Y) / (orthoSlope + phiSlope), phiSlope * (((orthoSlope * orthoPoint.X + phiSlope * phiPoint.X + orthoPoint.Y - phiPoint.Y) / (orthoSlope + phiSlope)) - phiPoint.X) + phiPoint.Y);
                p2f = new Vector2((orthoSlope * orthoPoint.X + alphaSlope * alphaPoint.X + orthoPoint.Y - alphaPoint.Y) / (orthoSlope + alphaSlope), alphaSlope * (((orthoSlope * orthoPoint.X + alphaSlope * alphaPoint.X + orthoPoint.Y - alphaPoint.Y) / (orthoSlope + alphaSlope)) - alphaPoint.X) + alphaPoint.Y);
            }
        }

        public static float AngleFromPointToPoint(Vector2 p1, Vector2 p2)
        {
            return (float)(Math.Atan((p1.Y - p2.Y) / (p1.X - p2.X)));
        }
    }
}
