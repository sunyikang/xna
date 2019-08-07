using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace WellnessGame
{
    public class Constant
    {
        // constant data
        //public const float TileSize = 40.2f;           // The size of the tile model.
        //public const float StartingPieceHeight = 50f;  // The starting height of the pieces over the board.
        //public const int FallingPieceFrames = 10;      // The number of frames in the falling-piece animation.
        //public const int fruitWidth = 50;
        //public const int fruitHeight = 60;


        // player data
        public static readonly Vector2 PlayerStartPosition = new Vector2(380, 720);
        public static readonly float PlayerSpeed = 5.0f;
        public static readonly string PlayerTexturePath = "Gameplay/MyAvatar_iteboy";

        // background data
        public static readonly string BackgroundTexturePath = "Gameplay/bg";
        public static readonly Rectangle SafeBound = new Rectangle(380, 0, 460, 720);

        // software data
        public static readonly string SoftwareTexturePath = "Gameplay/antiVirusSoftware";
        public static readonly float SoftwareFallSpeed = 3.0f;
        public static readonly double SoftwareCreationRate = 0.005f;
        public static readonly double SoftwareHeight = 60;
        public static readonly double SoftwareWidth = 50;

        // hardware data
        public static readonly string HardwareTexturePath = "Gameplay/Hardware";
        public static readonly double HardwareCreationRate = 0.008f;
        public static readonly float HardwareFallSpeed = 6.0f;

        // clock data


        // computer data



        // other data
        public static readonly Random GameRandom = new Random();
        public static readonly int SoftScoreMax = 10;
        public static readonly int HardScoreMax = 10;


        // global methods
        public static Vector2 NewVector()
        {
            double rate = GameRandom.NextDouble();
            double x = SafeBound.Width * rate + SafeBound.X;
            return new Vector2((float)x, 0);
        }

        public static bool IsTouchGround(float height)
        {
            if (height + SoftwareHeight > SafeBound.Bottom)
            {
                return true;
            }
            return false;
        }     
    }
}
