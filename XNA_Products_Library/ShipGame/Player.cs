//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Content;

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
    class Players
    {
        public List<Player> List = new List<Player>();
        public Players()   //int num, Matrix projectionMatrix, Matrix viewMatrix, Rocks rocks)
        {
            //this.Num = num;
            //if (num == 1)
            //{
            //    Score score1 = new Score(Constants.ScoreX1, Constants.ScoreY1);
            //    ShipOption option1 = new ShipOption(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.LeftControl, Keys.LeftControl);
            //    Player player1 = new Player("Player1", projectionMatrix, viewMatrix, rocks, score1, option1);
            //    List.Add(player1);
            //}
            //else if (num == 2)
            //{
            //    Score score1 = new Score(Constants.ScoreX1, Constants.ScoreY1);
            //    ShipOption option1 = new ShipOption(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.LeftControl, Keys.LeftControl);
            //    Player player1 = new Player("Player1", projectionMatrix, viewMatrix, rocks, score1, option1);
                
            //    Score score2 = new Score(Constants.ScoreX2, Constants.ScoreY2);
            //    ShipOption option2 = new ShipOption(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftControl, Keys.RightControl);
            //    Player player2 = new Player("Player1", projectionMatrix, viewMatrix, rocks, score2, option2);
            //}
            //else
            //{
            //    // error come here
            //}
        }

        public void Update(KeyboardState state)
        {
            for (int i = 0; i < List.Count; i++)
            {
                List[i].Update(state);
            }
        }

        public void Draw()
        {
            for (int i = 0; i < List.Count; i++)
            {
                List[i].Draw();
            }
        }
    }

    class Player
    {
        public String name;
        public Score score;
        public PlayerOption option;
        public Ship ship;

        public Player(String name, Ship ship, Score score, PlayerOption option)
        {
            this.name = name;
            this.ship = ship;
            this.score = score;
            this.option = option;
            ship.FireDelegateHandler += new FireDelegate(score.ShootPenalty);
        }

        public void Update(KeyboardState state)
        {
            // 1. ship update
            if (ship.isActive)
            {
                ship.Update(state);
            }
        }

        public void Draw()
        {
            if (ship.isActive)
            {
                ship.Draw();
            }
        }
    }

    class PlayerOption
    {
 
    }
}
