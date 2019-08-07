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
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ScaleSprite
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
        protected Texture2D SpriteTexture;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteTexture = Content.Load<Texture2D>("ship");
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
        protected float scale = 1f;
        protected override void Update(GameTime gameTime)
        {
            // Allows the default game to exit on Xbox 360 and Windows.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your game logic here.
            scale += elapsed;
            scale = scale % 6;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawForeground(spriteBatch);
            base.Draw(gameTime);
        }
        protected virtual void DrawForeground( SpriteBatch batch )
        {
            Rectangle safeArea = GetTitleSafeArea( .8f );
            Vector2 position = new Vector2( safeArea.X, safeArea.Y );
            batch.Begin();
            batch.Draw( SpriteTexture, position, null,
                Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f );
            batch.End();
        }

        protected Rectangle GetTitleSafeArea( float percent )
        {
            Rectangle retval = new Rectangle( graphics.GraphicsDevice.Viewport.X,
                graphics.GraphicsDevice.Viewport.Y,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height );
    #if XBOX
            // Find Title Safe area of Xbox 360.
            float border = (1 - percent) / 2;
            retval.X = (int)(border * retval.Width);
            retval.Y = (int)(border * retval.Height);
            retval.Width = (int)(percent * retval.Width);
            retval.Height = (int)(percent * retval.Height);
            return retval;

    #else
            return retval;
    #endif

        }
    }

    // See program.cs for instructions to instantiate this class
    public class RectangleDemo : Game1
    {
        protected Rectangle destrect;
        protected override void Update(GameTime gameTime)
        {
            destrect = new Rectangle();
            Rectangle safeArea = GetTitleSafeArea(.8f);
            destrect.X = safeArea.X;
            destrect.Y = safeArea.Y;
            destrect.Width = (int)scale * 100;
            destrect.Height = (int)scale * 80;
            base.Update(gameTime);
        }
        protected override void DrawForeground(SpriteBatch batch)
        {
            batch.Begin();
            batch.Draw(SpriteTexture, destrect, Color.White);
            batch.End();
        }
    }

    // See program.cs for instructions to instantiate this class
    public class NonUniformScalarDemo : Game1
    {
        protected Vector2 nonuniformscale = Vector2.One;
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float basescale = nonuniformscale.Y;
            basescale += (float)gameTime.ElapsedGameTime.TotalSeconds;
            basescale = basescale % 6;
            nonuniformscale.Y = basescale;
            nonuniformscale.X = basescale * .8f;
        }
        protected override void DrawForeground(SpriteBatch batch)
        {
            Rectangle safeArea = GetTitleSafeArea(.8f);
            Vector2 position = new Vector2(safeArea.X, safeArea.Y);
            batch.Begin();
            batch.Draw(SpriteTexture, position, null, Color.White, 0, Vector2.Zero,
                nonuniformscale, SpriteEffects.None, 0);
            batch.End();
        }
    }
}
