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

namespace ShipGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // camera / view info
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, Constants.CameraHeight);
        CrashChecker crashChecker;

        // objects
        Texture2D background;
        GameOptions options;
        Rocks rocks;
        Players players = new Players();

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
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                                      MathHelper.ToRadians(45.0f), 
                                      graphics.GraphicsDevice.DisplayMode.AspectRatio,
                                      Constants.CameraHeight - 1000.0f,
                                      Constants.CameraHeight + 1000.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            base.Initialize();            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // 1. create SpriteBatch and draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Textures/B1_stars");

            // 2. load option and rocks 
            options = new GameOptions(this);
            
            rocks = new Rocks(Content.Load<Model>("Models/rock"),
                                    Content.Load<SoundEffect>("Audios/explosion_rock"),
                                    projectionMatrix, viewMatrix);

            // 3. load players
            if(Constants.PlayersNum == 1)
            {
                players.List.Add(CreatePlayer());

                // change player0 info
                players.List[0].name = "Player0";
                players.List[0].ship.InitialPosition = new Vector3(Constants.ShipX0, Constants.ShipY0, 0);
                players.List[0].ship.Position = players.List[0].ship.InitialPosition;
            }
            else if(Constants.PlayersNum == 2)
            {
                for (int i = 0; i < Constants.PlayersNum; i++)
                {
                    players.List.Add(CreatePlayer());
                }

                // change player0 info
                players.List[0].name = "Player0";
                players.List[0].ship.InitialPosition = new Vector3(Constants.ShipX0, Constants.ShipY0, 0);
                players.List[0].ship.Position = players.List[0].ship.InitialPosition;

                // change player1 info 
                players.List[1].name = "Player1";
                players.List[1].ship.InitialPosition = new Vector3(Constants.ShipX1, Constants.ShipY1, 0);
                players.List[1].ship.Position = players.List[1].ship.InitialPosition;
                players.List[1].score.Position.X = Constants.ScoreX1;
                players.List[1].score.Position.Y = Constants.ScoreY1;                
                players.List[1].ship.Option.up = Keys.W;
                players.List[1].ship.Option.down = Keys.S;
                players.List[1].ship.Option.left = Keys.A;
                players.List[1].ship.Option.right = Keys.D;
                players.List[1].ship.Option.fire1 = Keys.LeftControl;
                players.List[1].ship.Option.fire2 = Keys.LeftControl;
            }
            else
            {
                // un-support yet
            }
            
            // 4. create crash checker
            crashChecker = new CrashChecker(players, rocks,
                                            Content.Load<SoundEffect>("Audios/explosion_ship"),
                                            Content.Load<SoundEffect>("Audios/explosion_rock"));
        }

        private Player CreatePlayer()
        {
            ShipOption shipOption = new ShipOption(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.RightControl, Keys.RightControl);
            
            Bullets bullets = new Bullets(Content.Load<Model>("Models/bullet"),
                                    Content.Load<SoundEffect>("Audios/fire"),
                                    projectionMatrix, viewMatrix);

            Ship ship = new Ship(Content.Load<Model>("Models/ship"),
                        Content.Load<SoundEffect>("Audios/engine"),
                        projectionMatrix, viewMatrix, shipOption, bullets);

            Score score = new Score(Constants.ScoreX0, 
                                    Constants.ScoreY0,
                                    Content.Load<SpriteFont>("Fonts/Lucida Console"));

            PlayerOption playerOption = new PlayerOption();

            return new Player("", ship, score, playerOption);            
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
            KeyboardState state = Keyboard.GetState();

            // 1. game option
            options.Update(state);

        
            // 2. rocks update
            rocks.Update();

            // 3. players update
            players.Update(state);

            // 4. crash check
            crashChecker.Check();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // 1. draw background
            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), Color.White);
            spriteBatch.End();

            // 2. draw objects
            players.Draw();
            rocks.Draw();

            // 3. draw score
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            foreach (Player one in players.List)
            {
                spriteBatch.DrawString(one.score.Console, "[" + one.name + "]: " + one.score.Num.ToString(), one.score.Position, Color.LightGreen);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
     }
}
