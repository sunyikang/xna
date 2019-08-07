using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Configuration;

namespace Resource
{
    public delegate Tile CreateTileDelegate(int x, int y);

    public class TileFactory
    {
        public Hashtable MarkTileTable = new Hashtable(7);
        ContentManager Content;

        public TileFactory(ContentManager content)
        {
            Content = content;
            Init();
        }

        Tile CreateEmptyTile(int x, int y)
        {
            return null;
        }

        Tile CreateBlockTile(int x, int y)
        {
            return new Tile("Block", GetLocation(x, y), TileCollision.Impassable, Content.Load<Texture2D>("Tiles/Block"));
        }

        Tile CreateExitTile(int x, int y)
        {
            return new Tile("Exit", GetLocation(x, y), TileCollision.Passable, Content.Load<Texture2D>("Tiles/Exit"));
        }

        Tile CreatePlatformTile(int x, int y)
        {
            return new Tile("Platform", GetLocation(x, y), TileCollision.Platform, Content.Load<Texture2D>("Tiles/Platform"));
        }

        Tile CreateGemTile(int x, int y)
        {
            return new Tile("Gem", GetCenter(x, y), TileCollision.Passable, Content.Load<Texture2D>("Tiles/Gem"));
        }

        private void Init()
        {
            MarkTileTable.Add('.', new CreateTileDelegate(CreateEmptyTile));   // do nothing for empty
            MarkTileTable.Add('#', new CreateTileDelegate(CreateBlockTile));
            MarkTileTable.Add('X', new CreateTileDelegate(CreateExitTile));
            MarkTileTable.Add('-', new CreateTileDelegate(CreatePlatformTile));
            MarkTileTable.Add('G', new CreateTileDelegate(CreateGemTile));
        }

        Vector2 GetCenter(int x, int y)
        {
            Rectangle rect = new Rectangle(x * Constant.TileWidth, y * Constant.TileHeight, Constant.TileWidth, Constant.TileHeight);
            Point center = rect.Center;
            center.X -= Constant.TileWidth / 4;
            center.Y -= Constant.TileHeight / 4;
            return new Vector2(center.X, center.Y);
        }

        Vector2 GetLocation(int x, int y)
        {
            Rectangle rect = new Rectangle(x * Constant.TileWidth, y * Constant.TileHeight, Constant.TileWidth, Constant.TileHeight);
            Point location = rect.Location;
            return new Vector2(location.X, location.Y);
        }

    }


    public class Tile
    {
        public string Name = "";
        public Vector2 Position = Vector2.Zero;
        public TileCollision Collision = TileCollision.Impassable;
        public Texture2D Texture;

        public Tile(string name, Vector2 position, TileCollision collision, Texture2D texture)
        {
            Name = name;
            Collision = collision;
            Texture = texture;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }


}
