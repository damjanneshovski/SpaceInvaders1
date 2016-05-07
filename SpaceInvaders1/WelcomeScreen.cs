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

namespace SpaceInvaders1
{
    public partial class WelcomeScreen : Form
    {
        private SoundPlayer soundPlayer;

        public WelcomeScreen()
        {
            InitializeComponent();
            InitializeComponent();
            soundPlayer = new SoundPlayer(Resource1.Beep);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            triangle1.Visible = true;
            soundPlayer.Play();
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            triangle1.Visible = false;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            triangle2.Visible = true;
            soundPlayer.Play();
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            triangle2.Visible = false;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            triangle3.Visible = true;
            soundPlayer.Play();
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            triangle3.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            QuitScreen q = new QuitScreen();
            q.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Controls c = new Controls();
            c.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game g = new Game();
            g.ShowDialog();
        }
    }
}
