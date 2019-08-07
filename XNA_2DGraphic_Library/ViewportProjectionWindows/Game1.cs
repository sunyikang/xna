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

namespace ViewportProjection
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model Ring;
        float RingRotation;
        Vector3 RingPosition;

        SampleArcBallCamera Camera1;
        Matrix projectionMatrix;

        AnimatedTexture explosion;
        Vector2 explosionpos;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Camera1 = new SampleArcBallCamera(SampleArcBallCameraMode.Free);
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


            Camera1.Target = new Vector3(0, 0, 0);
            Camera1.Distance = 25f;
            Camera1.OrbitRight(MathHelper.PiOver4);
            Camera1.OrbitUp(MathHelper.PiOver4);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4.0f / 3.0f, 1.0f, 10000f);
            RingPosition = Vector3.Zero;
            RingRotation = 0.0f;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Ring = Content.Load<Model>( "redtorus" );
            explosion.Load(Content, "explosion", 10, 3 );
            explosion.Pause();
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            //GraphicsDevice.RenderState.CullMode = CullMode.None;
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

            // Move the camera using thumbsticks
            MoveCamera(PlayerOne);

            // Start or stop the animated sprite using buttons
            if (PlayerOne.Buttons.A == ButtonState.Pressed)
                explosion.Play();
            if (PlayerOne.Buttons.B == ButtonState.Pressed)
                explosion.Stop();

            // Update the animated sprite
            explosion.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);

            // Create a total bounding sphere for the mesh
            BoundingSphere totalbounds = new BoundingSphere();
            foreach (ModelMesh mesh in Ring.Meshes)
            {
                totalbounds = BoundingSphere.CreateMerged(totalbounds, mesh.BoundingSphere);
            }

            // Project the center of the 3D object to the screen, and center the
            // sprite there
            Vector3 center = GraphicsDevice.Viewport.Project(totalbounds.Center,
                projectionMatrix, Camera1.ViewMatrix, Matrix.Identity);
            explosionpos.X = center.X;
            explosionpos.Y = center.Y;

            // Create a bounding box from the bounding sphere, and find the corner
            // that is farthest away from the center using Project
            BoundingBox extents = BoundingBox.CreateFromSphere(totalbounds);
            float maxdistance = 0;
            float distance;
            Vector3 screencorner;
            foreach (Vector3 corner in extents.GetCorners())
            {
                screencorner = GraphicsDevice.Viewport.Project(corner,
                projectionMatrix, Camera1.ViewMatrix, Matrix.Identity);
                distance = Vector3.Distance(screencorner, center);
                if (distance > maxdistance)
                    maxdistance = distance;
            }

            // Scale the sprite using the two points (the sprite is 
            // 75 pixels square)
            explosion.Scale = maxdistance / 75; 
            
            base.Update(gameTime);
        }

        private void MoveCamera(GamePadState Player)
        {
            Camera1.OrbitUp(Player.ThumbSticks.Right.Y / 4);
            Camera1.OrbitRight(Player.ThumbSticks.Right.X / 4);
            Camera1.Distance -= Player.ThumbSticks.Left.Y;
            Vector3 newTarget = Camera1.Target;
            newTarget.X += Player.ThumbSticks.Left.X;
            Camera1.Target = newTarget;
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in Ring.Meshes)
            {
                //This is where the mesh orientation is set, as well as our camera and projection
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity * Matrix.CreateRotationY( RingRotation )
                        * Matrix.CreateTranslation( RingPosition );
                    effect.View = Camera1.ViewMatrix;
                    effect.Projection = projectionMatrix;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }

            // Draw the sprite over the 3D object
            spriteBatch.Begin( SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState );
            explosion.DrawFrame( spriteBatch, explosionpos );
            spriteBatch.End();

            base.Draw( gameTime );
        }
    }
}
