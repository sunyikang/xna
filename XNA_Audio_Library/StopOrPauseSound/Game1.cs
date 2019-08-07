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

namespace StopOrPauseSound
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        // Audio objects
        AudioEngine engine;
        SoundBank soundBank;
        WaveBank waveBank;
        Cue cue;

        GamePadState oldState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Initialize audio objects.
            engine = new AudioEngine("Content\\Audio\\StopOrPauseSound.xgs");
            soundBank = new SoundBank(engine, "Content\\Audio\\Sound Bank.xsb");
            waveBank = new WaveBank(engine, "Content\\Audio\\Wave Bank.xwb");

            // Get the cue and play it.
            cue = soundBank.GetCue("music");
            cue.Play();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
        }

        protected void UpdateInput()
        {
            KeyboardState keystate = Keyboard.GetState();

            //if (keystate.IsKeyDown(Keys.Up))
            //{
            //    // If stopped, create a new cue.
            //    cue = soundBank.GetCue(cue.Name);
            //    cue.Play();
            //}
            if (keystate.IsKeyDown(Keys.Left))
            {
                if (cue.IsPaused)
                {
                    cue.Resume();
                }
                else
                {
                    cue.Pause();
                }
            }                       
            if (keystate.IsKeyDown(Keys.Right))
            {
                if (cue.IsPlaying)
                {
                    cue.Stop(AudioStopOptions.AsAuthored);
                }
                else
                {
                    cue.Play();
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // Allow the game to exit.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Check input.
            UpdateInput();

            // Update the audio engine.
            engine.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
