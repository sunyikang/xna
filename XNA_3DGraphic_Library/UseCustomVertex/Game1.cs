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
using System.Runtime.InteropServices;

namespace UseCustomVertex
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionColoredNormal
    {
        Vector3 vertexPosition;
        Color vertexColor;
        Vector3 vertexNormal;

        public static int SizeInBytes { get { return 28; } }

        //Declares the elements of the custom vertex. Each vertex stores information on the current 
        //position, color, and normal.
        public static readonly VertexElement[] VertexElements = new VertexElement[]
        {
            new VertexElement(0, 0, VertexElementFormat.Vector3,
            VertexElementMethod.Default, VertexElementUsage.Position, 0),
            new VertexElement(0, sizeof(float) * 3, VertexElementFormat.Color,
            VertexElementMethod.Default, VertexElementUsage.Color, 0),
            new VertexElement(0, sizeof(float) * 3 + 4, VertexElementFormat.Vector3,
            VertexElementMethod.Default, VertexElementUsage.Normal, 0),
        };

        //The constructor for the custom vertex. This allows similar initialization of custom vertex arrays
        //as compared to arrays of a standard vertex type, such as VertexPositionColor.
        public VertexPositionColoredNormal(Vector3 pos, Color color, Vector3 normal)
        {
            vertexPosition = pos;
            vertexColor = color;
            vertexNormal = normal;
        }

        //Public methods for accessing the components of the custom vertex.
        public Vector3 Position { get { return vertexPosition; } set { vertexPosition = value; } }
        public Color Color { get { return vertexColor; } set { vertexColor = value; } }
        public Vector3 Normal { get { return vertexNormal; } set { vertexNormal = value; } }
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        BasicEffect basicEffect;
        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;

        VertexPositionColoredNormal[] triangleStripVertices;
        VertexDeclaration basicEffectVertexDeclaration;
        short[] triangleStripIndices;

        GraphicsDeviceManager graphics;

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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            InitializeTransform();
            InitializeEffect();
            InitializePrimitive();
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
        /// Initializes the transforms used for the triangle strip.
        /// </summary>
        private void InitializeTransform()
        {
            //Centers the triangle strip in the game's viewport.
            worldMatrix = Matrix.CreateTranslation(-1.5f, -0.5f, 0.0f);

            viewMatrix = Matrix.CreateLookAt(
                new Vector3(0.0f, 0.0f, 5.0f),
                Vector3.Zero,
                Vector3.Up
                );

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),
                (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f, 100.0f
                );
        }

        /// <summary>
        /// Initializes the basic effect (parameter setting and technique selection)
        /// used for the 3D model.
        /// </summary>
        private void InitializeEffect()
        {
            basicEffectVertexDeclaration = new VertexDeclaration(
                graphics.GraphicsDevice, VertexPositionColoredNormal.VertexElements);

            //Enables some basic effect characteristics, such as vertex coloring an ddefault lighting.
            basicEffect = new BasicEffect(graphics.GraphicsDevice, null);
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.EnableDefaultLighting();

            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;
        }

        /// <summary>
        /// Initializes the vertices and indices of the triangle strip.
        /// </summary>
        private void InitializePrimitive()
        {
            triangleStripVertices = new VertexPositionColoredNormal[8];

            //Initialize the custom vertex values for the triangle strip.
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    triangleStripVertices[(x * 2) + y] = new VertexPositionColoredNormal(
                        new Vector3(x, y, 0.0f),
                        Color.Red,
                        new Vector3(0.0f, 0.0f, 1)
                        );
                }
            }

            //Initialize the indices for the triangle strip.
            triangleStripIndices = new short[8];
            for (int i = 0; i < 8; i++)
            {
                triangleStripIndices[i] = (short)i;
            }
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
            graphics.GraphicsDevice.Clear(Color.SteelBlue);

            // This code would go between a device BeginScene-EndScene block.
            graphics.GraphicsDevice.VertexDeclaration = basicEffectVertexDeclaration;

            basicEffect.Begin();
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColoredNormal>(
                    PrimitiveType.TriangleStrip,
                    triangleStripVertices,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    8,  // number of vertices to draw
                    triangleStripIndices,
                    0,  // first index element to read
                    6   // number of primitives to draw
                );

                pass.End();
            }
            basicEffect.End();

            base.Draw(gameTime);
        }
    }
}
