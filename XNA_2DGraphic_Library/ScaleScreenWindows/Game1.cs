#region File Description
//-----------------------------------------------------------------------------
// Game1.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ScaleScreen
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
#if ZUNE
            // Frame rate is 30 fps by default for Zune.
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();

            graphics.GraphicsDevice.DeviceReset += new EventHandler(GraphicsDevice_DeviceReset);

            base.Initialize();
        }

        /// <summary>
        /// The event handler for the DeviceReset event. We only need to change the matrix
        /// when the screen itself changes size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            // Default resolution is 800x600; scale sprites up or down based on
            // current viewport
            float screenscale = (float)graphics.GraphicsDevice.Viewport.Width / 800f;
            // Create the scale transform for Draw. 
            // Do not scale the sprite depth (Z=1).
            SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        Texture2D square;
        Vector2[] spritepos;
        Matrix SpriteScale;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);            

            // Load a sprite to draw at each corner
            square = Content.Load<Texture2D>( "sprite" );

            // Create the corners where the sprites will be drawn
            spritepos = new Vector2[4];
            spritepos[0] = new Vector2( 42, 42 );
            spritepos[1] = new Vector2( graphics.GraphicsDevice.Viewport.Width - 42, 42 );
            spritepos[2] = new Vector2( 42, graphics.GraphicsDevice.Viewport.Height - 42 );
            spritepos[3] = new Vector2( spritepos[1].X, spritepos[2].Y );

            // Default resolution is 800x600; scale sprites up or down based on
            // current viewport
            float screenscale = (float) graphics.GraphicsDevice.Viewport.Width / 800f;
            // Create the scale transform for Draw. 
            // Do not scale the sprite depth (Z=1).
            SpriteScale = Matrix.CreateScale( screenscale, screenscale, 1 );
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();

            // Change the resolution dynamically based on input
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed
                || keyState.IsKeyDown(Keys.Left))
            {
                graphics.PreferredBackBufferHeight = 300;
                graphics.PreferredBackBufferWidth = 400;
                graphics.ApplyChanges();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed
                || keyState.IsKeyDown(Keys.Right))
            {
                graphics.PreferredBackBufferHeight = 600;
                graphics.PreferredBackBufferWidth = 800;
                graphics.ApplyChanges();
            }
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Set draw parameters
            Vector2 origin = new Vector2(32);
            float rotation = 0;
            float depth = 0;
            float scale = 1;


            // Initialize the batch with the scaling matrix
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,
                SaveStateMode.None, SpriteScale);
            // Draw a sprite at each corner
            for (int i = 0; i < spritepos.Length; i++)
            {
                spriteBatch.Draw(square, spritepos[i], null, Color.White, rotation, 
                    origin, scale, SpriteEffects.None, depth);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
