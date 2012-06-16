#region License
/*
 * Microsoft Public License (Ms-PL)
 * 
 * This license governs use of the accompanying software. If you use the software, you accept this license. 
 * If you do not accept the license, do not use the software.
 *
 * 1. Definitions
 *
 * The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
 *
 * A "contribution" is the original software, or any additions or changes to the software.
 *
 * A "contributor" is any person that distributes its contribution under this license.
 *
 * "Licensed patents" are a contributor's patent claims that read directly on its contribution.
 *
 * 2. Grant of Rights
 *
 * (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
 * each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, 
 * prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
 *
 * (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
 * each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, 
 * use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the 
 * contribution in the software.
 *
 * 3. Conditions and Limitations
 *
 * (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
 *
 * (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
 * your patent license from such contributor to the software ends automatically.
 *
 * (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, 
 * and attribution notices that are present in the software.
 *
 * (D) If you distribute any portion of the software in source code form, you may do so only under this license by including 
 * a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, 
 * you may only do so under a license that complies with this license.
 *
 * (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. 
 * You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, 
 * the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement. 
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Varpal.Base;
using Varpal.Objects;
using Varpal.Objects._2D;

namespace Varpal.Managers
{
    // This file will be deleted, just up for testing atm
    // All GameScreens made from now on should be made in the 
    // Screens Dir.
    public class BlankScreen : GameScreen
    {
        GameObject3D ground;
        Camera gameCamera;

        Player player;
        List<CollectableItem> Items = new List<CollectableItem>();
        List<ImpassibleObject> Barriers = new List<ImpassibleObject>();
        Random random;
        Status _status;

        public BlankScreen(ScreenManager mngr)
            : base(mngr)
        {
            
        }

        public override void Initialize()
        {
            ground = new GameObject3D(this);
            gameCamera = new Camera(this);
            player = new Player(this);
            random = new Random();
            _status = new Status(this);
            base.Initialize();
        }

        public override void LoadContent()
        {
            
            ground.Model = Manager.Content.Load<Model>("Models/Ground/ground3");
            player.Model = Manager.Content.Load<Model>("Models/Player/playerhead");

            //Initialize fuel cells
            
            for (int index = 0; index < GameConstants.NumItems; index++)
            {
                CollectableItem _item = new CollectableItem(this);
                _item.LoadContent(Manager.Content, "Models/Containers/Jug1");
                Items.Add(_item);
            }
            // Init Barriers
            
            int randomBarrier = random.Next(3);
            string barrierName = null;

            for (int indexBarrier = 0; indexBarrier < GameConstants.NumBarriers; indexBarrier++)
            {
                ImpassibleObject _barrier = new ImpassibleObject(this);

                    switch (randomBarrier)
                    {
                        case 0:
                            barrierName = "Models/Misc/bundle";
                            break;
                        case 1:
                            barrierName = "Models/Containers/barrel";
                            break;
                        case 2:
                            barrierName = "Models/Containers/chest1";
                            break;
                    }
                    _barrier.LoadContent(Manager.Content, barrierName);
                    randomBarrier = random.Next(3);
                    Barriers.Add(_barrier);
                }
            _status.LoadContent(Manager.Content, "Textures2D/Boxes/Status", 1.0f);
            PlaceFuelCellsAndBarriers();
            
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            player.Update(gameTime);
            float aspectRatio = Manager.Adapter.GraphicsDevice.Viewport.AspectRatio;
            gameCamera.Update(player.ForwardDirection,
                player.Position, aspectRatio);
            foreach (ImpassibleObject o in Barriers)
            {
                o.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Manager.Adapter.GraphicsDevice.Clear(Color.BurlyWood);
            SpriteBatch batch = new SpriteBatch(Manager.Adapter.GraphicsDevice);

            //batch.Begin();

            DrawTerrain(ground.Model);
            foreach (ImpassibleObject o in Barriers)
            {
                o.Draw(gameCamera.ViewMatrix,
                    gameCamera.ProjectionMatrix);
            }

            foreach (CollectableItem o in Items)
            {
                o.Draw(gameCamera.ViewMatrix,
                    gameCamera.ProjectionMatrix);
            }
            player.Draw(gameCamera.ViewMatrix,
                gameCamera.ProjectionMatrix);

            _status.Draw(Manager.Content, gameTime, new Vector2(0,0));

            //batch.End();
            base.Draw(gameTime);
        }

        #region Screen Stuff
        private void DrawTerrain(Model model)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = Matrix.Identity;

                    // Use the matrices provided by the game camera
                    effect.View = gameCamera.ViewMatrix;
                    effect.Projection = gameCamera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
        private void PlaceFuelCellsAndBarriers()
        {
            int min = GameConstants.MinDistance;
            int max = GameConstants.MaxDistance;
            Vector3 tempCenter;

            //place fuel cells
            foreach (CollectableItem i in Items)
            {
                i.Position = GenerateRandomPosition(min, max);
                tempCenter = i.BoundingSphere.Center;
                tempCenter.X = i.Position.X;
                tempCenter.Z = i.Position.Z;
                i.BoundingSphere = new BoundingSphere(tempCenter,
                    i.BoundingSphere.Radius);
                i.Retrieved = false;
            }

            //place barriers
            foreach (ImpassibleObject barrier in Barriers)
            {
                barrier.Position = GenerateRandomPosition(min, max);
                tempCenter = barrier.BoundingSphere.Center;
                tempCenter.X = barrier.Position.X;
                tempCenter.Z = barrier.Position.Z;
                barrier.BoundingSphere = new BoundingSphere(tempCenter,
                    barrier.BoundingSphere.Radius);
            }
        }

        private Vector3 GenerateRandomPosition(int min, int max)
        {
            int xValue, zValue;
            do
            {
                xValue = random.Next(min, max);
                zValue = random.Next(min, max);
                if (random.Next(100) % 2 == 0)
                    xValue *= -1;
                if (random.Next(100) % 2 == 0)
                    zValue *= -1;

            } while (IsOccupied(xValue, zValue));

            return new Vector3(xValue, 0, zValue);
        }
        private bool IsOccupied(int xValue, int zValue)
        {
            foreach (CollectableItem currentObj in Items)
            {
                if (((int)(MathHelper.Distance(
                    xValue, currentObj.Position.X)) < 15) &&
                    ((int)(MathHelper.Distance(
                    zValue, currentObj.Position.Z)) < 15))
                    return true;
            }

            foreach (ImpassibleObject currentObj in Barriers)
            {
                if (((int)(MathHelper.Distance(
                    xValue, currentObj.Position.X)) < 15) &&
                    ((int)(MathHelper.Distance(
                    zValue, currentObj.Position.Z)) < 15))
                    return true;
            }
            return false;
        }
        #endregion
    }
}
