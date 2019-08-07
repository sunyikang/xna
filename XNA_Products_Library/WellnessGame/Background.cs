using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoeEngine;

namespace WellnessGame
{
    class Background
    {
        Texture2D texture;

        public Background(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {}

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, Vector2.Zero, Color.White);
        }
    }
}
