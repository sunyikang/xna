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

namespace BackgroundSprite
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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        private Vector2 ViperPos;  // Position of foreground sprite on screen
        public int ScrollHeight; // Height of background sprite
        private Viewport viewport;
        Texture2D ShipTexture;
        Texture2D StarTexture;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            StarTexture = Content.Load<Texture2D>("starfield");
            ShipTexture = Content.Load<Texture2D>("ship");
            viewport = graphics.GraphicsDevice.Viewport;

            ViperPos.X = viewport.Width / 2;
            ViperPos.Y = viewport.Height - 100;
            ScrollHeight = StarTexture.Height;
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Turn off blending to draw background
            spriteBatch.Begin(SpriteBlendMode.None);
            DrawBackground(spriteBatch);
            spriteBatch.End();
            // Turn on blending to draw foreground
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            DrawForeground(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void DrawBackground(SpriteBatch Batch)
        {
            // Center the sprite on the center of the screen.
            Vector2 origin = new Vector2(ScrollHeight / 2
                - (viewport.Width / 2), 0);
            Batch.Draw(StarTexture, Vector2.Zero, null,
                Color.White, 0, origin, 1, SpriteEffects.None, 0.9f);
        }
        private void DrawForeground(SpriteBatch Batch)
        {
            // The ship texture is 64x64, so the center is 32x32.
            Vector2 origin = new Vector2(32, 32);
            Batch.Draw(ShipTexture, ViperPos, null,
                Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}
