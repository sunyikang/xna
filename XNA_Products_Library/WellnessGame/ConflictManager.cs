using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using RoeEngine;

namespace WellnessGame
{
    class ConflictManager
    {
        Player player;
        Softwares softwares;
        Hardwares hardwares;
        SoftwareScore softscore;
        HardwareScore hardscore;
        Computers computers;
        SoundEffect softwareSound;
        SoundEffect hardwareSound;

        public ConflictManager(Softwares softwares, Hardwares hardwares, Player player, 
            SoftwareScore softscore, HardwareScore hardscore, Computers computers, 
            SoundEffect softwareSound, SoundEffect hardwareSound)
        {
            this.player = player;
            this.softwares = softwares;
            this.hardwares = hardwares;
            this.softscore = softscore;
            this.hardscore = hardscore;
            this.computers = computers;
            this.softwareSound = softwareSound;
            this.hardwareSound = hardwareSound;
        }

        public void Check()
        {
            CheckSoftwareAndPlayer();
            CheckHardwareAndPlayer();
        }

        private void CheckSoftwareAndPlayer()
        {
            for (int i = 0; i < softwares.List.Count; i++ )
            {
                if (Intersects(softwares.List[i], player))
                {
                    // 1. add score
                    softscore.Num++;
                    if (softscore.Num > Constant.SoftScoreMax)
                    {
                        softscore.Num = 0;

                        // TODO: rescue a computer                        
                    }

                    // 2. remove ware
                    softwares.List.RemoveAt(i);

                    // 3. audio effect
                    softwareSound.Play(0.5f, 0.0f, 0.0f, false);
                }
            }
        }
        
        private void CheckHardwareAndPlayer()
        {
            for (int i = 0; i < hardwares.List.Count; i++)
            {
                if (Intersects(hardwares.List[i], player))
                {
                    // 1. add score
                    hardscore.Num++;
                    if (hardscore.Num > Constant.HardScoreMax)
                    {
                        // TODO: rescue a computer

                        hardscore.Num = 0;
                    }

                    // 2. remove ware
                    hardwares.List.RemoveAt(i);

                    // 3. audio effect
                    hardwareSound.Play(0.5f, 0.0f, 0.0f, false);
                }
            }
        }

        // check if two Rectangles intersect
        private bool Intersects(Rectangle a, Rectangle b)
        {
            return (a.Right > b.Left && a.Left < b.Right && 
                    a.Bottom > b.Top && a.Top < b.Bottom);
        }

        public bool Intersects(Droper droper, Player player)
        {
            Rectangle rcDroper = droper.CurrentRect();
            Rectangle rcPlayer = player.CurrentRect();

            return Intersects(Shrink(rcDroper, 3), Shrink(rcPlayer, 3));
        }

        private Rectangle Shrink(Rectangle rect, int rate)
        {
            Rectangle rc = rect;
            rc.Width /= rate;
            rc.X += rc.Width * (rate - 1) / 2;
            return rc;
        }
    }
}
