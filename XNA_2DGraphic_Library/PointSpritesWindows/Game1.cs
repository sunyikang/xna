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

namespace PointSprites
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

        Effect pointSpritesEffect;
        VertexPositionColor[] spriteArray;
        VertexDeclaration vertexPosColDecl;
        Random rand;

        Matrix projectionMatrix;
        Matrix viewMatrix;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pointSpritesEffect = Content.Load<Effect>("pointsprites");
            pointSpritesEffect.Parameters["SpriteTexture"].SetValue(
                Content.Load<Texture2D>("fire"));
            spriteArray = new VertexPositionColor[200];
            vertexPosColDecl = new VertexDeclaration(graphics.GraphicsDevice,
                VertexPositionColor.VertexElements); 
            rand = new Random();
            for (int i = 0; i < spriteArray.Length; i++)
            {
                spriteArray[i].Position = new Vector3(rand.Next(100) / 10f,
                   rand.Next(100) / 10f, rand.Next(100) / 10f);
                spriteArray[i].Color = Color.WhiteSmoke;
            }
            viewMatrix = Matrix.CreateLookAt(Vector3.One * 25, Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                4.0f / 3.0f, 1.0f, 10000f);
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
            spriteArray[rand.Next(0, 200)].Position = new Vector3(rand.Next(100) / 10f,
                   rand.Next(100) / 10f, rand.Next(100) / 10f);
            spriteArray[rand.Next(0, 200)].Position = new Vector3(rand.Next(100) / 10f,
                  rand.Next(100) / 10f, rand.Next(100) / 10f);
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            graphics.GraphicsDevice.RenderState.PointSpriteEnable = true;
            graphics.GraphicsDevice.RenderState.PointSize = 64.0f;
            graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.One;
            graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;

            graphics.GraphicsDevice.VertexDeclaration
                = vertexPosColDecl;
            Matrix WVPMatrix = Matrix.Identity * viewMatrix * projectionMatrix;
            pointSpritesEffect.Parameters["WVPMatrix"].SetValue(WVPMatrix);

            pointSpritesEffect.Begin();
            foreach (EffectPass pass in pointSpritesEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.PointList,
                    spriteArray, 0, spriteArray.Length);
                pass.End();
            }
            pointSpritesEffect.End();

            graphics.GraphicsDevice.RenderState.PointSpriteEnable = false;
            graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphics.GraphicsDevice.RenderState.DestinationBlend =
                Blend.InverseSourceAlpha;

            base.Draw(gameTime);
        }
    }
}
