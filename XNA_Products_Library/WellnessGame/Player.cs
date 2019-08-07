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
    public class Player : AnimatingSprite
    {
        SoundEffect soundEngine;
        SoundEffectInstance soundInstance; 
        bool shouldSoundPlay = false;

        public Player(Texture2D texture, Vector2 position, float speed, SoundEffect soundEngine)
        {
            this.Texture = texture;
            this.Position = position;
            this.Speed = speed;
            this.Position.Y -= Texture.Height;
            Texture.GetData<Color>(new Color[Texture.Width * Texture.Height]);
            this.soundEngine = soundEngine;

            // animation
            Animation up = new Animation(128, 64, 2, 0, 0);
            Animation down = new Animation(128, 64, 2, 128, 0);
            Animation left = new Animation(128, 64, 2, 256, 0);
            Animation right = new Animation(128, 64, 2, 384, 0);
            this.Animations.Add("Up", up);
            this.Animations.Add("Down", down);
            this.Animations.Add("Left", left);
            this.Animations.Add("Right", right);
            CurrentAnimationKey = "Up";
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

            // 1. position            
            if (keyboard.IsKeyDown(Keys.Left) || gamePad.DPad.Left == ButtonState.Pressed)
            {
                Position.X -= Speed;
                CurrentAnimationKey = "Left";
                StartAnimation();
                shouldSoundPlay = true;
            }
            else if (keyboard.IsKeyDown(Keys.Right) || gamePad.DPad.Right == ButtonState.Pressed)
            {
                Position.X += Speed;
                CurrentAnimationKey = "Right";
                StartAnimation();
                shouldSoundPlay = true;
            }
            else if (keyboard.IsKeyDown(Keys.Space) || gamePad.Buttons.A == ButtonState.Pressed)
            {   // open mouth 
                CurrentAnimationKey = "Up";
                StartAnimation();
                shouldSoundPlay = false;
            }
            else
            {
                CurrentAnimationKey = "Down";
                StopAnimation();
                shouldSoundPlay = false;
            }

            // prevent the person from moving off of the screen
            Position.X = MathHelper.Clamp(Position.X, Constant.SafeBound.Left, Constant.SafeBound.Right);
            base.Update(gameTime);

            // 2. sound
            if (shouldSoundPlay)
            {
                if (soundInstance == null)
                {
                    soundInstance = soundEngine.Play(0.5f, 0.0f, 0.0f, true);
                }
                else
                {
                    soundInstance.Resume();
                }
            }
            else
            {
                if (soundInstance != null)
                {
                    soundInstance.Pause();
                }
            }
        }
    }
}
