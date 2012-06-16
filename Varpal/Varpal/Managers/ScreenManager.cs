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
using Varpal.Base;
using Microsoft.Xna.Framework.Content;

namespace Varpal.Managers
{
    /// <summary>
    /// A centralized way to control all gamescreens 
    /// we draw, we will use a dictionary so we
    /// can name our screens as we make them.
    /// </summary>
    public class ScreenManager
    {
        /// <summary>
        /// The dictionaries to hold our GameScreens
        /// the .Key value (string) is the name we give the screen.
        /// We cannot update our Screens while we are using it, so
        /// we make a second dictionary ToUpdate, and when we need
        /// to work with our collection, we just copy it back and forth.
        /// </summary>
        private Dictionary<string, GameScreen> Screens = new Dictionary<string, GameScreen>();
        private Dictionary<string, GameScreen> ScreensToUpdate = new Dictionary<string, GameScreen>();

        #region Properties
        /// <summary>
        /// Our GraphicsAdapter we need this to be able to change
        /// the resolution and screen size this is set in Gam1.cs
        /// </summary>
        public GraphicsDeviceManager Adapter { get; private set; }

        /// <summary>
        /// Our Mouse input class, shared here 
        /// for all screens to use.
        /// </summary>
        public Input Controls { get; private set; }

        /// <summary>
        /// Shared ContentManager
        /// </summary>
        public ContentManager Content { get; set; }

        /// <summary>
        /// Our Base Game
        /// </summary>
        public Game MainGame { get; set; }

        #endregion

        public ScreenManager(Game game, ContentManager content, GraphicsDeviceManager graphics)
        {
            MainGame = game;
            Content = content;
            Adapter = graphics;
            Controls = new Input();
            Adapter.PreferredBackBufferWidth = 1280;
            Adapter.PreferredBackBufferHeight = 720;
            Initialize();
        }

        public virtual void Initialize()
        {
            Adapter.ApplyChanges();
        }

        public void Update(GameTime gameTime)
        {
            // First clear our update collection, make it anew
            ScreensToUpdate.Clear();
            // Now copy all the current screens we have over
            foreach (KeyValuePair<string, GameScreen> screen in Screens)
            {
                ScreensToUpdate.Add(screen.Key, screen.Value);
            }

            // So now we filter through all the screens
            while (ScreensToUpdate.Count > 0)
            {
                // Get the topmost screen off the waiting list.
                string screenName = ScreensToUpdate.Keys.ElementAt(0);
                GameScreen screen = ScreensToUpdate.Values.ElementAt(0);

                // Remove this screen from our list.
                ScreensToUpdate.Remove(screenName);

                // Update the screen
                screen.Update(gameTime);
            }
            // Lastly Update our controls
            Controls.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (KeyValuePair<string, GameScreen> screen in Screens)
            {
                // Call the screens Draw
                screen.Value.Draw(gameTime);
            }
        }

        #region Screen Controls
        /// <summary>
        /// Add a screen under our control
        /// </summary>
        /// <param name="name">The friendly name of the screen</param>
        /// <param name="screen">The GameScreen</param>
        public void AddScreen(string name, GameScreen screen)
        {
            // Dont add a screen duplicate
            if (Screens.ContainsKey(name)) { return; }
            // Add the screen
            Screens.Add(name, screen);
        }

        /// <summary>
        /// Removes a screen from our control
        /// </summary>
        /// <param name="name">The friendly name of the screen</param>
        /// <param name="screen">The GameScreen</param>
        public void RemoveScreen(string name, GameScreen screen)
        {
            // Dont try and remove it if its not there
            if (!Screens.ContainsKey(name)) { return; }
            // Remove the screen
            Screens.Remove(name);
        }
        #endregion
    }
}
