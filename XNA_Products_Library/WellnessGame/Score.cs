using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoeEngine;

namespace WellnessGame
{
    class Score : AnimatingSprite
    {
        private int num = 0;

        public int Num
        {
            get { return num; }
            set { num = value; }
        }
    }


    class SoftwareScore : Score
    {

    }

    class HardwareScore : Score
    { }
}
