#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using RoeEngine;
#endregion

namespace WellnessGame
{
    /// <summary>
    /// This screen implements the actual game logic.
    /// </summary>
    /// <remarks>Based on a similar class in the Game State Management sample.</remarks>
    public class GameplayScreen : GameScreen
    {
        #region Field

        // 1. core objects
        Background background;
        Player player;
        SoftwareScore softscore;
        HardwareScore hardscore;
        Computers computers;
        //Clock clock;
        ConflictManager conflict;
        Hardwares hardwaresTwo;
        Softwares softwaresTwo;

        // 2. others
        ContentManager content;
        #endregion        

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            // 3. create instance
            background = new Background(content.Load<Texture2D>(Constant.BackgroundTexturePath));
            player = new Player(content.Load<Texture2D>(Constant.PlayerTexturePath),
                                Constant.PlayerStartPosition,
                                Constant.PlayerSpeed,
                                content.Load<SoundEffect>("Audios/walk"));
            softscore = new SoftwareScore();
            hardscore = new HardwareScore();
            computers = new Computers();
            softwaresTwo = new Softwares(content.Load<Texture2D>(Constant.SoftwareTexturePath));
            hardwaresTwo = new Hardwares(content.Load<Texture2D>(Constant.HardwareTexturePath));
            conflict = new ConflictManager(softwaresTwo, hardwaresTwo, player, softscore, hardscore, computers,
                                            content.Load<SoundEffect>("Audios/get_hardware"),
                                            content.Load<SoundEffect>("Audios/get_software"));

            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }
        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // 4. update
            background.Update(gameTime);
            player.Update(gameTime);
            //softwares.Update(gameTime);
            //hardwares.Update(gameTime);

            //test 
            hardwaresTwo.Update(gameTime);
            softwaresTwo.Update(gameTime);
            conflict.Check();
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);

            // 5. draw
            SpriteBatch batch = ScreenManager.SpriteBatch;
            batch.Begin();            
            background.Draw(batch);
            player.Draw(batch);
            hardwaresTwo.Draw(batch);
            softwaresTwo.Draw(batch);
            batch.End();            

            ScreenManager.GraphicsDevice.RenderState.DepthBufferEnable = true;  //??
        }

        #endregion
    }
}
