using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StephenFetinko_Bullet_Hell
{
    /// <summary>
    /// Class for the bullets being shot
    /// </summary>
    class Bullet
    {
        // Objects for the play area
        public Rectangle displayArea;
        private Rectangle canvas;

        // Rate of fire
        //private int XVelocity; <- This can be used for powerups. Ran out of time.
        private int YVelocity = 10;

        // Colour of the bullet
        Color color;

        // Size of the bullet
        private readonly int size = 10;

        /// <summary>
        /// Constructor for the bullet
        /// </summary>
        /// <param name="canvas">Game area</param>
        /// <param name="startX">Start X position based on player</param>
        /// <param name="startY">Start Y position based on player</param>
        public Bullet(Rectangle canvas, int startX, int startY)
        {
            // Initialize the play area
            displayArea.Height = size;
            displayArea.Width = size;

            this.canvas = canvas;
            //place the ball in the center of the screen
            displayArea.X = startX + 20;
            displayArea.Y = startY;
            // Set the color of the bullets

        }

        /// <summary>
        /// Function to handle the player actually shooting
        /// </summary>
        public void Shoot()
        {
            color = Color.FromArgb(255, 255, 255, 255);
            displayArea.Y -= YVelocity;
        }

        /// <summary>
        /// Function to handle the enemy actually shooting
        /// </summary>
        /// <param name="wave">What wave we're on. Used for determining bullet colour</param>
        public void EnemyShooting(int wave)
        {
            if (wave == 4)
                color = Color.FromArgb(255, 238, 0);
            else
            {
                color = Color.FromArgb(0, 0, 0);
            }
            
            displayArea.Y += YVelocity;
        }
        /// <summary>
        /// Handle drawing the bullet
        /// </summary>
        /// <param name="graphics">Graphics</param>
        public void Draw(Graphics graphics)
        {
            SolidBrush brush = new SolidBrush(color);
            graphics.FillEllipse(brush, displayArea);
        }
    }
}
