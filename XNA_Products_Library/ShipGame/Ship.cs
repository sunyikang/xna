using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShipGame
{
    public delegate void FireDelegate();

    class Ship
    {        
        public Model Model;
        public bool isActive = true;
        public int LifeNum = Constants.ShipLife;
        public Vector3 Position = Vector3.Zero;
        public Vector3 InitialPosition = Vector3.Zero;
        public Vector3 Speed = Vector3.Zero;
        public int InvincibleTimeLeft = Constants.ShipInvincibleTime;
        public ShipOption Option;

        public Bullets bullets;
        public event FireDelegate FireDelegateHandler = null;

        float rotation = MathHelper.PiOver2;
        SoundEffect soundEngine;
        SoundEffectInstance soundInstance;        
        bool shouldSoundPlay = false;
        Matrix[] Transforms;        
        Matrix AdjustMatrix = Matrix.CreateRotationX(MathHelper.ToRadians(270.0f))
                                    * Matrix.CreateRotationY(MathHelper.ToRadians(180.0f))
                                    * Matrix.CreateRotationZ(MathHelper.ToRadians(90.0f));
        Matrix RotationMatrix = Matrix.Identity;
        int bulletProducingTime = 0;

        public int BulletProducingTime
        {
            get { return bulletProducingTime; }
            set 
            {
                if (value <= Constants.BulletProduceTime)
                {
                    bulletProducingTime = value;
                }
            }
        }
        

        public float Rotation
        {
            get { return rotation; }
            set 
            {
                float newVal = value;
                while (newVal >= MathHelper.TwoPi)
                {
                    newVal -= MathHelper.TwoPi;
                }
                while (newVal < 0)
                {
                    newVal += MathHelper.TwoPi;
                }

                if (Rotation != newVal)
                {
                    rotation = newVal;
                    RotationMatrix = Matrix.CreateRotationZ(rotation);
                }
            }
        }

        public Ship(Model model, SoundEffect soundEngine, Matrix projectionMatrix, Matrix viewMatrix, ShipOption option, Bullets bullets)
        {
            // 1. Model
            this.Model = model;

            // 2. Sound
            this.soundEngine = soundEngine;

            // 3. Transform
            Transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(Transforms);
            for (int i = 0; i < model.Bones.Count; i++)
            {
                Transforms[i] *= AdjustMatrix;
            }

            // 4. Meshes' Effects
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
            }

            // 5. ship option
            this.Option = option;

            // 6. bullets
            this.bullets = bullets;
        }

        public void DieOnce()
        {
            Position = InitialPosition;
            Rotation = MathHelper.PiOver2;
            Speed = Vector3.Zero;
            LifeNum--;
            InvincibleTimeLeft = Constants.ShipInvincibleTime;
            if (LifeNum == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isActive = false;
            Position.Z += Constants.CameraHeight;
            if (soundInstance != null)
            {
                soundInstance.Stop();
            }
        }

        public void Update(KeyboardState state)
        {
            // 0. change invincible time left
            if(InvincibleTimeLeft > 0)
                InvincibleTimeLeft--;

            // 1. change the speed
            Vector3 addSpeed = Vector3.Zero;
            addSpeed.X = Constants.ShipUnitSpeed * (float)Math.Cos(Rotation);
            addSpeed.Y = Constants.ShipUnitSpeed * (float)Math.Sin(Rotation);            

            // up
            if (state.IsKeyDown(Option.up))    // up = speed
            {
                Speed += addSpeed;
                shouldSoundPlay = true;
            }
            // down
            if (state.IsKeyDown(Option.down))     // down = stop
            {
                Speed *= 0.80f;
            }
            // left
            if (state.IsKeyDown(Option.left))   // left = rotation
            {
                Rotation += Constants.ShipUnitRotationSpeed;
            }
            // right
            if (state.IsKeyDown(Option.right))   // right = rotation
            {
                Rotation -= Constants.ShipUnitRotationSpeed;
            }
            // fire1
            if (state.IsKeyDown(Option.fire1))      // fire1
            {
                Fire1();
            }
            // fire2
            if (state.IsKeyDown(Option.fire2))
            {
                // un-supported
            }

            // 2. move location
            Position += Speed;
            Speed *= 0.95f;
            if (Position.X > Constants.PlayfieldSizeX)
            {
                Position.X -= Constants.PlayfieldSizeX * 2;
            }
            if (Position.X < -Constants.PlayfieldSizeX)
            {
                Position.X += Constants.PlayfieldSizeX * 2;
            }
            if (Position.Y > Constants.PlayfieldSizeY)
            {
                Position.Y -= Constants.PlayfieldSizeY * 2;
            }
            if (Position.Y < -Constants.PlayfieldSizeY)
            {
                Position.Y += Constants.PlayfieldSizeY * 2;
            }

            // 3. make soundEngine            
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
            shouldSoundPlay = false;
            
            // 4. update bullets
            bullets.Update();
            BulletProducingTime ++;
        }

        private void Fire1()
        {
            // create bullet when producing time over
            if (BulletProducingTime == Constants.BulletProduceTime)
            {
                Bullet b = new Bullet(Position, Rotation);
                bullets.List.Add(b);
                bullets.sound.Play(0.5f, 0.0f, 0.0f, false);

                if (this.FireDelegateHandler != null)
                {
                    this.FireDelegateHandler();
                }
                BulletProducingTime = 0;
            }
        }

        public void Draw()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (InvincibleTimeLeft > 0)
                    {
                        effect.Alpha = 0.1f;
                    }
                    else
                    {
                        effect.Alpha = 1.0f;
                    }
                    effect.World = Transforms[mesh.ParentBone.Index] * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(Position);
                }
                // draw the mesh, using the effects set above
                mesh.Draw();
            }

            bullets.Draw();
        }
    }
}
