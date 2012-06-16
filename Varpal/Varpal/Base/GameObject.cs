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
using Microsoft.Xna.Framework.Audio;
using Varpal.Managers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Varpal.Base
{
    /// <summary>
    /// Base for all objects in our game
    /// </summary>
    public class GameObject : MarshalByRefObject, IDisposable
    {
        /// <summary>
        /// The GameScreen Drawing us
        /// </summary>
        public GameScreen Screen { get; set; }

        public GameObject(GameScreen screen)
        {
            Screen = screen;
        }

        // All objects can have sounds
        // Sound effects
        #region Sound Effects
        /// <summary>
        /// Mouse Hovering Sound Effect
        /// </summary>
        SoundEffect _hoverSoundEffect;
        public SoundEffect HoverSoundEffect
        {
            get { return _hoverSoundEffect; }
            set { _hoverSoundEffect = value; }
        }

        /// <summary>
        /// If the hover Sound Effect was played, this will be true.
        /// </summary>
        public bool _hoverPlayed = false;

        /// <summary>
        /// Sound to play when object is selected
        /// </summary>
        SoundEffect _selectedSoundEffect;
        public SoundEffect SelectedSoundEffect
        {
            get { return _selectedSoundEffect; }
            set { _selectedSoundEffect = value; }
        }

        /// <summary>
        /// The instance used to play and control the sounds
        /// </summary>
        SoundEffectInstance _soundEffectInstance;
        public SoundEffectInstance SoundEffectInstance
        {
            get { return _soundEffectInstance; }
            set { _soundEffectInstance = value; }
        }
        #endregion

        public virtual void LoadContent(ContentManager content, string modelName)
        {
        }
        public virtual void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, string modelName, float scale)
        {
        }
        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(Matrix view, Matrix projection)
        {
        }

        public virtual void Draw(ContentManager content, GameTime gameTime, Vector2 position)
        {
        }
        public void Dispose()
        {
        }
    }
}
