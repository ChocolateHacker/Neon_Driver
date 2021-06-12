using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/* ОПИСАНИЕ
//Игра Neon Driver
//Гонщик попадает на арену, и ему надо проезжать арки.
//Игра проходит на арене, где гонщик катается на неоновом байке.
//Суть игры набрать много очков.
//За мотоциклом следует линия, которая показывает их путь и способна убить игрока.
//Убивает наезд на линию.
*/
namespace Neon_Drivers
{
    public partial class PlayerView : Form
    {
        public int score = 1;
        public int dirX, dirY, lineX, lineY, randI, randJ, pictureAngle;
        private PictureBox riderBlue;
        private PictureBox[] line = new PictureBox[100];
        private PictureBox arch;
        private PictureBox logoNeonDrivers;
        private Label labelScore;
        private Label rules;
        public PlayerView()
        {
            InitializeComponent();
            dirX = 0;
            dirY = 0;
            lineX = 19;
            lineY = 19;
            line[0] = new PictureBox()
            {
                Location = new Point(0,0),
                Size = new Size(0, 0),
                BackColor = Color.Black
            };
            riderBlue = new PictureBox()
            {
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Center,
                Image = Properties.Resources.Driver_BlueV2,
                Location = new Point(552, 263),
                Name = "riderBlue",
                Size = new Size(40, 40),
                TabIndex = 0,
                TabStop = false
            };
            labelScore = new Label()
            {
                Text = "Score: 0",
                Size = new Size(150, 40),
                Location = new Point(480, 500),
                BackColor = Color.Aqua,
                Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point),
            };
            arch = new PictureBox()
            {
                BackColor = Color.Red,
                Size = new Size(15, 15)
            };
            logoNeonDrivers = new PictureBox()
            {
                Image = Properties.Resources.New_Neon_Driver,
                Location = new Point(658, 519),
                Name = "logoNeonDrivers",
                Size = new Size(594, 150),
                TabIndex = 7,
                TabStop = false,
            };
            rules = new Label()
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Aqua,
                Location = new Point(12, 529),
                Name = "rules",
                Size = new Size(343, 140),
                TabIndex = 8,
                Text = "WASD - Управление гонщиком\r\nУдерживание WASD - Тормоз",
            };
            timer1 = new Timer();
            timer1.Tick += new EventHandler(Update);
            timer1.Interval = 5;
            timer1.Start();
            Controls.Add(riderBlue);
            Controls.Add(rules);
            Controls.Add(labelScore);
            Controls.Add(logoNeonDrivers);
            GenerateMap();
            GenerateArch();
            riderBlue.Paint += new PaintEventHandler(RiderBlueRotate);
            KeyDown += new KeyEventHandler(Key);
        }

        private void Update(Object o, EventArgs e)
        {
            Borders();
            RideToArchs();
            MoveRider();
            CreateLine();
        }

        private void GenerateArch()
        {
            var rand = new Random();
            randI = rand.Next(0, 1225);
            randJ = rand.Next(0, 440);
            int cancelI = randI % 10;
            int cancelJ = randJ % 10;
            randI -= cancelI;
            randJ -= cancelJ;
            arch.Location = new Point(randI, randJ);
            this.Controls.Add(arch);
        }

        private void GenerateMap()
        {
            for (var i = 0; i < 13; i++)
            {
                var p = new PictureBox();
                p.BackColor = Color.White;
                p.Location = new Point(1, 40 * i);
                p.Size = new Size(1280, 1);
                this.Controls.Add(p);
            }

            for (var i = 1; i < 100; i++)
            {
                var p = new PictureBox();
                p.BackColor = Color.White;
                p.Location = new Point(40 * i, 1);
                p.Size = new Size(1, 480);
                this.Controls.Add(p);
            }
        }

        void RiderBlueRotate(Object o, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(3,28,30));
            e.Graphics.TranslateTransform(riderBlue.Image.Width / 2, riderBlue.Image.Height / 2);
            e.Graphics.RotateTransform(pictureAngle);
            e.Graphics.DrawImage(riderBlue.Image, -riderBlue.Image.Width / 2, -riderBlue.Image.Height / 2);
            e.Graphics.RotateTransform(pictureAngle);
            e.Graphics.TranslateTransform(-riderBlue.Image.Width / 2, -riderBlue.Image.Height / 2);
        }

        public void Key(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                if (pictureAngle != 180)
                {
                    pictureAngle = 0;
                    dirX = 0;
                    dirY = -1;
                }
            }
            if (e.KeyCode == Keys.S)
            {
                if (pictureAngle != 0)
                {
                    pictureAngle = 180;
                    dirX = 0;
                    dirY = 1;
                }
            }
            if (e.KeyCode == Keys.A)
            {
                if (pictureAngle != 90)
                {
                    pictureAngle = 270;
                    dirX = -1;
                    dirY = 0;
                }
            }
            if (e.KeyCode == Keys.D)
            {
                if (pictureAngle != 270)
                {
                    dirX = 1;
                    dirY = 0;
                    pictureAngle = 90;
                }
            }
        }

        public void CreateLine()
        {
                line[0] = new PictureBox
                {
                    Location = new Point(riderBlue.Location.X + lineX, riderBlue.Location.Y + lineY),
                    Size = new Size(2, 2),
                    BackColor = Color.FromArgb(0, 255, 255)
                };
                Controls.Add(line[0]);
                line[0].BringToFront();
        }

        private void RideToArchs()
        {
            if (riderBlue.Location.X <= randI + 25
                && riderBlue.Location.X >= randI - 25
                && riderBlue.Location.Y <= randJ + 25
                && riderBlue.Location.Y >= randJ - 25)
            {
                arch.Location = new Point(line[0].Location.X + dirX, line[0].Location.Y - dirY);
                arch.Size = new Size(15, 15);
                arch.BackColor = Color.Red;
                this.Controls.Add(arch);
                GenerateArch();
                labelScore.Text = "Score: " + score++;
            }
        }

        public void MoveRider()
        {
            riderBlue.Location = new Point(riderBlue.Location.X + dirX, riderBlue.Location.Y + dirY);
        }

        private void Borders()
        {
            if (riderBlue.Location.X < 1)
            {
                riderBlue.Location = new Point(riderBlue.Location.X + 1, riderBlue.Location.Y);
                dirX = 0;
                GameOver();
            }
            if (riderBlue.Location.X > 1225)
            {
                riderBlue.Location = new Point(riderBlue.Location.X - 1, riderBlue.Location.Y);
                dirX = 0;
                GameOver();
            }
            if (riderBlue.Location.Y < 1)
            {
                riderBlue.Location = new Point(riderBlue.Location.X, riderBlue.Location.Y + 1);
                dirY = 0;
                GameOver();
            }
            if (riderBlue.Location.Y > 440)
            {
                riderBlue.Location = new Point(riderBlue.Location.X, riderBlue.Location.Y - 1);
                dirY = 0;
                GameOver();
            }
        }

        private void GameOver()
        {
            var label = new Label();
            label.Text = "Game over";
            label.Size = new Size(150, 40);
            label.Location = new Point(480, 560);
            label.BackColor = Color.Orange;
            label.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            Controls.Add(label);
            riderBlue.Hide();
            riderBlue.Location = new Point(9999, 9999);
        }
    }
}