using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Resource;
using Configuration;

namespace Data
{
    public class Tiles
    {
        public List<Tile> List = new List<Tile>();  // don't store the Empty TileResource
        TileFactory factory;

        public Tiles(LevelContent levelContent, ContentManager content)
        {
            factory = new TileFactory(content);

            // according the level content, create tile and insert it into tile list.
            for (int y = 0; y < levelContent.LineNum; y++)
            {
                for (int x = 0; x < levelContent.ColumnNum; x++)
                {
                    // to load each tile.
                    char mark = levelContent.LineList[y][x];                    
                    if (!factory.MarkTileTable.ContainsKey(mark))
                        continue;

                    CreateTileDelegate createTile = (CreateTileDelegate)(factory.MarkTileTable[mark]);
                    Tile tile = createTile(x, y);

                    if (null != tile)
                    {
                        List.Add(tile);
                    }
                }
            }
        }
    }
}
