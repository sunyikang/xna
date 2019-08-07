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

namespace AppModelDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model Box;

        private bool paused = false;
        private bool pauseKeyDown = false;

        // Time out limit in ms.
        static private int TimeOutLimit = 8000;

        // Amount of time that has passed.
        private double timeoutCount = 0;

        // Speed in world units per ms.
        private double speed = 0.02f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Components.Add(new GamerServicesComponent(this));

            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler(Window_ClientSizeChanged);

            this.Exiting += new EventHandler(Game1_Exiting);

            this.IsFixedTimeStep = false;
        }

        void Game1_Exiting(object sender, EventArgs e)
        {
            // Add any code that must execute before the game ends.
        }


        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            // Make changes to handle the new window size.            
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

            // TODO: Load your game content here            
            Box = Content.Load<Model>("box");
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
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            // Time elapsed since the last call to update.
            double elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            // Multiply speed by elapsed time to get the distance moved.
            double distance = (speed * elapsedTime);

            // Check to see if there has been any activity
            
            if (checkActivity(keyboardState, gamePadState) == false)
            {
                timeoutCount += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
                timeoutCount = 0;

            // Timeout if idle long enough
            if (timeoutCount > TimeOutLimit)
            {
                Exit();
                base.Update(gameTime);
                return;
            }

            // Check to see if the user has exited
            if (checkExitKey(keyboardState, gamePadState))
            {
                base.Update(gameTime);
                return;
            }

            // Check to see if the user has paused or unpaused
            checkPauseKey(keyboardState, gamePadState);

            // Pause if the Guide is up, or if the user has paused
            if ((paused == false) && (Guide.IsVisible == false))
            {
                base.Update(gameTime);

                Simulate(gameTime);
            }
        }

        private void Simulate(GameTime gameTime)
        {
            // Add game logic.
            // Time elapsed since the last call to update.
            double elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

            // Multiply speed by elapsed time to get the distance moved.
            double distance = (speed * elapsedTime);
        }

        private void checkPauseKey(KeyboardState keyboardState, GamePadState gamePadState)
        {
            if (keyboardState.IsKeyDown(Keys.P) || (gamePadState.Buttons.Y == ButtonState.Pressed))
            {
                pauseKeyDown = true;
                paused = true;
            }
            else if (pauseKeyDown)
            {
                pauseKeyDown = false;
                paused = false;
            }
        }

        bool checkExitKey(KeyboardState keyboardState, GamePadState gamePadState)
        {
            // Check to see whether ESC was pressed on the keyboard or BACK was pressed on the controller.
            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
            {
                Exit();
                return true;
            }
            return false;
        }

        GamePadState blankGamePadState = new GamePadState(new GamePadThumbSticks(), new GamePadTriggers(), 
            new GamePadButtons(), new GamePadDPad());
        bool checkActivity(KeyboardState keyboardState, GamePadState gamePadState)
        {
            // Check to see if the input states are different from last frame
            //KeyboardState blankKeyboardState = new KeyboardState();
            GamePadState nonpacketGamePadState = new GamePadState(gamePadState.ThumbSticks, gamePadState.Triggers, gamePadState.Buttons, gamePadState.DPad);           
            //if ((blankKeyboardState == keyboardState) && (blankGamePadState == nonpacketGamePadState))
            bool keybidle = keyboardState.GetPressedKeys().Length == 0;
            bool gamepidle = blankGamePadState == nonpacketGamePadState;
            if ( keybidle && gamepidle)
            {
                // no activity;
                return false;
            }
            return true;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (timeoutCount > 0)
                GraphicsDevice.Clear(Color.LawnGreen);
            else
                GraphicsDevice.Clear(Color.CornflowerBlue);

            if (paused)
                GraphicsDevice.Clear(Color.DarkGray);

            // TODO: Add your drawing code here
            

            base.Draw(gameTime);
        }
    }
}
