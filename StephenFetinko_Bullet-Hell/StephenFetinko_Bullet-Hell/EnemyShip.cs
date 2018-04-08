using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StephenFetinko_Bullet_Hell
{
    /// <summary>
    /// Class for the enemy.
    /// Regular enemy https://opengameart.org/content/purple-space-ship
    /// Boss https://postimg.org/image/cyf7qssc9/
    /// </summary>
    class EnemyShip
    {
        // Objects
        Image image;

        // Define the play area
        public Rectangle displayArea;
        private Rectangle canvas;

        // Movement speed for X and Y
        private int XVelocity;
        private int YVelocity;

        // Variables to help with randomizing the position. midPointY1 and midPointY2 are meant to act as a range
        public int midPointX;
        public int midPointY1;
        public int midPointY2;
        public int topPoint;

        // Health for the enemy
        public int health;

        // Make the hitbox
        private int size = 50;

        // Make an enum for the directions
        public enum Direction { UP, DOWN, LEFT, RIGHT };

        /// <summary>
        /// Constructor for the enemy
        /// </summary>
        /// <param name="canvas">Play area</param>
        public EnemyShip(Rectangle canvas, int startX, int wave)
        {
            // Set the speed of the enemies based on the wave
            if (wave == 1)
            {
                XVelocity = 8;
                YVelocity = 8;
                health = 15;
                // Set the size of the canvas
                displayArea.Height = size;
                displayArea.Width = size;
                image = Image.FromFile("C:\\Users\\stephen\\Source\\Repos\\Fetinko-Stephen-w0297109\\StephenFetinko_Bullet-Hell\\StephenFetinko_Bullet-Hell\\assets\\DurrrSpaceShip.png");
            }
            else if (wave == 2)
            {
                XVelocity = 10;
                YVelocity = 10;
                health = 20;
                // Set the size of the canvas
                displayArea.Height = size;
                displayArea.Width = size;
                image = Image.FromFile("C:\\Users\\stephen\\Source\\Repos\\Fetinko-Stephen-w0297109\\StephenFetinko_Bullet-Hell\\StephenFetinko_Bullet-Hell\\assets\\DurrrSpaceShip.png");
            }
            else if (wave == 3)
            {
                XVelocity = 12;
                YVelocity = 12;
                health = 25;
                displayArea.Height = size;
                displayArea.Width = size;
                image = Image.FromFile("C:\\Users\\stephen\\Source\\Repos\\Fetinko-Stephen-w0297109\\StephenFetinko_Bullet-Hell\\StephenFetinko_Bullet-Hell\\assets\\DurrrSpaceShip.png");
            }
            else if (wave == 4)
            {
                XVelocity = 14;
                YVelocity = 14;
                health = 100;
                displayArea.Height = 130;
                displayArea.Width = 130;
                image = Image.FromFile("C:\\Users\\stephen\\Source\\Repos\\Fetinko-Stephen-w0297109\\StephenFetinko_Bullet-Hell\\StephenFetinko_Bullet-Hell\\assets\\VS_demilich.png");
            }

            // Set the location to the top of the screen
            displayArea.X = (canvas.Width / 2) - (int)(displayArea.Width * startX);
            displayArea.Y = canvas.Top - (int)(canvas.Height * 0.2);

            this.canvas = canvas;

            // Image of the enemy goes here

            midPointX = (canvas.Width / 2) - (int)(displayArea.Width / 2);
            midPointY1 = (canvas.Height / 2) - (int)(displayArea.Height / 2);
            midPointY2 = (canvas.Height / 2) - (int)(displayArea.Height / 2) - 80;

            topPoint = canvas.Top;
        }

        /// <summary>
        /// Class to handle the enemy moving. Done similar to the player
        /// class minus the up variable so the enemy can come from somewhere
        /// above (the idea is randomly) and then they will hand around the
        /// upper portion of the play area and move around shooting down
        /// </summary>
        /// <param name="direction">Randomly chosen direction of movement</param>
        public void MoveEnemy(Direction direction)
        {
            switch (direction)
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
        /// Class to draw the enemy icon
        /// </summary>
        /// <param name="graphics">Image of the enemy</param>
        public void Draw(Graphics graphics)
        {
            // Draw the enemy here
            graphics.DrawImage(image, displayArea);
        }
    }
}
