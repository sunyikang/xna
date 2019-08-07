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

namespace SkySphere
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
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        SampleArcBallCamera myCamera;
        protected override void Initialize()
        {
            myCamera = new SampleArcBallCamera(SampleArcBallCameraMode.Free);
            myCamera.Target = new Vector3(0, 0, 0);
            myCamera.Distance = -50f;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        Vector3 ModelPosition;
        float ModelRotation = 0.0f;
        Model Model;
        Model SkySphere;
        Effect SkySphereEffect;
        Matrix projectionMatrix;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Model = Content.Load<Model>("redtorus");
            ModelPosition = Vector3.Zero;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4.0f / 3.0f, 1.0f, 10000f);
            // Load the effect, the texture it uses, and 
            // the model used for drawing it
            SkySphereEffect = Content.Load<Effect>("SkySphere");
            TextureCube SkyboxTexture = Content.Load<TextureCube>("uffizi_cross");
            SkySphere = Content.Load<Model>("SphereHighPoly");

            // Set the parameters of the effect
            //SkySphereEffect.Parameters["ViewMatrix"].SetValue(myCamera2.ViewMatrix);
            SkySphereEffect.Parameters["ViewMatrix"].SetValue(myCamera.ViewMatrix);
            SkySphereEffect.Parameters["ProjectionMatrix"].SetValue(projectionMatrix);
            SkySphereEffect.Parameters["SkyboxTexture"].SetValue(SkyboxTexture);
            // Set the Skysphere Effect to each part of the Skysphere model
            foreach (ModelMesh mesh in SkySphere.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = SkySphereEffect;
                }
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
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            GamePadState PlayerOne = GamePad.GetState(PlayerIndex.One);
            if (PlayerOne.Buttons.A == ButtonState.Pressed)
            {
                myCamera.Target = new Vector3(0, 0, 0);
                myCamera.Distance = -50f;               
            }
            
            myCamera.OrbitUp(PlayerOne.ThumbSticks.Right.Y / 4);
            myCamera.OrbitRight(PlayerOne.ThumbSticks.Right.X / 4);
          
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in Model.Meshes)
            {
                //This is where the mesh orientation is set, as well as our camera and projection
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY(ModelRotation)
                        * Matrix.CreateTranslation(ModelPosition);
                    //effect.View = myCamera.View;
                    effect.View = myCamera.ViewMatrix;
                    effect.Projection = projectionMatrix;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }

            // Set the View and Projection matrix for the effect
            SkySphereEffect.Parameters["ViewMatrix"].SetValue(myCamera.ViewMatrix);
            //SkySphereEffect.Parameters["ViewMatrix"].SetValue(myCamera.View);
            SkySphereEffect.Parameters["ProjectionMatrix"].SetValue(projectionMatrix);
            // Draw the sphere model that the effect projects onto
            foreach (ModelMesh mesh in SkySphere.Meshes)
            {
                mesh.Draw();
            }

            // Undo the renderstate settings from the shader
            graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            base.Draw(gameTime);
        }
    }
}
