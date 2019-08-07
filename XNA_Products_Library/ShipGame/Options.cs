using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShipGame
{
    class GameOptions
    {
        private Game game;

        public GameOptions(Game game)
        {
            this.game = game;
        }

        public void Update(KeyboardState currentState)
        {
            // Allows the game to exit
            if (currentState.IsKeyDown(Keys.Escape))
                game.Exit();
        }
    }

    class ShipOption
    {
        public Keys up = Keys.Up;
        public Keys down = Keys.Down;
        public Keys left = Keys.Left;
        public Keys right = Keys.Right;
        public Keys fire1 = Keys.RightControl;
        public Keys fire2 = Keys.RightControl;

        public ShipOption(Keys up, Keys down, Keys left, Keys right, Keys fire1, Keys fire2)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.fire1 = fire1;
            this.fire2 = fire2;
        }
    }
        
    class Score
    {        
        public int Num = 0;
        public SpriteFont Console;
        public Vector2 Position;

        public Score(float X, float Y, SpriteFont console)
        {
            Position.X = X;
            Position.Y = Y;
            this.Console = console;
        }

        public void ShootPenalty()
        {
            Num -= Constants.ScoreShootPenalty;
        }

        public void KillRockRonus()
        {
            Num += Constants.ScoreKillRockBonus;
        }

        public void DeathPenalty()
        {
            Num -= Constants.ScoreDeathPenalty;
        }
    }
    
    
    //public delegate void RockCrashDelegate();
    //public delegate void ShipCrashDelegate();            

    class CrashChecker
    {
        public Players players;
        public Rocks rocks;
        public SoundEffect soundShipBoom;
        public SoundEffect soundRockBoom;
        public int shipLife = Constants.ShipLife;
        //public event RockCrashDelegate RockCrashDelegateHandler = null;
        //public event ShipCrashDelegate ShipCrashDelegateHandler = null;

        public CrashChecker(Players players, Rocks rocks, SoundEffect soundShipBoom, SoundEffect soundRockBoom)
        {
            this.players = players;
            this.rocks = rocks;
            this.soundShipBoom = soundShipBoom;
            this.soundRockBoom = soundRockBoom;
        }

        public void Check()
        {
            for (int i=0; i<players.List.Count; i++)
            {
                Player player = players.List[i];
                Ship ship = player.ship;
                Bullets bullets = player.ship.bullets;
                Score score = player.score;

                CheckRocksBullets(bullets, score);
                CheckRocksShip(ship, score);
            }
        }

        private void CheckRocksShip(Ship ship, Score score)
        {
            if (ship.InvincibleTimeLeft > 0)
                return;

            // check rocks and ship
            BoundingSphere shipSphere = new BoundingSphere(ship.Position, ship.Model.Meshes[0].BoundingSphere.Radius * Constants.ShipBoundingSphereScale);

            foreach (Rock rock in rocks.List)
            {
                BoundingSphere rockSphere = new BoundingSphere(rock.Position, rocks.Model.Meshes[0].BoundingSphere.Radius * Constants.RockBoundingSphereScale);
                if (rockSphere.Intersects(shipSphere))
                {
                    rock.Die();
                    ship.DieOnce();
                    score.DeathPenalty();
                    soundShipBoom.Play(0.5f, 0.0f, 0.0f, false);
                    soundRockBoom.Play(0.5f, 0.0f, 0.0f, false);
                    break; //exit the loop
                }
            }
        }

        private void CheckRocksBullets(Bullets bullets, Score score)
        {
            // check rocks and bullets
            foreach (Rock rock in rocks.List)
            {
                BoundingSphere rSphere = new BoundingSphere(rock.Position, rocks.Model.Meshes[0].BoundingSphere.Radius * Constants.ShipBoundingSphereScale);
                foreach (Bullet bullet in bullets.List)
                {
                    BoundingSphere bSphere = new BoundingSphere(bullet.Position, bullets.Model.Meshes[0].BoundingSphere.Radius);
                    if (rSphere.Intersects(bSphere))
                    {
                        rock.Die();
                        bullet.Die();
                        score.KillRockRonus();
                        soundRockBoom.Play(0.5f, 0.0f, 0.0f, false);
                    }
                }
            }
        }
    }
}
