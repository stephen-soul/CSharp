using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StephenFetinko_Bullet_Hell
{
    /// <summary>
    /// Class for the player controlled object - the ship
    /// Player ship found here http://www.mattkeeter.com/projects/pixelsim/
    /// </summary>
    class PlayerShip
    {
        // Objects
        Image image;

        // Define the play area
        public Rectangle displayArea;
        private Rectangle canvas;

        // Movement speed for X and Y
        private int XVelocity = 8;
        private int YVelocity = 8;

        // Make the hitbox
        private readonly int size = 50;

        // Make an enum for the directions
        public enum Direction { UP, DOWN, LEFT, RIGHT };

        /// <summary>
        /// Constructor for the player object (The ship)
        /// </summary>
        /// <param name="canvas"></param>
        public PlayerShip(Rectangle canvas)
        {
            // Set the size of the canvas
            displayArea.Height = size;
            displayArea.Width = size;

            // Set the location to the bottom center of the screen
            displayArea.X = (canvas.Width / 2) - (int)(displayArea.Width / 2);
            displayArea.Y = canvas.Bottom - (int)(canvas.Height * 0.2);

            this.canvas = canvas;

            //image = Image.FromFile("C:\\Users\\Stephen\\source\\repos\\Fetinko-Stephen-w0297109\\Fetinko-Stephen-w0297109\\StephenFetinko_Bullet-Hell\\StephenFetinko_Bullet-Hell\\assets\\PlayerShip.PNG");

            image = Image.FromFile("C:\\Users\\stephen\\source\\repos\\Fetinko-Stephen-w0297109\\StephenFetinko_Bullet-Hell\\StephenFetinko_Bullet-Hell\\assets\\PlayerShip.PNG");
        }
        
        /// <summary>
        /// Handle player movement in up, down, left and right directions
        /// </summary>
        /// <param name="direction"></param>
        public void MovePlayer(Direction direction)
        {
            switch(direction)
            {
                case Direction.UP:
                    {
                        displayArea.Y
                            = (displayArea.Y <= YVelocity)
                            ? canvas.Top : displayArea.Y - YVelocity;
                        break;
                    }
                case Direction.DOWN:
                    {
                        int maxValue = canvas.Bottom - (int)(canvas.Height * 0.1);

                        if (displayArea.Y >= maxValue)
                        {
                            displayArea.Y = maxValue;
                        }
                        else
                        {
                            displayArea.Y += YVelocity;
                        }
                        break;
                    }
                case Direction.LEFT:
                    {
                        displayArea.X
                            = (displayArea.X <= XVelocity)
                            ? canvas.Left : displayArea.X - XVelocity;
                        break;
                    }
                case Direction.RIGHT:
                    {
                        int maxValue = canvas.Width - displayArea.Width;

                        if (displayArea.X >= maxValue)
                        {
                            displayArea.X = maxValue;
                        }
                        else
                        {
                            displayArea.X += XVelocity;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Draw the player object (The ship)
        /// </summary>
        /// <param name="graphics"></param>
        public void Draw(Graphics graphics)
        {
            graphics.DrawImage(image, displayArea);
        }
    }
}
