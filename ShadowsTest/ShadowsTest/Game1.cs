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
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly);
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
            pixel = Content.Load<Texture2D>("pixel");
            lights.Add(new PointLight(new Vector2(0, 0), 100, Content.Load<Texture2D>("point")));
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

            basicEffect.World = Matrix.Identity;
            basicEffect.View = Matrix.Identity;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            bool check = false;

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (Light light in lights)
            {
                light.Draw(spriteBatch);
            }
            foreach (Platform platform in platforms)
            {
                platform.Draw(spriteBatch);
            }
            foreach(Shadow shadow in Platform.GlobalShadows)
            {
                if(shadow.WithinShadow(rect))
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
            if(check)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("platform"), rect, Color.White);
            }
            else
            {
                spriteBatch.Draw(Content.Load<Texture2D>("platform"), rect, Color.Black);
            }
            Platform.DrawShadows(basicEffect, GraphicsDevice, vertexBuffer);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
