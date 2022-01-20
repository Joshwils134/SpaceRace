using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        Rectangle player1 = new Rectangle(120, 300, 20, 20);
        Rectangle player2 = new Rectangle(420, 300, 20, 20);
        Rectangle divider = new Rectangle(280, 0, 10, 900);
        Rectangle winRect = new Rectangle(0, 0, 900, 10);
        Random randGen = new Random();


        int player1Speed = 3;
        int player2Speed = 3;
        int player1Score = 0;
        int player2Score = 0;
        int leftLines = 20;
        int rightLines = 20;


        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        List<Rectangle> leftLine = new List<Rectangle>();
        List<Rectangle> rightLine = new List<Rectangle>();
        List<int> lineSpeeds = new List<int>();

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush grayBrush = new SolidBrush(Color.Gray);
        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Pen borderPen = new Pen(Color.White, 3);

        string gameState = "waiting";

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "Player1Win" || gameState == "Player2Win")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "Player1Win" || gameState == "Player2Win")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move player 1 
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= player1Speed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += player1Speed;
            }

            //move  player 2 
            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= player2Speed;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += player2Speed;
            }

            for (int i = 0; i < leftLine.Count(); i++)
            {
                int x = leftLine[i].X + lineSpeeds[i];
                leftLine[i] = new Rectangle(x, leftLine[i].Y, 5, 5);
            }

            leftLines--;
            if (leftLines == 0)
            {
                leftLine.Add(new Rectangle(0, randGen.Next(2, 250), 5, 5));
                leftLines = 20;
                lineSpeeds.Add(randGen.Next(2, 10));
            }

            for (int i = 0; i < rightLine.Count(); i++)
            {
                int x = rightLine[i].X - lineSpeeds[i];
                rightLine[i] = new Rectangle(x, rightLine[i].Y, 5, 5);
            }

            rightLines--;
            if (rightLines == 0)
            {
                rightLine.Add(new Rectangle(600, randGen.Next(2, 250), 5, 5));
                rightLines = 20;
                lineSpeeds.Add(randGen.Next(2, 10));
            }

            for (int i = 0; i < leftLine.Count(); i++)
            {
                int x = leftLine[i].X + lineSpeeds[i];
                leftLine[i] = new Rectangle(x, leftLine[i].Y, 5, 5);
            }


            for (int i = 0; i < leftLine.Count(); i++)
            {
                if (player1.IntersectsWith(leftLine[i]))
                {
                    SoundPlayer resetSound = new SoundPlayer(Properties.Resources.resetNoise);
                    resetSound.Play();
                    player1.X = 120;
                    player1.Y = 300;
                }
            }
            for (int i = 0; i < rightLine.Count(); i++)
            {
                if (player1.IntersectsWith(rightLine[i]))
                {
                    SoundPlayer resetSound = new SoundPlayer(Properties.Resources.resetNoise);
                    resetSound.Play();
                    player1.X = 120;
                    player1.Y = 300;
                }
            }

            if (player1.IntersectsWith(winRect))
            {
                SoundPlayer pointNoise = new SoundPlayer(Properties.Resources.pointCollect);
                pointNoise.Play();
                player1.X = 120;
                player1.Y = 300;
                player1Score++;
                p1ScoreLabel.Text = $"{player1Score}";
            }

            for (int i = 0; i < leftLine.Count(); i++)
            {
                if (player2.IntersectsWith(leftLine[i]))
                {
                    SoundPlayer resetSound = new SoundPlayer(Properties.Resources.resetNoise);
                    resetSound.Play();
                    player2.X = 420;
                    player2.Y = 300;
                }
            }
            for (int i = 0; i < rightLine.Count(); i++)
            {
                if (player2.IntersectsWith(rightLine[i]))
                {
                    SoundPlayer resetSound = new SoundPlayer(Properties.Resources.resetNoise);
                    resetSound.Play();
                    player2.X = 420;
                    player2.Y = 300;
                }
            }

            if (player2.IntersectsWith(winRect))
            {
                SoundPlayer pointNoise = new SoundPlayer(Properties.Resources.pointCollect);
                pointNoise.Play();
                player2.X = 420;
                player2.Y = 300;
                player2Score++;
                p2ScoreLabel.Text = $"{player2Score}";
            }

            if (player1Score == 3)
            {
                gameTimer.Enabled = false;
                gameState = "Player1Win";
            }

            if (player2Score == 3)
            {
                gameTimer.Enabled = false;
                gameState = "Player2Win";
            }
            Refresh();
        }
        public void GameInitialize()
        {
            player1.X = 120;
            player1.Y = 300;
            player2.X = 420;
            player2.Y = 300;
            player1Score = 0;
            player2Score = 0;
            p1ScoreLabel.Text = "0";
            p2ScoreLabel.Text = "0";
            titleLabel.Visible = false;
            subTitleLabel.Visible = false;
            gameTimer.Enabled = true;
            gameState = "running";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                gameTimer.Enabled = false;
                titleLabel.Text = "SPACE RACE";
                subTitleLabel.Text = "Press Spacebar to Start or Escape to Exit";
            }
            else if (gameState == "Player1Win")
            {
                SoundPlayer victoryPlayer = new SoundPlayer(Properties.Resources.victoryNoise);
                victoryPlayer.Play();
                titleLabel.Visible = true;
                titleLabel.Text = "PLAYER 1 WINS";
                subTitleLabel.Visible = true;
                subTitleLabel.Text = "Press Spacebar to Try Again or Escape to Exit";
            }
            else if (gameState == "Player2Win")
            {
                SoundPlayer victoryPlayer = new SoundPlayer(Properties.Resources.victoryNoise);
                victoryPlayer.Play();
                titleLabel.Visible = true;
                titleLabel.Text = "PLAYER 2 WINS";
                subTitleLabel.Visible = true;
                subTitleLabel.Text = "Press Spacebar to Try Again or Escape to Exit";
            }
            e.Graphics.FillRectangle(blueBrush, player1);
            e.Graphics.FillRectangle(orangeBrush, player2);
            e.Graphics.FillRectangle(grayBrush, divider);
            e.Graphics.FillRectangle(blackBrush, winRect);
            for (int i = 0; i < leftLine.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, leftLine[i]);
            }

            for (int i = 0; i < leftLine.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, rightLine[i]);
            }
        }
    }
}
