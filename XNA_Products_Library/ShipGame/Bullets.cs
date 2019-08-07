using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ShipGame
{
    class Bullets
    {
        public Model Model;
        public SoundEffect sound;
        public List<Bullet> List = new List<Bullet>();
        Matrix[] Transforms;        
        int produceTimeLeft = 0;
        
        public Bullets(Model model, SoundEffect sound, Matrix projectionMatrix, Matrix viewMatrix)
        {
            // 1. Model
            this.Model = model;

            // 2. Sound
            this.sound = sound;

            // 3. Transform
            Transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(Transforms);

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
        }

        public void Update()
        {
            // 1. update all bullets
            foreach (Bullet b in List)
            {
                b.Update();
            }

            // 2. remove the inactive bullets
            for (int i = 0; i < List.Count; i++)
            {
                if (!List[i].isActive)
                    List.RemoveAt(i);
            }

            //// 3. add new bullet
            //if (currentState.IsKeyDown(Keys.LeftControl))
            //{
            //    if (produceTimeLeft == 0)
            //    {
            //        Bullet b = new Bullet(shipPosition, shipRotation);
            //        Score.Instance.Num -= Constants.ScoreShootPenalty;
            //        List.Add(b);
            //        produceTimeLeft = Constants.BulletProduceTime;
            //        sound.Play(0.5f, 0.0f, 0.0f, false);
            //    }
            //    else
            //    {
            //        produceTimeLeft--;
            //    }
            //}
        }

        public void AddBullet(Bullet bullet)
        {
            List.Add(bullet);
            sound.Play(0.5f, 0.0f, 0.0f, false);
        }

        public void Draw()
        {
            foreach (Bullet b in List)
            {
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Transforms[mesh.ParentBone.Index] * Matrix.CreateRotationZ(b.Rotation) * Matrix.CreateTranslation(b.Position);
                    }
                    mesh.Draw();
                }
            }
        }
    }

    class Bullet
    {
        public Vector3 Position;
        public float Rotation;
        public bool isActive = true;

        public Bullet(Vector3 position, float rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public void Die()
        {
            isActive = false;
        }

        public void Update()
        {
            Vector3 move = Vector3.Zero;
            move.X = Constants.BulletSpeed * (float) Math.Cos(Rotation);
            move.Y = Constants.BulletSpeed * (float) Math.Sin(Rotation);
            Position += move;

            if (Position.X > Constants.PlayfieldSizeX ||
                Position.X < -Constants.PlayfieldSizeX ||
                Position.Y > Constants.PlayfieldSizeY ||
                Position.Y < -Constants.PlayfieldSizeY)
                isActive = false;
        }
    }

}
