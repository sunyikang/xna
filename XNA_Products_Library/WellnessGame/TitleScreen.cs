#region File Description
//-----------------------------------------------------------------------------
// TitleScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RoeEngine;
#endregion

namespace WellnessGame
{
    /// <summary>
    /// The title screen is the first thing displayed when the game starts up.
    /// </summary>
    public class TitleScreen : MenuScreen
    {
        #region Fields


        MenuEntry startMenuEntry;
        MenuEntry twoPlayersMenuEntry;
        MenuEntry exitMenuEntry;
        

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public TitleScreen()
            : base()
        {
            // Create our menu entries.
            startMenuEntry = new MenuEntry();
            twoPlayersMenuEntry = new MenuEntry();
            exitMenuEntry = new MenuEntry();

            // Hook up menu event handlers.
            startMenuEntry.Selected += StartMenuEntrySelected;
            twoPlayersMenuEntry.Selected += TwoPlayerMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(startMenuEntry);
            MenuEntries.Add(twoPlayersMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            // start the title screen music
            AudioManager.PlayMusic("Music_Title");
        }


        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            BackgroundTexture = content.Load<Texture2D>("Title/title_screen");
            startMenuEntry.Texture = content.Load<Texture2D>("Title/start");
            twoPlayersMenuEntry.Texture = content.Load<Texture2D>("Title/2_player");
            exitMenuEntry.Texture = content.Load<Texture2D>("Title/exit");

            base.LoadContent();
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the One Player menu entry is selected.
        /// </summary>
        void StartMenuEntrySelected(object sender, EventArgs e)
        {
            ExitScreen();
            ScreenManager.AddScreen(new GameplayScreen());
        }


        /// <summary>
        /// Event handler for when the Two Players menu entry is selected.
        /// </summary>
        void TwoPlayerMenuEntrySelected(object sender, EventArgs e)
        {
            ExitScreen();
//            ScreenManager.AddScreen(new GameplayScreen(true));
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
