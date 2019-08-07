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

namespace RotateSpriteGroup
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
        private Vector2[] myVectors;
        private Vector2[] drawVectors;
        protected override void Initialize()
        {
            myVectors = new Vector2[9];
            drawVectors = new Vector2[9];

            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        private Texture2D SpriteTexture;
        private Vector2 origin;
        private Vector2 screenpos;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteTexture = Content.Load<Texture2D>("ship");
            origin.X = SpriteTexture.Width / 2;
            origin.Y = SpriteTexture.Height / 2;
            Viewport viewport = graphics.GraphicsDevice.Viewport;
            screenpos.X = viewport.Width / 2;
            screenpos.Y = viewport.Height / 2;
            // Create unrotated texture locations.
            float texsize = SpriteTexture.Height;
            float[] points = { -texsize, 0, texsize };
            Vector2 offset = Vector2.Zero;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    offset.X = points[i];
                    offset.Y = points[j];
                    myVectors[(i * 3) + j] =
                        Vector2.Add(screenpos, offset);
                }
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
        private float RotationAngle = 0f;
        private bool batchAutoRotate = false;
        private Matrix rotationMatrix = Matrix.Identity;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                batchAutoRotate = false;
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                batchAutoRotate = true;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            RotationAngle += elapsed;
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;

            // Copy and rotate the sprite positions.
            drawVectors = (Vector2[])myVectors.Clone();

            if (!batchAutoRotate)
                RotatePoints(ref screenpos, RotationAngle, ref drawVectors);
            else
                UpdateMatrix(ref screenpos, RotationAngle);

            base.Update(gameTime);
        }
        private void UpdateMatrix(ref Vector2 origin, float radians)
        {
            // Translate sprites to center around screen (0,0), rotate them, and
            // translate them back to their original positions
            Vector3 matrixorigin = new Vector3(origin, 0);
            rotationMatrix = Matrix.CreateTranslation(-matrixorigin) *
                Matrix.CreateRotationZ(radians) *
                Matrix.CreateTranslation(matrixorigin);
        }
        private static void RotatePoints(ref Vector2 origin, float radians, ref Vector2[] Vectors)
        {
            Matrix myRotationMatrix = Matrix.CreateRotationZ(radians);

            for (int i = 0; i < 9; i++)
            {
                // Rotate relative to origin.
                Vector2 rotatedVector = Vector2.Transform(Vectors[i] - origin, myRotationMatrix);

                // Add origin to get final location.
                Vectors[i] = rotatedVector + origin;
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            if (!batchAutoRotate)
            {
                GraphicsDevice.Clear(Color.Black);
                DrawPoints();
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                DrawMatrix();
            }

            base.Draw(gameTime);
        }

        private void DrawMatrix()
        {
            // Draw using a rotation matrix with SpriteBatch
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,
                SaveStateMode.None, rotationMatrix);
            for (int j = 0; j < myVectors.Length; j++)
                spriteBatch.Draw(SpriteTexture, myVectors[j], null, Color.White, 0,
                    origin, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
        private void DrawPoints()
        {
            // Draw using manually rotated vectors
            spriteBatch.Begin();
            for (int i = 0; i < drawVectors.Length; i++)
                spriteBatch.Draw(SpriteTexture, drawVectors[i], null, Color.White, RotationAngle,
                    origin, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

    }
}
