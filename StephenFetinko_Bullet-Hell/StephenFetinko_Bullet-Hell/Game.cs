using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StephenFetinko_Bullet_Hell
{
    /// <summary>
    /// Class for the main game
    /// </summary>
    public partial class BulletHell : Form
    {
        // Variable to check if the game has started yet
        private bool hasGameStarted;

        // Make a list for the bullets for the player
        List<Bullet> playerBullets = new List<Bullet>();

        // Make a list for the enemy bullets
        List<Bullet> enemyBullets = new List<Bullet>();

        // Make lists to handle enemy waves
        List<EnemyShip> firstWave = new List<EnemyShip>();
        List<EnemyShip> secondWave = new List<EnemyShip>();
        List<EnemyShip> thirdWave = new List<EnemyShip>();
        List<EnemyShip> bossWave = new List<EnemyShip>();

        // Variables for smoother movement
        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;

        // Make a random object for enemy patterns
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        // Make an object for the player object (the ship)
        PlayerShip ship;

        // Make colors for times of the days which change based on the wave
        private Color morning = Color.FromArgb(25, 77, 147);
        private Color afternoon = Color.FromArgb(56, 118, 201);
        private Color dusk = Color.FromArgb(209, 14, 69);
        private Color night = Color.FromArgb(17, 0, 96);

        // Track the number of enemies
        private int enemiesKilled;

        // Track the wave number
        private int wave;

        // Track if we've made a wave
        private bool madeWave;
            
        // Make a ticker for the enemy
        private int timesHitEnemy;

        // Enemy shot timer
        private int enemyShotTimer;

        // Make a bool to track a game over
        private bool gameOver;

        // Make a bool to track a successful run
        private bool playerWon;

        /// <summary>
        /// Constructor for Ballistophobia. After we initialize,
        /// set the minimum and maximum size to the current size
        /// (which we set to 800x600 through the form.) to stop
        /// resizing, then disable the minimize and maximize
        /// buttons. Also set the background color to a 'morning'
        /// colour. Also centered the screen.
        /// </summary>
        public BulletHell()
        {
            InitializeComponent();
            this.BackColor = morning;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.CenterToScreen();
        }

        /// <summary>
        /// When the form loads, initialize the ship object and note
        /// that the game hasn't actually 'started' yet to give time
        /// for an intro message to be played. And also make a note
        /// that the player has killed 0 enemies on load.
        /// </summary>
        /// <param name="sender">Object(Sender)</param>
        /// <param name="e">Event(Load)</param>
        private void BulletHell_Load(object sender, EventArgs e)
        {
            // Initialize the player
            ship = new PlayerShip(this.DisplayRectangle);
            // Initialize the player bullets
            playerBullets.Add(new Bullet(this.DisplayRectangle, ship.displayArea.X, ship.displayArea.Y));
            // Initialize the first wave of enemies
            for(int i = 0; i < 5; i++)
            {
                firstWave.Add(new EnemyShip(this.DisplayRectangle, random.Next(2, 8), 1));
            }
            // Make a note that the game hasn't started and that there's been 0 enemies destroyed
            // Then note that the game starts on wave 1, and initialize the shot timer, game over
            // bool and the player won bool
            hasGameStarted = false;
            enemiesKilled = 0;
            timesHitEnemy = 0;
            wave = 1;
            madeWave = false;
            enemyShotTimer = 0;
            gameOver = false;
            playerWon = false;
        }

        /// <summary>
        /// The paint method for the main game (where we draw the ship and any messages and enemies)
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Paint</param>
        private void BulletHell_Paint(object sender, PaintEventArgs e)
        {
            if (!hasGameStarted)
                DisplayIntroMessage(e.Graphics);
            else
            {
                ship.Draw(e.Graphics);
                RemoveIntroMessage(e.Graphics);
                ShowKillCount(e.Graphics);
                foreach (Bullet bullet in playerBullets)
                {
                    bullet.Draw(e.Graphics);
                }
                foreach (Bullet enemyBullet in enemyBullets)
                {
                    enemyBullet.Draw(e.Graphics);
                }
                if(wave == 1)
                {
                    foreach (EnemyShip enemy in firstWave)
                    {
                        enemy.Draw(e.Graphics);
                    }
                }
                if (wave == 2)
                {
                    foreach (EnemyShip secondWave in secondWave)
                    {
                        secondWave.Draw(e.Graphics);
                    }
                }
                if (wave == 3)
                {
                    foreach (EnemyShip thirdWave in thirdWave)
                    {
                        thirdWave.Draw(e.Graphics);
                    }
                }
                if (wave == 4)
                {
                    foreach (EnemyShip bossWave in bossWave)
                    {
                        bossWave.Draw(e.Graphics);
                    }
                }
                if (playerWon)
                {
                    ShowWinMessage(e.Graphics);
                    GameTimer.Stop();
                }
            }
        }
        
        /// <summary>
        /// Function to display an intro message before the game begins
        /// </summary>
        /// <param name="graphics">Graphics</param>
        private void DisplayIntroMessage(Graphics graphics)
        {
            /* Text position. Since the screen is 800x600 I can do this programatically
               and play around with it until it meets my needs */
            int xPos = 110;
            int yPos = 300;

            // Text content
            string welcomeMessage = String.Format("Welcome to Ballistophobia. You've got one life. Kill all the aliens. Press Z to begin.");

            // Font style
            Font font = new Font(FontFamily.GenericSansSerif, 12);

            // Actually drawing the text
            graphics.DrawString(welcomeMessage, font, Brushes.White, xPos, yPos);
        }

        /// <summary>
        /// Remove the intro message. Draw string shouldn't take too much resources, so we
        /// should be able to just draw over it again with no text
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="timeOfDay">Text color based on time of day</param>
        private void RemoveIntroMessage(Graphics graphics)
        {
            /* Text position. Since the screen is 800x600 I can do this programatically
               and play around with it until it meets my needs */
            int xPos = 230;
            int yPos = 300;

            // Text content
            string clearedMessage = String.Format("");
            // Font style
            Font font = new Font(FontFamily.GenericSansSerif, 12);

            graphics.DrawString(clearedMessage, font, Brushes.White, xPos, yPos);
        }

        /// <summary>
        /// Handle the movement for the player object to move/shoot. If we have the
        /// movement actually handled specifically on key press we'll have an issue
        /// where there's slight delay if held down. We want to avoid that delay.
        /// Best answer on https://stackoverflow.com/questions/29942437/removing-the-delay-after-keydown-event
        /// defines using the key down event to set a movement direction instead
        /// of actually moving
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Key pressed</param>
        private void BulletHell_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                    {
                        if(hasGameStarted)
                        {
                            moveUp = true;
                        }
                        break;
                    }
                case Keys.Down:
                    {
                        if (hasGameStarted)
                        {
                            moveDown = true;
                        }
                        break;
                    }
                case Keys.Left:
                    {
                        if (hasGameStarted)
                        {
                            moveLeft = true;
                        }
                        break;
                    }
                case Keys.Right:
                    {
                        if (hasGameStarted)
                        {
                            moveRight = true;
                        }
                        break;
                    }
                case Keys.Z:
                    {
                        if (!hasGameStarted)
                        {
                            hasGameStarted = true;
                            GameTimer.Start();
                        }
                        break;
                    }
            }
            MovementTimer.Enabled = true;
        }

        /// <summary>
        /// Key up event for smoother player movement. Changes the values
        /// of bools when the key is up to avoid the player moving forever.
        /// To avoid shooting issues have the shot fire on the key up as
        /// well to avoid any problems that happens from the key down
        /// event
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Keyboard input</param>
        private void BulletHell_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        if (hasGameStarted)
                        {
                            moveUp = false;
                        }
                        break;
                    }
                case Keys.Down:
                    {
                        if (hasGameStarted)
                        {
                            moveDown = false;
                        }
                        break;
                    }
                case Keys.Left:
                    {
                        if (hasGameStarted)
                        {
                            moveLeft = false;
                        }
                        break;
                    }
                case Keys.Right:
                    {
                        if (hasGameStarted)
                        {
                            moveRight = false;
                        }
                        break;
                    }
                case Keys.Z:
                    {
                        playerBullets.Add(new Bullet(this.DisplayRectangle, ship.displayArea.X, ship.displayArea.Y));
                        break;
                    }
                case Keys.Space:
                    {
                        if(playerWon)
                            Application.Restart();
                        break;
                    }
            }
        }
        
        /// <summary>
        /// Function to show off how many enemies are killed
        /// </summary>
        /// <param name="graphics">Graphics</param>
        private void ShowKillCount(Graphics graphics)
        {
            string message = String.Format("Kills: {0}", enemiesKilled);

            Font font = new Font(FontFamily.GenericSansSerif, 13);

            graphics.DrawString(message, font, Brushes.White, 20, 20);
        }

        /// <summary>
        /// Every tick of the in game timer we fire a shot, then have a for
        /// loop that iterates every time the clock ticks if there are more
        /// than 15 objects in the player bullets list then we continue to
        /// remove the 0 element so at most there are 15 bullets from the player
        /// we keep it at 15 as the limit so there's enough on screen when
        /// they are removed. Also handle the game over, killing enemies,
        /// enemies killing players, the enemies shot timer, and handling
        /// waves of enemies and then ultimately refreshing the screen
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Timer tick</param>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (gameOver)
                Application.Restart();
            HandleShootingEnemy(wave);

            // Mpve whichever wave is currently active
            HandleMovement(wave);

            foreach (Bullet bullet in playerBullets)
            {
                bullet.Shoot();
            }
            foreach (Bullet enemyBullet in enemyBullets)
            {
                enemyBullet.EnemyShooting(wave);
            }
            if(playerBullets.Count > 15)
            {
                for (int i = 0; i < playerBullets.Count; i++)
                {
                    if (playerBullets[i].displayArea.Y >= DisplayRectangle.Y)
                        playerBullets.RemoveAt(0);
                }
            }
            HandleEnemiesShooting(wave);
            HandleWaves(wave);
            HandleMakingWave(wave);
            enemyShotTimer++;
            Invalidate();
        }

        /// <summary>
        /// Function to show a win message if the player gets this far
        /// </summary>
        /// <param name="graphics"></param>
        private void ShowWinMessage(Graphics graphics)
        {
            /* Text position. Since the screen is 800x600 I can do this programatically
               and play around with it until it meets my needs */
            int xPos = 140;
            int yPos = 300;

            // Text content
            string welcomeMessage = String.Format("You've stopped the alien onslaught! Press Space to do it all again!");

            // Font style
            Font font = new Font(FontFamily.GenericSansSerif, 12);

            // Actually drawing the text
            graphics.DrawString(welcomeMessage, font, Brushes.White, xPos, yPos);
        }
        /// <summary>
        /// Movement timer tick event. Handles the actual moving for the
        /// moving. Done in a seperate timer to handle specific movement
        /// in case anything happens in the game timer where the precision
        /// of interval can be finicky sometimes. This acts as a failsafe.
        /// ALSO this is because I pause this timer whenever the player
        /// isn't moving, and if the actual game pauses whenever the player
        /// does that's an issue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovementTimer_Tick(object sender, EventArgs e)
        {
            bool stopTimer = true;
            if (moveUp == true)
            {
                ship.MovePlayer(PlayerShip.Direction.UP);
                stopTimer = false;
            }
            if (moveDown == true)
            {
                ship.MovePlayer(PlayerShip.Direction.DOWN);
                stopTimer = false;
            }
            if (moveLeft == true)
            {
                ship.MovePlayer(PlayerShip.Direction.LEFT);
                stopTimer = false;
            }
            if (moveRight == true)
            {
                ship.MovePlayer(PlayerShip.Direction.RIGHT);
                stopTimer = false;
            }
            if (stopTimer)
                MovementTimer.Enabled = false;
        }

        /// <summary>
        /// Random function from https://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number
        /// This is used for sparadic bug like movements of the enemy ships
        /// </summary>
        /// <param name="min">Start of range</param>
        /// <param name="max">End of range</param>
        /// <returns></returns>
        private static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        /// <summary>
        /// Function to handle enemy movement depending on the wave
        /// </summary>
        /// <param name="waveNumber">What wave the player is on</param>
        private void HandleMovement(int waveNumber)
        {
            if(waveNumber == 1)
            {
                foreach (EnemyShip enemy in firstWave)
                {
                    if (enemy.displayArea.Y < enemy.midPointY1)
                    {
                        int randomDirection = RandomNumber(1, 10);

                        if (randomDirection == 1 || randomDirection == 3 || randomDirection == 7)
                            enemy.MoveEnemy(EnemyShip.Direction.DOWN);
                        else if (randomDirection == 2 || randomDirection == 4 || randomDirection == 6)
                            enemy.MoveEnemy(EnemyShip.Direction.LEFT);
                        else
                            enemy.MoveEnemy(EnemyShip.Direction.RIGHT);
                    }
                    if (enemy.displayArea.Y > enemy.midPointY2)
                    {
                        int randomDirection = RandomNumber(1, 10);

                        if (randomDirection == 1 || randomDirection == 3 || randomDirection == 7)
                            enemy.MoveEnemy(EnemyShip.Direction.UP);
                        else if (randomDirection == 2 || randomDirection == 4 || randomDirection == 6)
                            enemy.MoveEnemy(EnemyShip.Direction.LEFT);
                        else
                            enemy.MoveEnemy(EnemyShip.Direction.RIGHT);
                    }
                }
            }
            if(waveNumber == 2)
            {
                foreach (EnemyShip secondEnemy in secondWave)
                {
                    if (secondEnemy.displayArea.Y < secondEnemy.midPointY1)
                    {
                        int randomDirection = RandomNumber(1, 10);

                        if (randomDirection == 1 || randomDirection == 3 || randomDirection == 7)
                            secondEnemy.MoveEnemy(EnemyShip.Direction.DOWN);
                        else if (randomDirection == 2 || randomDirection == 4 || randomDirection == 6)
                            secondEnemy.MoveEnemy(EnemyShip.Direction.LEFT);
                        else
                            secondEnemy.MoveEnemy(EnemyShip.Direction.RIGHT);
                    }
                    if (secondEnemy.displayArea.Y > secondEnemy.midPointY2)
                    {
                        int randomDirection = RandomNumber(1, 10);

                        if (randomDirection == 1 || randomDirection == 3 || randomDirection == 7)
                            secondEnemy.MoveEnemy(EnemyShip.Direction.UP);
                        else if (randomDirection == 2 || randomDirection == 4 || randomDirection == 6)
                            secondEnemy.MoveEnemy(EnemyShip.Direction.LEFT);
                        else
                            secondEnemy.MoveEnemy(EnemyShip.Direction.RIGHT);
                    }
                }
            }
            if (waveNumber == 3)
            {
                foreach (EnemyShip enemy in thirdWave)
                {
                    if (enemy.displayArea.Y < enemy.midPointY1)
                    {
                        int randomDirection = RandomNumber(1, 10);

                        if (randomDirection == 1 || randomDirection == 3 || randomDirection == 7)
                            enemy.MoveEnemy(EnemyShip.Direction.DOWN);
                        else if (randomDirection == 2 || randomDirection == 4 || randomDirection == 6)
                            enemy.MoveEnemy(EnemyShip.Direction.LEFT);
                        else
                            enemy.MoveEnemy(EnemyShip.Direction.RIGHT);
                    }
                    if (enemy.displayArea.Y > enemy.midPointY2)
                    {
                        int randomDirection = RandomNumber(1, 10);

                        if (randomDirection == 1 || randomDirection == 3 || randomDirection == 7)
                            enemy.MoveEnemy(EnemyShip.Direction.UP);
                        else if (randomDirection == 2 || randomDirection == 4 || randomDirection == 6)
                            enemy.MoveEnemy(EnemyShip.Direction.LEFT);
                        else
                            enemy.MoveEnemy(EnemyShip.Direction.RIGHT);
                    }
                }
            }
            if (waveNumber == 4)
            {
                foreach (EnemyShip enemy in bossWave)
                {
                    if (enemy.displayArea.Y < enemy.midPointY1)
                    {
                        int randomDirection = RandomNumber(1, 10);

                        if (randomDirection == 1 || randomDirection == 3 || randomDirection == 7)
                            enemy.MoveEnemy(EnemyShip.Direction.DOWN);
                        else if (randomDirection == 2 || randomDirection == 4 || randomDirection == 6)
                            enemy.MoveEnemy(EnemyShip.Direction.LEFT);
                        else
                            enemy.MoveEnemy(EnemyShip.Direction.RIGHT);
                    }
                    if (enemy.displayArea.Y > enemy.midPointY2)
                    {
                        int randomDirection = RandomNumber(1, 10);

                        if (randomDirection == 1 || randomDirection == 3 || randomDirection == 7)
                            enemy.MoveEnemy(EnemyShip.Direction.UP);
                        else if (randomDirection == 2 || randomDirection == 4 || randomDirection == 6)
                            enemy.MoveEnemy(EnemyShip.Direction.LEFT);
                        else
                            enemy.MoveEnemy(EnemyShip.Direction.RIGHT);
                    }
                }
            }
        }

        /// <summary>
        /// Function to handle the enemy being shot
        /// </summary>
        /// <param name="waveNumber">What wave the player is on</param>
        private void HandleShootingEnemy(int waveNumber)
        {
            if(waveNumber == 1)
            {
                foreach (Bullet bullet in playerBullets.ToList())
                {
                    foreach (EnemyShip enemy in firstWave.ToList())
                    {
                        if (bullet.displayArea.IntersectsWith(enemy.displayArea))
                        {
                            if (enemy.health > 0)
                            {
                                enemy.health--;
                                timesHitEnemy++;
                            }
                            if (enemy.health == 0)
                            {
                                firstWave.Remove(enemy);
                                enemiesKilled++;
                            }
                        }
                    }
                }
            }
            if (waveNumber == 2)
            {
                for (int x = 0; x < playerBullets.Count; x++)
                {
                    for (int o = 0; o < secondWave.Count; o++)
                    {
                        if (playerBullets[x].displayArea.IntersectsWith(secondWave[o].displayArea))
                        {
                            if (secondWave[o].health > 0)
                            {
                                secondWave[o].health--;
                                timesHitEnemy++;
                            }
                            if (secondWave[o].health == 0)
                            {
                                secondWave.Remove(secondWave[o]);
                                enemiesKilled++;
                            }
                        }
                    }
                }
            }
            if (waveNumber == 3)
            {
                foreach (Bullet bullet in playerBullets.ToList())
                {
                    foreach (EnemyShip enemy in thirdWave.ToList())
                    {
                        if (bullet.displayArea.IntersectsWith(enemy.displayArea))
                        {
                            if (enemy.health > 0)
                            {
                                enemy.health--;
                                timesHitEnemy++;
                            }
                            if (enemy.health == 0)
                            {
                                thirdWave.Remove(enemy);
                                enemiesKilled++;
                            }
                        }
                    }
                }
            }
            if (waveNumber == 4)
            {
                foreach (Bullet bullet in playerBullets.ToList())
                {
                    foreach (EnemyShip enemy in bossWave.ToList())
                    {
                        if (bullet.displayArea.IntersectsWith(enemy.displayArea))
                        {
                            if (enemy.health > 0)
                            {
                                enemy.health--;
                                timesHitEnemy++;
                            }
                            if (enemy.health == 0)
                            {
                                bossWave.Remove(enemy);
                                enemiesKilled++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Function to handle which wave the player is on
        /// </summary>
        /// <param name="waveNumber">What wave the player is on (is changed during function)</param>
        private void HandleWaves(int waveNumber)
        {
            if (waveNumber == 1)
            {
                if (firstWave.Count == 0)
                {
                    wave++;
                    madeWave = false;
                    this.BackColor = afternoon;
                    
                }
            }
            if (waveNumber == 2)
            {
                if (secondWave.Count == 0)
                {
                    wave++;
                    madeWave = false;
                    this.BackColor = dusk;
                }
            }
            if(waveNumber == 3)
            {
                if (thirdWave.Count == 0)
                {
                    wave++;
                    madeWave = false;
                    this.BackColor = night;
                }
            }
            if(waveNumber == 4)
            {
                if (bossWave.Count == 0)
                {
                    playerWon = true;
                }
            }
        }

        /// <summary>
        /// Function to handle creating the wave
        /// </summary>
        /// <param name="waveNumber">What wave the player is on</param>
        private void HandleMakingWave(int waveNumber)
        {
            if(waveNumber == 2 && !madeWave)
            {
                firstWave.Clear();
                for (int i = 0; i < 10; i++)
                {
                    secondWave.Add(new EnemyShip(this.DisplayRectangle, random.Next(2, 8), 2));
                }
                playerBullets.Clear();
                madeWave = true;
            }
            if (waveNumber == 3 && !madeWave)
            {
                secondWave.Clear();
                for (int i = 0; i < 15; i++)
                {
                    thirdWave.Add(new EnemyShip(this.DisplayRectangle, random.Next(2, 8), 3));
                }
                playerBullets.Clear();
                madeWave = true;
            }
            if (waveNumber == 4 && !madeWave)
            {
                thirdWave.Clear();
                for (int i = 0; i < 3; i++)
                {
                    bossWave.Add(new EnemyShip(this.DisplayRectangle, random.Next(2, 8), 4));
                }
                playerBullets.Clear();
                madeWave = true;
            }
        }

        /// <summary>
        /// Handle the enemies shots (the rate of fire and handling the player getting hit
        /// </summary>
        /// <param name="waveNumber">What wave the player is on</param>
        private void HandleEnemiesShooting(int waveNumber)
        {
            if (waveNumber == 1)
            {
                if(enemyShotTimer > 100)
                {
                    enemyShotTimer = 0;
                    foreach(EnemyShip enemies in firstWave)
                    {
                        enemyBullets.Add(new Bullet(this.DisplayRectangle, enemies.displayArea.X, enemies.displayArea.Y));
                    }
                }
                foreach (Bullet enemyBullet in enemyBullets.ToList())
                {
                    if(enemyBullet.displayArea.IntersectsWith(ship.displayArea))
                    {
                        gameOver = true;
                    }
                }
            }
            if (waveNumber == 2)
            {
                if (enemyShotTimer > 120)
                {
                    enemyShotTimer = 0;
                    foreach (EnemyShip enemies in secondWave)
                    {
                        enemyBullets.Add(new Bullet(this.DisplayRectangle, enemies.displayArea.X, enemies.displayArea.Y));
                    }
                }
                foreach (Bullet enemyBullet in enemyBullets.ToList())
                {
                    if (enemyBullet.displayArea.IntersectsWith(ship.displayArea))
                    {
                        gameOver = true;
                    }
                }
            }
            if (waveNumber == 3)
            {
                if (enemyShotTimer > 150)
                {
                    enemyShotTimer = 0;
                    foreach (EnemyShip enemies in thirdWave)
                    {
                        enemyBullets.Add(new Bullet(this.DisplayRectangle, enemies.displayArea.X, enemies.displayArea.Y));
                    }
                }
                foreach (Bullet enemyBullet in enemyBullets.ToList())
                {
                    if (enemyBullet.displayArea.IntersectsWith(ship.displayArea))
                    {
                        gameOver = true;
                    }
                }
            }
            if (waveNumber == 4)
            {
                if (enemyShotTimer > 60)
                {
                    enemyShotTimer = 0;
                    foreach (EnemyShip enemies in bossWave)
                    {
                        enemyBullets.Add(new Bullet(this.DisplayRectangle, enemies.displayArea.X, enemies.displayArea.Y));
                    }
                }
                foreach (Bullet enemyBullet in enemyBullets.ToList())
                {
                    if (enemyBullet.displayArea.IntersectsWith(ship.displayArea))
                    {
                        gameOver = true;
                    }
                }
            }
        }
    }
}
