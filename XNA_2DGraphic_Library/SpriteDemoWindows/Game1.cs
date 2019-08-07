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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

public class Game1 : Microsoft.Xna.Framework.Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    public Game1()
    {
        graphics = new GraphicsDeviceManager( this );
        Content.RootDirectory = "Content";
#if ZUNE
            // Frame rate is 30 fps by default for Zune.
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
#endif
    }


    protected override void Initialize()
    {
        base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    private Texture2D SpriteTexture;
    private Rectangle TitleSafe;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteTexture = Content.Load<Texture2D>("Sprite");
        TitleSafe = GetTitleSafeArea(.8f);
    }


    protected override void Update( GameTime gameTime )
    {
        // Allows the default game to exit on Xbox 360 and Windows.
        if (GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed)
            this.Exit();

        base.Update( gameTime );
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

    protected override void Draw( GameTime gameTime )
    {
        graphics.GraphicsDevice.Clear( Color.CornflowerBlue );
        spriteBatch.Begin();
        Vector2 pos = new Vector2( TitleSafe.Left, TitleSafe.Top );
        spriteBatch.Draw(SpriteTexture, pos, Color.White);
        spriteBatch.End();
        base.Draw( gameTime );
    }
}
