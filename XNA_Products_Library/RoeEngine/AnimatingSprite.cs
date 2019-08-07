using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoeEngine
{
    /// <summary>
    /// Our general purpose Animation set. Contains an array
    /// of source Rectangles for our AnimatedSprite to use when
    /// rendering. Also handles updating itself based on a given
    /// frame rate.
    /// </summary>
    public class Animation
    {
        internal Rectangle[] frames;            //the frames of our animation
        private float frameLength = 1f / 5f;    //how long each frame lasts (in seconds)        
        float timer = 0f;                       //a timer to use for tracking frame time
        public int currentFrameNo = 0;          //the current frame

        /// <summary>
        /// Gets the current frame as a source Rectangle
        /// </summary>
        public Rectangle CurrentFrame
        {
            get { return frames[currentFrameNo]; }
        }

        public void Next()
        {
            if (currentFrameNo < frames.Length - 1)
            {
                currentFrameNo++;
            }
        }

        /// <summary>
        /// Gets or sets the animation speed
        /// </summary>
        public float FramesPerSecond
        {
            //we need to convert from our floating point frameLength
            //variable to an integer number that represents frames per
            //second and vice versa.
            get { return (int)(1f / frameLength); }
            set { frameLength = 1f / value; }
        }

        /// <summary>
        /// Creates a new Animation
        /// </summary>
        /// <param name="width">The total width of the animation</param>
        /// <param name="height">The height of the animation</param>
        /// <param name="numFrames">The number of frames in the animation</param>
        /// <param name="xOffset">The offset along the X axis</param>
        /// <param name="yOffset">The offset along the Y axis</param>
        public Animation(int width, int height, int numFrames, int xOffset, int yOffset)
        {
            //create our frames array
            frames = new Rectangle[numFrames];

            //determine the width of a single frame
            int frameWidth = width / numFrames;

            //create the frame Rectangles
            for (int i = 0; i < numFrames; i++)
            {
                //each Rectangle is created in a row. we use the xOffset as the starting point for
                //the Rectangle's X parameter and add to it for each frame. Since each frame is in
                //a horizontal line, the yOffset is used for the Y parameter. Each frame is the same
                //width and height so we use frameWidth and height for the Rectangle's width and height
                //parameters, respectively.
                frames[i] = new Rectangle(xOffset + (frameWidth * i), yOffset, frameWidth, height);
            }
        }

        /// <summary>
        /// Updates the animation
        /// </summary>
        /// <param name="gameTime">The current gameTime</param>
        public void Update(GameTime gameTime)
        {
            //add to our timer based on the time past
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Console.WriteLine(timer);
            //if the timer has past the frameLength...
            if (timer >= frameLength)
            {
                //reset the timer to 0
                timer = 0f;

                //increment the currentFrame. we use the mod function (%) to make sure
                //our currentFrame stays within the bounds of the frames array.
                currentFrameNo = (currentFrameNo + 1) % frames.Length;
            }
        }

        /// <summary>
        /// Resets the currentFrame and timer to 0
        /// </summary>
        public void Reset()
        {
            //simply resetting the animation back to 0.
            currentFrameNo = 0;
            timer = 0f;
        }
    }

    /// <summary>
    /// A basic class for creating an animated sprite. The Sprite
    /// maintains its own position as well as a Dictionary of animations
    /// it can use.
    /// </summary>
    public class AnimatingSprite
    {
        public Vector2 Position = Vector2.Zero;         // the current position of the sprite
        public float Rotation = 1.0f;
        public float Speed = 5.0f;
        public bool Alive = true;
        public Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();        // A set of Animations the sprite can use
        public Texture2D Texture;                       // the sprite's texture sheet
        bool updateAnimation = true;                    // whether or not to Update the animation     
   
        string currentAnimationKey;                     // the current animation key
        public string CurrentAnimationKey
        {
            get { return currentAnimationKey; }

            set
            {                
                if (!Animations.ContainsKey(value))
                {   //if the value passed in is not a key in our dictionary, we can't use it so we will throw an exception.
                    throw new Exception("Invalid animation specified.");
                }
                if (currentAnimationKey == null || !currentAnimationKey.Equals(value))
                {
                    //set the current animation
                    currentAnimationKey = value;

                    //reset the animation
                    Animations[currentAnimationKey].Reset();
                }
            }
        }

        public AnimatingSprite()
        {
            // e.g.
            //Animation bounce = new Animation(250, 60, 5, 50, 0);
            //bounce.FramesPerSecond = 24;
            //Animation drop = new Animation(50, 60, 1, 0, 0);
            //drop.FramesPerSecond = 24;
            //this.Animations.Add("Bounce", bounce);
            //this.Animations.Add("Drop", drop);
            //this.CurrentAnimationKey = "Drop";
        }

        public Rectangle CurrentRect()
        {
            Rectangle rect;
            rect.X = (int) Position.X;
            rect.Y = (int) Position.Y;
            rect.Width = Animations[currentAnimationKey].CurrentFrame.Width;
            rect.Height = Animations[currentAnimationKey].CurrentFrame.Height;
            return rect;
        }

        public void AddAnimation(string key, Animation animation)
        {
            Animations.Add(key, animation);
        }

        // Starts animating the sprite if not already animating.
        public void StartAnimation()
        {
            updateAnimation = true;
        }

        // Stops animating the sprite.
        public void StopAnimation()
        {
            updateAnimation = false;
        }

        public void Update(GameTime gameTime)
        {
            // AnimationSprite has at list 1 Animation, or no Animation picture updated
            if (Animations.Keys.Count == 0)
                return;

            if (updateAnimation)
                Animations[CurrentAnimationKey].Update(gameTime);
        }

        // Draws the sprite with the given SpriteBatch
        public void Draw(SpriteBatch batch)
        {
            // AnimationSprite has at list 1 Animation, or no Animation picture updated
            if (Animations.Keys.Count == 0)
                return;

            //simple draw call. we use our Animations[currentAnimation].CurrentFrame to get the source rectangle to use for picking our sprite.
            batch.Draw(Texture, Position, Animations[currentAnimationKey].CurrentFrame, Color.White);
        }
    }
}