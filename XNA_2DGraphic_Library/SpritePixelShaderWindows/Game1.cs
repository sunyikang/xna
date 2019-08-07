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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpritePixelShader
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
        public static RenderTarget2D CloneRenderTarget(GraphicsDevice device, int numberLevels)
        {
            return new RenderTarget2D(device,
                device.PresentationParameters.BackBufferWidth,
                device.PresentationParameters.BackBufferHeight,
                numberLevels,
                device.DisplayMode.Format,
                device.PresentationParameters.MultiSampleType,
                device.PresentationParameters.MultiSampleQuality
            );
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        Texture2D grid;
        EffectParameter waveParam, distortionParam, centerCoordParam;

        RenderTarget2D ShaderRenderTarget;
        Texture2D ShaderTexture;
        Effect ripple;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            grid = Content.Load<Texture2D>("grid");
            ripple = Content.Load<Effect>("ripple");
            waveParam = ripple.Parameters["wave"];
            distortionParam = ripple.Parameters["distortion"];
            centerCoordParam = ripple.Parameters["centerCoord"];

            ShaderRenderTarget = CloneRenderTarget(GraphicsDevice, 1);
            ShaderTexture = new Texture2D(GraphicsDevice, ShaderRenderTarget.Width, ShaderRenderTarget.Height, 1,
                TextureUsage.None, ShaderRenderTarget.Format);

            Reset();
        }
        Vector2 centerCoord = new Vector2(0.5f);
        float distortion = 1.0f;
        float divisor = 0.75f;
        float wave = MathHelper.Pi;
        private void Reset()
        {
            centerCoord = new Vector2(0.5f);
            distortion = 1.0f;
            divisor = 0.75f;
            wave = MathHelper.Pi / divisor;
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

            GamePadState state = GamePad.GetState(PlayerIndex.One);

            // Reset.
            if (state.Buttons.Start == ButtonState.Pressed)
            {
                Reset();
            }

            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the center.
            centerCoord.X = MathHelper.Clamp(centerCoord.X +
                (state.ThumbSticks.Right.X * seconds * 0.5f),
                0, 1);
            centerCoord.Y = MathHelper.Clamp(centerCoord.Y -
                (state.ThumbSticks.Right.Y * seconds * 0.5f),
                0, 1);

            // Change the distortion.
            distortion += state.ThumbSticks.Left.X * seconds * 0.5f;

            // Change the period.
            divisor += state.ThumbSticks.Left.Y * seconds * 0.5f;

            //wave = MathHelper.Pi / divisor;
            wave = MathHelper.Pi / divisor;
            waveParam.SetValue(wave);
            distortionParam.SetValue(distortion);
            centerCoordParam.SetValue(centerCoord);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Change to our offscreen render target.
            RenderTarget2D temp = (RenderTarget2D)GraphicsDevice.GetRenderTarget(0);
            GraphicsDevice.SetRenderTarget(0, ShaderRenderTarget);

            GraphicsDevice.Clear(Color.Black);
            // Render a simple scene.
            spriteBatch.Begin();
            TileSprite(spriteBatch, grid);
            spriteBatch.End();

            // Change back to the back buffer, and get our scene
            // as a texture.
            //GraphicsDevice.ResolveRenderTarget(0);
            GraphicsDevice.SetRenderTarget(0, temp);
            ShaderTexture = ShaderRenderTarget.GetTexture();

            // Use Immediate mode and our effect to draw the scene
            // again, using our pixel shader.
            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            ripple.Begin();
            ripple.CurrentTechnique.Passes[0].Begin();
            spriteBatch.Draw(ShaderTexture, Vector2.Zero, Color.White);
            spriteBatch.End();
            ripple.CurrentTechnique.Passes[0].End();
            ripple.End();
            base.Draw(gameTime);
        }
        private void TileSprite(SpriteBatch batch, Texture2D texture)
        {
            Vector2 pos = Vector2.Zero;
            for (int i = 0; i < GraphicsDevice.Viewport.Height; i += texture.Height)
            {
                for (int j = 0; j < GraphicsDevice.Viewport.Width; j += texture.Width)
                {
                    pos.X = j; pos.Y = i;
                    batch.Draw(texture, pos, Color.White);
                }
            }
        }
    }
}
