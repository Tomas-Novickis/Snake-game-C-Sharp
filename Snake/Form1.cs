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

namespace Snake
{
    public partial class Form1 : Form
    {
        private SoundPlayer sp;
        
        private List<Circle> Player = new List<Circle>();
        private Circle food = new Circle();

        int maxWidth;
        int maxHeight;

        int score;
        int HighScore;

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;

        public Form1()
        {
            InitializeComponent();
            sp = new SoundPlayer("cabaraSNAKE.wav");
            new Settings();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if(goLeft)
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }

            for (int i = Player.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.directions)
                    {
                        case "left":
                            Player[i].X--;
                            break;
                        case "right":
                            Player[i].X++;
                            break;
                        case "down":
                            Player[i].Y++;
                            break;
                        case "up":
                            Player[i].Y--;
                            break;
                    }

                    if (Player[i].X < 0)
                    {
                        Player[i].X = maxWidth;
                    }
                    if (Player[i].X > maxWidth)
                    {
                        Player[i].X = 0;
                    }
                    if (Player[i].Y < 0)
                    {
                        Player[i].Y = maxHeight;
                    }
                    if (Player[i].Y > maxHeight)
                    {
                        Player[i].Y = 0;
                    }

                    if (Player[i].X == food.X && Player[i].Y == food.Y)
                    {
                        EatFood();
                    }

                    for (int j = 1; j< Player.Count; j++)
                    {
                        if (Player[i].X == Player[j].X && Player[i].Y == Player[j].Y)
                        {
                            GameOver();
                        }
                    }



                }
                else
                {
                    Player[i].X = Player[i - 1].X;
                    Player[i].Y = Player[i - 1].Y;
                }
            }

            picCanvas.Invalidate();


        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            Brush snakeColour;

            for(int i = 0; i < Player.Count; i++)
            {
                if (i == 0)
                {
                    snakeColour = Brushes.Black;
                }
                else
                {
                    snakeColour = Brushes.DarkGreen; 
                }

                canvas.FillEllipse(snakeColour, new Rectangle
                    (
                    Player[i].X * Settings.Width,
                    Player[i].Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
            }


            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
                    (
                    food.X * Settings.Width,
                    food.Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "1":
                    gameTimer.Interval = 60;
                    break;
                case "2":
                    gameTimer.Interval = 55;
                    break;
                case "3":
                    gameTimer.Interval = 50;
                    break;
                case "4":
                    gameTimer.Interval = 45;
                    break;
                case "5":
                    gameTimer.Interval = 40;
                    break;
                case "6":
                    gameTimer.Interval = 35;
                    break;
                case "7":
                    gameTimer.Interval = 30;
                    break;
                case "8":
                    gameTimer.Interval = 25;
                    break;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
                
        }

        private void RestartGame()
        {
            sp.PlayLooping();
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;

            Player.Clear();

            button1.Enabled = false;
            comboBox1.Enabled = false;
            score = 0;
            Score_label.Text = "Score: " + score;

            Circle head = new Circle { X = 10, Y = 5 };
            Player.Add(head);

            for (int i = 0; i < 5; i++)
            {
                Circle body = new Circle();
                Player.Add(body);
            }

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight)};

            gameTimer.Start();
        }

        private void EatFood()
        {
            score += 1;

            Score_label.Text = "Score: " + score;

            Circle body = new Circle
            {
                X = Player[Player.Count - 1].X,
                Y = Player[Player.Count - 1].Y
            };

            Player.Add(body);

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
            gameTimer.Interval = gameTimer.Interval - 1;
        }

        private void GameOver()
        {
            gameTimer.Stop();
            button1.Enabled = true;
            comboBox1.Enabled = true;

            if(score> HighScore)
            {
                HighScore = score;

                Highscore_label.Text = "High Score: " + Environment.NewLine + HighScore;
                Highscore_label.ForeColor = Color.DarkRed;
                Highscore_label.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
