using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace MyFirst3DGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model myModel;          // Set the 3D model to draw.
        float aspectRatio;      // The aspect ratio determines how to scale 3d to 2d projection.
        Vector3 modelPosition = Vector3.Zero;
        float modelRotation = MathHelper.ToRadians(20.0f);
        Vector3 cameraPosition = new Vector3(0, 50, 5000);
        Vector3 modelVelocity = new Vector3(0, 7, -10);

        //Set the sound effects to use
        SoundEffect soundEngine;
        bool soundEnginePlaying = false;
        bool soundHyperspacePlaying = false;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;   


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
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            // TODO: use this.Content to load your game content here
            myModel = Content.Load<Model>("Models\\p1_wedge");
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            soundEngine = Content.Load<SoundEffect>("Audios\\engine_2");
            soundHyperspaceActivation = Content.Load<SoundEffect>("Audios\\hyperspace_activate");
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
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                this.Exit();

            // TODO: Add your update logic here
            UpdateInput();
            modelPosition += modelVelocity;
            modelVelocity *= 0.95f;

            base.Update(gameTime);
        }

        private void UpdateInput()
        {            
            KeyboardState currentState = Keyboard.GetState();

            // 1. location
            if (currentState.IsKeyDown(Keys.Up))
            {
                Vector3 modelVelocityAdd = Vector3.Zero;
                modelVelocityAdd.Z = -1.0f;
                modelVelocityAdd.Y = 0.50f;
                modelVelocity += modelVelocityAdd;
            }
            else if (currentState.IsKeyDown(Keys.Down))
            {
                Vector3 modelVelocityAdd = Vector3.Zero;
                modelVelocityAdd.Z = 1.0f;
                modelVelocityAdd.Y = -0.50f;
                modelVelocity += modelVelocityAdd;
            }
            else
            { }            

            // 2. sound
            if (currentState.IsKeyDown(Keys.Up) || currentState.IsKeyDown(Keys.Down) && !soundEnginePlaying)
            {
                if (soundEngineInstance == null)
                    soundEngineInstance = soundEngine.Play(0.5f, 0.0f, 0.0f, true);
                else
                    soundEngineInstance.Resume();
                soundEnginePlaying = true;
            }
            else if (!currentState.IsKeyDown(Keys.Up) && !currentState.IsKeyDown(Keys.Down) && soundEnginePlaying)
            {
                soundEngineInstance.Pause();
                soundEnginePlaying = false;
            }
            if (!soundHyperspacePlaying)
            {
                soundHyperspaceActivation.Play(0.5f, 0, 0, false);
                soundHyperspacePlaying = true;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // copy any parent transforms
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateRotationZ(modelRotation) * Matrix.CreateTranslation(modelPosition) * transforms[mesh.ParentBone.Index];
                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10000.0f);
                }
                // draw the mesh, using the effects set above
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
