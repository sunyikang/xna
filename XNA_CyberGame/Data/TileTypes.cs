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

namespace Data
{
    public class TileTypes
    {
        public static TileType Empty;
        public static TileType Block;
        public static TileType Exit;
        public static TileType Platform;

        public static TileType GetTypeFromMark(char mark)
        {
            switch (mark)
            {
                // Blank space
                case '.':
                    return Empty;

                // Exit
                case 'X':
                    return Exit;

                // Platform block
                case '-':
                    return Platform;

                // Impassable block
                case '#':
                    return Block;

                // Unknown tile type character
                default:
                    return Empty;
            }
        }

        public TileTypes(ContentManager content)
        {
            Empty = new TileType('.', TileCollision.Passable, "", 64, 48, null);
            Block = new TileType('#', TileCollision.Impassable, "Block", 64, 48, content.Load<Texture2D>("Block"));
            Exit = new TileType('X', TileCollision.Passable, "Exit", 64, 48, content.Load<Texture2D>("Exit"));
            Platform = new TileType('-', TileCollision.Platform, "Platform", 64, 48, content.Load<Texture2D>("Platform"));
        }
    }

    public class TileType
    {
        public char Mark = '\0';
        public TileCollision Collision = TileCollision.Impassable;
        public string Name = "";
        public int Width = 0;
        public int Height = 0;
        public Texture2D Texture;

        public TileType(char mark, TileCollision collision, string name, int width, int height, Texture2D texture)
        {
            Mark = mark;
            Collision = collision;
            Name = name;
            Width = width;
            Height = height;
            Texture = texture;
        }
    }


    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    public enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,
    }
}
