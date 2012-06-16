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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Varpal.Base
{
    // TODO: Fix this right, add gamepads and keyboard full support
    public class Input
    {
  /* Declare all the Vars we will need, we are going to be using get/set
   * This is a way different way to handle vars, but just as useful
   * and looks like you know what your doing, even though i dont ;p
   */
        // MouseStates
        MouseState previousMouseState, currentMouseState;
        KeyboardState oldKeyboardState, currentKeyboardState;

        Texture2D texture;
        public bool leftIsHeld;
        public bool clickedonce;

        // The mouses X Y
        //Private Vector2 used within this class, to set the value of a Public Vector2
        private Vector2 m_position;

        /// <summary>
        /// Returns or sets the Mouse X,Y
        /// </summary>
        public Vector2 Position
        {
            get { return m_position; } // When we ask for this value its going to give us the value of m_position
            set { m_position = value; } // When we set this value, its sets the value of m_position
        }


        public Point MousePoint { get; private set; }

        // Private Rectangle used to set the value of our Public Rectangle
        private Rectangle m_rectangle;

        /// <summary>
        /// Returns or sets the value of our Mouse Rectangle
        /// </summary>
        public Rectangle Rectangle
        {
            get { return m_rectangle; }
            set { m_rectangle = value; }
        }
        // Way to Check for Clicks

        /// <summary>
        /// Returns true if Mouse is LeftClicked
        /// </summary>
        public bool LeftClick
        {
            get { return currentMouseState.LeftButton == ButtonState.Pressed; }
            // We dont use a set here, as we only want it to return the state, not to forcefully set it.
        }

        /// <summary>
        /// Returns true if Mouse is RightClicked
        /// </summary>
        public bool RightClick
        {
            get { return currentMouseState.RightButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Returns true if the mouse was LeftClicked for the first time
        /// </summary>
        public bool NewLeftClick
        {
            get
            {
                return currentMouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released;
            }
        }

        /// <summary>
        /// Returns true if the Mouse was RightClicked for the first time
        /// </summary>
        public bool NewRightClick
        {
            get
            {
                return currentMouseState.RightButton == ButtonState.Pressed &&
                    previousMouseState.RightButton == ButtonState.Released;
            }
        }

        /// <summary>
        /// Returns true if the Mouse LeftButton was pressed, but now is not
        /// </summary>
        public bool LeftRelease
        {
            get { return !LeftClick && previousMouseState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Returns true if the Mouse RightButton was pressed, but now is not
        /// </summary>
        public bool RightRelease
        {
            get { return !RightClick && previousMouseState.RightButton == ButtonState.Pressed; }
        }

        public bool NormalState
        {
            get { return !LeftClick && !LeftRelease && !RightClick && !RightRelease; }
        }


        // Blank Constructor
        /// <summary>
        /// Empty Mouse Object
        /// </summary>
        public Input()
        {
        }
        /// <summary>
        /// Mouse Object with the icon we want
        /// </summary>
        /// <param name="texture"></param>
        public Input(Texture2D texture)
        {
            this.texture = texture;
            this.Rectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, 5, 5);
        }
        // This is the end of our constructors
        /// <summary>
        /// Gets the current mousestate, and changed mouse Position and the Rectangle its using each frame
        /// </summary>
        public void Update(GameTime gameTime)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            Position = new Vector2(currentMouseState.X, currentMouseState.Y);

            this.Rectangle = new Rectangle(currentMouseState.X, currentMouseState.Y, 5, 5);
            MousePoint = new Point(currentMouseState.X, currentMouseState.Y);

            if (clickedonce && 100 < gameTime.TotalGameTime.Milliseconds)
            {
                clickedonce = false;
            }

            if (leftIsHeld == true)
            {
                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    leftIsHeld = false;
                }
            }

        }

        // Keyboard

        public bool IsEscapePressed
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Escape);
            }
        }

        // Keyboard keys
        /// <summary>
        /// Up Arrow
        /// </summary>
        public bool IsUpArrowPressed
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Up);
            }
        }

        /// <summary>
        /// Down Arrow
        /// </summary>
        public bool IsDownArrowPressed
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Down);
            }
        }

        /// <summary>
        /// left Arrow
        /// </summary>
        public bool IsLeftArrowPressed
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Left);
            }
        }

        /// <summary>
        /// right Arrow
        /// </summary>
        public bool IsRightArrowPressed
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Right);
            }
        }

        /// <summary>
        /// Draws our Mouse object to the screen
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw this object</param>
        public void Draw(SpriteBatch spriteBatch)
        {

        }

        #region Left Held
        /// <summary>
        /// Use this to determine if the left button is being held
        /// The tricky part is figuring out the difference 
        /// between a drag and a long click.
        /// 
        /// What we're doing is testing if we already determined
        /// if it's already being held, or if it's being held and 
        /// it's been moved.
        /// Else we assume it's a long click.
        /// </summary>
        /// <returns>If the button is being held</returns>
        public bool LeftHeld()
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Pressed
                && leftIsHeld == true)
            {
                return true;
            }
            else if ((currentMouseState.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Pressed) &&
                (currentMouseState.X != previousMouseState.X || currentMouseState.Y != previousMouseState.Y))
            {
                leftIsHeld = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Use this to determine if the left button was
        /// double clicked
        /// </summary>
        /// <returns>If the left button was double clicked</returns>
        public bool DoubleClick()
        {
            if (clickedonce)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Released)
                {
                    clickedonce = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
