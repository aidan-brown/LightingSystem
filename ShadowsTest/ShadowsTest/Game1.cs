using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShadowsTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D pixel;
        List<Platform> platforms;
        List<Light> lights;
        VertexBuffer vertexBuffer;
        Rectangle rect;

        BasicEffect basicEffect;
        Matrix world = Matrix.CreateTranslation(0, 0, 0);
        Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        Matrix projection;
        Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

        public static Texture2D lightMask;
        public static Effect effect1;
        RenderTarget2D lightsTarget;
        RenderTarget2D mainTarget;
        RenderTarget2D lightsAndMainTarget;
        RenderTarget2D background;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            platforms = new List<Platform>();
            lights = new List<Light>();
            rect = new Rectangle(250, 100, 5, 5);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            basicEffect = new BasicEffect(GraphicsDevice);
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            lightMask = Content.Load<Texture2D>("lightMask1");
            effect1 = Content.Load<Effect>("lighteffect");
            var pp = GraphicsDevice.PresentationParameters;
            lightsTarget = new RenderTarget2D(
            GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(
            GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            lightsAndMainTarget = new RenderTarget2D(
            GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            background = new RenderTarget2D(
            GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

            pixel = Content.Load<Texture2D>("pixel");
            lights.Add(new PointLight(new Vector2(0, 0), 500, Content.Load<Texture2D>("lightMask1")));
            //lights.Add(new PointLight(new Vector2(200, 0), 100, Content.Load<Texture2D>("point")));
            for (int i = 0; i < 5; i++)
            {
                //platforms.Add(new Platform(new Rectangle(0 + i * 50, 400, 50, 50), Content.Load<Texture2D>("platform"), GraphicsDevice));
                platforms.Add(new Platform(new Rectangle(100 + i * 60, 300, 25, 50), Content.Load<Texture2D>("platform"), GraphicsDevice));
            }

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (Light light in lights)
            {
                light.Update();
            }
            
            foreach(Platform platform in platforms)
            {
                platform.Update(lights);
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                rect.Y--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                rect.Y++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rect.X--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rect.X++;
            }

            Platform.UpdateShadows();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            bool check = false;

            // TODO: Add your drawing code here
            GraphicsDevice.SetRenderTarget(lightsTarget);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            foreach (Light light in lights)
            {
                light.Draw(spriteBatch);
            }
            spriteBatch.End();



            GraphicsDevice.SetRenderTarget(mainTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (Platform platform in platforms)
            {
                platform.Draw(spriteBatch);
            }
            foreach (Shadow shadow in Platform.GlobalShadows)
            {
                if (shadow.WithinShadow(rect))
                {
                    check = true;
                    break;
                }
            }
            if (!check)
            {
                foreach (Light light in lights)
                {
                    if (!light.IsWithinLight(rect))
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                        break;
                    }
                }
            }
            if (check)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("platform"), rect, Color.White);
            }
            else
            {
                spriteBatch.Draw(Content.Load<Texture2D>("platform"), rect, Color.Black);
            }
            
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(lightsAndMainTarget);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect1.Parameters["lightMask"].SetValue(lightsTarget);
            effect1.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin();
            spriteBatch.Draw(lightsAndMainTarget, Vector2.Zero, Color.White);
            spriteBatch.End();


            basicEffect.World = Matrix.Identity;
            basicEffect.View = Matrix.Identity;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            Shadow.DrawShadows(basicEffect, GraphicsDevice, vertexBuffer);


            base.Draw(gameTime);
        }
    }
}
