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
    public class Levels
    {
        public List<Level> List = new List<Level>();
        ContentManager Content;

        public Levels(ContentManager content)
        {
            Content = content;
            LoadLevels();
        }

        void LoadLevels()
        {
            foreach (string path in LevelResource.Instance.LevelFilePathList)
            {
                // 1. load content from txt file
                if (!File.Exists(path))
                    throw new Exception(String.Format("File not exist: \n{0}.", path));

                LevelContent levelContent = new LevelContent();
                using (StreamReader reader = new StreamReader(path))
                {
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        levelContent.LineList.Add(line);
                    }
                }
                levelContent.Check();

                // 2. convert content to level
                Level level = new Level(levelContent, Content);

                // 3. add level to level list
                List.Add(level);
            }
        }
    }

    public class Level
    {
        public Tiles Tiles;

        public Level(LevelContent levelContent, ContentManager content)
        {
            Tiles = new Tiles(levelContent, content);
        }

        public void Update()
        { }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in Tiles.List)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
