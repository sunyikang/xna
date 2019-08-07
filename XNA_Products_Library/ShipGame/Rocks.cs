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
    class Rocks
    {
        public Model Model;
        SoundEffect sound;
        Matrix[] Transforms;
        public List<Rock> List = new List<Rock>(Constants.RocksNum);

        public Rocks(Model model, SoundEffect sound, Matrix projectionMatrix, Matrix viewMatrix)
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

            // 5. produce all Rock
            for (int i = 0; i < Constants.RocksNum; i++)
            {
                List.Add(RockFactory.Instance.Produce());
            }
        }

        public void Update()
        {
            // 1. update all rocks
            foreach (Rock rock in List)
            {
                rock.Update();
            }

            // 2. remove inactive rocks
            for (int i = 0; i < List.Count; i++)
            {
                if (!List[i].isActive)
                {
                    List.RemoveAt(i);
                }
            }      
        }

        public void Draw()
        {
            foreach (Rock rock in List)
            {
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {                        
                        effect.World = Transforms[mesh.ParentBone.Index] * Matrix.CreateRotationZ(rock.Rotation) * Matrix.CreateTranslation(rock.Position);                       
                    }
                    mesh.Draw();
                }
            }
        }
    }

    class Rock
    {
        public Vector3 Position;
        public float Speed;
        public float Rotation;        
        public bool isActive = true;

        public Rock(Vector3 position, float speed, float rotation)
        {
            Position = position;
            Speed = speed;
            Rotation = rotation;
        }

        public void Die()
        {
            isActive = false;
        }

        public void Update()
        {
            Vector3 move = Vector3.Zero;
            move.X = Speed * (float)Math.Cos(Rotation);
            move.Y = Speed * (float)Math.Sin(Rotation);
            Position += move;

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
        }
    }

    class RockFactory
    {
        Random random = new Random();
        static RockFactory instance = null;

        public static RockFactory Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new RockFactory();
                }
                return RockFactory.instance; 
            }            
        }
        
        public Rock Produce()
        {
            // 1. Location
            Vector3 position = Vector3.Zero;
            position.X = Constants.PlayfieldSizeX * ((float)random.NextDouble() - 0.5f);
            position.Y = Constants.PlayfieldSizeY * ((float)random.NextDouble() - 0.5f);

            // 2. Speed
            float speed = (Constants.RockMaxSpeed - Constants.RockMinSpeed) * (float)random.NextDouble() + Constants.RockMinSpeed;

            // 3. Rotation
            //float rotation = Math.Atan (double) ( (position.Y - ship.Position.Y) / (position.X - ship.Position.X) );
            float rotation = MathHelper.Pi * 2 * (float)random.NextDouble();

            return new Rock(position, speed, rotation);
        }
    }
}
