namespace StephenFetinko_Bullet_Hell
{
    partial class BulletHell
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BulletHell));
            this.GameTimer = new System.Windows.Forms.Timer(this.components);
            this.MovementTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // GameTimer
            // 
            this.GameTimer.Interval = 1;
            this.GameTimer.Tick += new System.EventHandler(this.GameTimer_Tick);
            // 
            // MovementTimer
            // 
            this.MovementTimer.Interval = 1;
            this.MovementTimer.Tick += new System.EventHandler(this.MovementTimer_Tick);
            // 
            // BulletHell
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BulletHell";
            this.Load += new System.EventHandler(this.BulletHell_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BulletHell_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BulletHell_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BulletHell_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Timer GameTimer;
        private System.Windows.Forms.Timer MovementTimer;
    }
}

