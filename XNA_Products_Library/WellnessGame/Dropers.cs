using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoeEngine;

namespace WellnessGame
{
    public class Dropers
    {
        protected Texture2D textureShared;
        public List<Droper> List = new List<Droper>();

        public Dropers(Texture2D texture)
        {
            textureShared = texture;
        }

        public void Update(GameTime gameTime)
        {
            Create();
            for (int i = 0; i < List.Count; i++)
            {
                Droper one = List[i];
                if (!one.Alive)
                {
                    List.Remove(one);
                }
                else
                {
                    one.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (Droper one in List)
            {
                one.Draw(batch);
            }
        }

        protected virtual void Create()
        {}
    }

    public class Softwares : Dropers
    {
        public Softwares(Texture2D texture)
            : base(texture)
        {
        }

        protected override void Create()
        {
            if (Constant.GameRandom.NextDouble() < Constant.SoftwareCreationRate)
            {
                // new an Animations
                Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
                Animation bounce = new Animation(250, 60, 5, 50, 0);
                bounce.FramesPerSecond = 24;
                animations.Add("Bounce", bounce);
                Animation drop = new Animation(50, 60, 1, 0, 0);
                drop.FramesPerSecond = 24;
                animations.Add("Drop", drop);

                // new a Droper
                Droper one = new Droper(textureShared, Constant.NewVector(), Constant.SoftwareFallSpeed, animations);
                one.CurrentAnimationKey = "Drop";

                // add Droper to Dropers
                List.Add(one);
            }
            base.Create();
        }
    }

    public class Hardwares : Dropers 
    {
        public Hardwares(Texture2D texture)
            : base(texture)
        {
        }

        protected override void Create()
        {
            if (Constant.GameRandom.NextDouble() < Constant.HardwareCreationRate)
            {
                // new an Animations
                Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
                Animation bounce = new Animation(250, 60, 5, 50, 0);
                bounce.FramesPerSecond = 24;
                animations.Add("Bounce", bounce);
                Animation drop = new Animation(50, 60, 1, 0, 0);
                drop.FramesPerSecond = 24;
                animations.Add("Drop", drop);

                // new a Droper
                Droper one = new Droper(textureShared, Constant.NewVector(), Constant.HardwareFallSpeed, animations);
                one.CurrentAnimationKey = "Drop";

                // add Droper to Dropers
                List.Add(one);
            }
            base.Create();
        }
    }

    public class Droper : AnimatingSprite
    {
        public Droper(Texture2D texture, Vector2 position, float speed, Dictionary<string, Animation> animations)
        {
            this.Texture = texture;
            this.Position = position;
            this.Speed = speed;
            this.Animations = animations;
            
        }

        public void Update(GameTime gameTime)
        {
            Position.Y += Speed;

            if (Constant.IsTouchGround(Position.Y))
            {
                // Bounce animation
                CurrentAnimationKey = "Bounce";
                StartAnimation();
                if (Animations["Bounce"].currentFrameNo == 4)
                {
                    this.Alive = false;
                }
            }

            base.Update(gameTime);
        }   
    }
}
