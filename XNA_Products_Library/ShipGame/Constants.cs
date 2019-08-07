using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShipGame
{
    class Constants
    {
        // player constants
        public const int PlayersNum = 2;

        // camera constants
        public const float CameraHeight = 50000.0f;
        public const float PlayfieldSizeX = 32000f;
        public const float PlayfieldSizeY = 22000f;

        // ship constants
        public const float ShipUnitSpeed = 10.0f;
        public const float ShipUnitRotationSpeed = 0.05f;
        public const float ShipBoundingSphereScale = 0.5f;  //50% size
        public const int   ShipLife = 3;
        public const int   ShipInvincibleTime = 100;
        public const float ShipX0 = - PlayfieldSizeX * 1/10;
        public const float ShipY0 = 0;
        public const float ShipX1 = PlayfieldSizeX * 1 / 10;
        public const float ShipY1 = 0;

        // rock constants
        public const int   RocksNum = 10;
        public const float RockMinSpeed = 50.0f;
        public const float RockMaxSpeed = 150.0f;
        public const float RockspeedAdjustment = 5.0f;
        public const float RockBoundingSphereScale = 0.95f;  //95% size
        
        // bullet constants
        public const float BulletSpeed = 400.0f;
        public const int   BulletProduceTime = 10;   // wait for so many times then ship can create a bullet

        // score constants
        public const int ScoreX0 = 50;
        public const int ScoreY0 = 50;
        public const int ScoreX1 = 600;
        public const int ScoreY1 = 50;
        public const int ScoreShootPenalty = 1;
        public const int ScoreDeathPenalty = 100;
        public const int ScoreWarpPenalty = 50;
        public const int ScoreKillRockBonus = 25;
    }
}
