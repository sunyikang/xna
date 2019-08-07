﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resource
{
    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    public enum TileCollision
    {
        // A passable tile is one which does not hinder player motion at all.
        Passable = 0,

        // An impassable tile is one which does not allow the player to move through it at all. It is completely solid.
        Impassable = 1,

        // A platform tile is one which behaves like a passable tile except when the player is above it. 
        // A player can jump up through a platform as well as move past it to the left and right, but can not fall down through the top of it.
        Platform = 2,
    }
}
