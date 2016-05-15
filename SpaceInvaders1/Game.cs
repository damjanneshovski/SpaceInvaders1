using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders1
{
    public partial class Game : Form
    {
        
        private const int kNumberOfRows = 5;
        private const int kNumberOfTries = 3;
        private const int kNumberOfShields = 4;
        private long TimerCounter = 0;
        private int TheSpeed = 6;
        private int TheLevel = 0;
        private bool ActiveBullet = false;
        private int NumberOfMen = 3;
        private Spaceship TheMan = null;
        private bool GameGoing = true;
        private Bullet TheBullet = new Bullet(20, 30);
        private Score TheScore = null;
        private InvadersRow[] InvaderRows = new InvadersRow[6];
        private InvadersRow TheInvaders = null;
        private string CurrentKeyDown = "";
        private string LastKeyDown = "";
        private Thread oThread = null;

        [DllImport("winmm.dll", SetLastError = true)]
        static extern bool PlaySound(string pszSound, UIntPtr hmod, uint fdwSound);

        [Flags]
        public enum SoundFlags
        {
            /// <summary>play synchronously (default)</summary>
            SND_SYNC = 0x0000,
            /// <summary>play asynchronously</summary>
            SND_ASYNC = 0x0001,
            /// <summary>silence (!default) if sound not found</summary>
            SND_NODEFAULT = 0x0002,
            /// <summary>pszSound points to a memory file</summary>
            SND_MEMORY = 0x0004,
            /// <summary>loop the sound until next sndPlaySound</summary>
            SND_LOOP = 0x0008,
            /// <summary>don’t stop any currently playing sound</summary>
            SND_NOSTOP = 0x0010,
            /// <summary>Stop Playing Wave</summary>
            SND_PURGE = 0x40,
            /// <summary>don’t wait if the driver is busy</summary>
            SND_NOWAIT = 0x00002000,
            /// <summary>name is a registry alias</summary>
            SND_ALIAS = 0x00010000,
            /// <summary>alias is a predefined id</summary>
            SND_ALIAS_ID = 0x00110000,
            /// <summary>name is file name</summary>
            SND_FILENAME = 0x00020000,
            /// <summary>name is resource name or atom</summary>
            SND_RESOURCE = 0x00040004
        }

        public Game()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            InitializeAllGameObjects(true);

            timer1.Start();
        }

        private void Game_Load(object sender, EventArgs e)
        {

        }

        private void InitializeAllGameObjects(bool bScore)
        {
            InitializeSpaceship();

            if (bScore)
                InitializeScore();

            InitializeInvaderRows(TheLevel);

            TheScore.GameOver = false;
            GameGoing = true;
            TheSpeed = 6;
        }

        private void InitializeSpaceship()
        {
            TheMan = new Spaceship();
            TheMan.position.Y = ClientRectangle.Bottom - 50;
            NumberOfMen = 3;
        }

        private void InitializeScore()
        {
            TheScore = new Score(ClientRectangle.Left + 15, 50);
        }

        void InitializeInvaderRows(int level)
        {
            InvaderRows[0] = new InvadersRow("invader1.gif", "invader1c.gif", 2 + level);
            InvaderRows[1] = new InvadersRow("invader2.gif", "invader2c.gif", 3 + level);
            InvaderRows[2] = new InvadersRow("invader2.gif", "invader2c.gif", 4 + level);
            InvaderRows[3] = new InvadersRow("invader3.gif", "invader3c.gif", 5 + level);
            InvaderRows[4] = new InvadersRow("invader3.gif", "invader3c.gif", 6 + level);
        }

        private string m_strCurrentSoundFile = "1.wav";
        public void PlayASound()
        {
            if (m_strCurrentSoundFile.Length > 0)
            {
                PlaySound(Application.StartupPath + "\\" + m_strCurrentSoundFile, UIntPtr.Zero, (uint)(SoundFlags.SND_FILENAME | SoundFlags.SND_ASYNC));
            }
            m_strCurrentSoundFile = "";
            m_nCurrentPriority = 3;
            oThread.Abort();
        }

        int m_nCurrentPriority = 3;
        public void PlaySoundInThread(string wavefile, int priority)
        {
            if (priority <= m_nCurrentPriority)
            {
                m_nCurrentPriority = priority;
                if (oThread != null)
                    oThread.Abort();

                m_strCurrentSoundFile = wavefile;
                oThread = new Thread(new ThreadStart(PlayASound));
                oThread.Start();

            }

        }

        private void HandleKeys()
        {
            switch (CurrentKeyDown)
            {
                case "Space":
                    if (ActiveBullet == false)
                    {
                        TheBullet.position = TheMan.GetBulletStart();
                        ActiveBullet = true;
                        PlaySoundInThread("1.wav", 2);
                    }
                    CurrentKeyDown = LastKeyDown;
                    break;
                case "Left":
                    TheMan.MoveLeft();
                    Invalidate(TheMan.GetBounds());
                    if (timer1.Enabled == false)
                        timer1.Start();
                    break;
                case "Right":
                    TheMan.MoveRight(ClientRectangle.Right);
                    Invalidate(TheMan.GetBounds());
                    if (timer1.Enabled == false)
                        timer1.Start();
                    break;
                default:
                    break;
            }


        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            string result = e.KeyData.ToString();
            CurrentKeyDown = result;
            if (result == "Left" || result == "Right")
            {
                LastKeyDown = result;
            }
        }

        private void Game_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            TheMan.Draw(g);
            TheScore.Draw(g);
            if (ActiveBullet)
            {
                TheBullet.Draw(g);
            }

            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                TheInvaders.Draw(g);
            }
        }

        private int CalculateLargestLastPosition()
        {
            int max = 0;
            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                int nLastPos = TheInvaders.GetLastInvader().position.X;
                if (nLastPos > max)
                    max = nLastPos;
            }

            return max;
        }

        private int CalculateSmallestFirstPosition()
        {
            int min = 50000;

            try
            {
                for (int i = 0; i < kNumberOfRows; i++)
                {
                    TheInvaders = InvaderRows[i];
                    int nFirstPos = TheInvaders.GetFirstInvader().position.X;
                    if (nFirstPos < min)
                        min = nFirstPos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            return min;
        }

        private void MoveInvaders()
        {
            bool bMoveDown = false;

            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                TheInvaders.Move();
            }

            PlaySoundInThread("4.wav", 3);

            if ((CalculateLargestLastPosition()) > ClientRectangle.Width - InvaderRows[4][0].GetWidth() - 20)
            {
                TheInvaders.DirectionRight = false;
                SetAllDirections(false);
                for (int i = 0; i < kNumberOfRows; i++)
                {
                    bMoveDown = true;
                }
            }

            if ((CalculateSmallestFirstPosition()) < InvaderRows[4][0].Width / 3)
            {
                TheInvaders.DirectionRight = true;
                SetAllDirections(true);
                for (int i = 0; i < kNumberOfRows; i++)
                {
                    bMoveDown = true;
                }
            }

            if (bMoveDown)
            {
                for (int i = 0; i < kNumberOfRows; i++)
                {

                    TheInvaders = InvaderRows[i];
                    TheInvaders.MoveDown();

                }
            }
        }

        private int TotalNumberOfInvaders()
        {
            int sum = 0;
            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                sum += TheInvaders.NumberOfLiveInvaders();
            }

            return sum;
        }

        private void MoveInvadersInPlace()
        {
            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                TheInvaders.MoveInPlace();
            }

        }

        private void SetAllDirections(bool bDirection)
        {
            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                TheInvaders.DirectionRight = bDirection;
            }

        }

        public int CalcScoreFromRow(int num)
        {
            int nScore = 10;
            switch (num)
            {
                case 0:
                    nScore = 40;
                    break;
                case 1:
                    nScore = 20;
                    break;
                case 2:
                    nScore = 20;
                    break;
                case 3:
                    nScore = 10;
                    break;
                case 4:
                    nScore = 10;
                    break;
            }

            return nScore;
        }

        void TestBulletCollision()
        {
            int rowNum = 0;
            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                rowNum = i;
                int collisionIndex = TheInvaders.CollisionTest(TheBullet.GetBounds());

                if ((collisionIndex >= 0) && ActiveBullet)
                {
                    TheInvaders.invaders[collisionIndex].beenHit = true;
                    TheScore.AddScore(CalcScoreFromRow(rowNum));
                    PlaySoundInThread("0.wav", 1);

                    ActiveBullet = false;
                    TheBullet.Reset();
                }
            }
        }

        void TestForLanding()
        {
            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                if (TheInvaders.AlienHasLanded(ClientRectangle.Bottom))
                {
                    TheMan.beenHit = true;
                    PlaySoundInThread("2.wav", 1);
                    TheScore.GameOver = true;
                    GameGoing = false;
                }
            }

        }

        void ResetAllBombCounters()
        {
            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                TheInvaders.ResetBombCounters();
            }
        }

        void TestBombCollision()
        {
            if (TheMan.died)
            {
                NumberOfMen--;
                if (NumberOfMen == 0)
                {
                    TheScore.GameOver = true;
                    GameGoing = false;
                }
                else
                {
                    TheMan.Reset();
                    ResetAllBombCounters();
                }
            }

            if (TheMan.beenHit == true)
                return;

            for (int i = 0; i < kNumberOfRows; i++)
            {
                TheInvaders = InvaderRows[i];
                for (int j = 0; j < TheInvaders.invaders.Length; j++)
                {
                    if (TheInvaders.invaders[j].IsBombColliding(TheMan.GetBounds()))
                    {
                        TheMan.beenHit = true;
                        PlaySoundInThread("2.wav", 1);
                    }
                }
            }
        }

        private int nTotalInvaders = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            HandleKeys();

            TimerCounter++;

            if (GameGoing == false)
            {
                if (TimerCounter % 6 == 0)
                    MoveInvadersInPlace();
                Invalidate();
                return;
            }


            if (TheBullet.position.Y < 0)
            {
                ActiveBullet = false;
            }
            

            if (TimerCounter % TheSpeed == 0)
            {
                MoveInvaders();

                nTotalInvaders = TotalNumberOfInvaders();

                if (nTotalInvaders <= 20)
                {
                    TheSpeed = 5;
                }

                if (nTotalInvaders <= 10)
                {
                    TheSpeed = 4;
                }


                if (nTotalInvaders <= 5)
                {
                    TheSpeed = 3;
                }

                if (nTotalInvaders <= 3)
                {
                    TheSpeed = 2;
                }

                if (nTotalInvaders <= 1)
                {
                    TheSpeed = 1;
                }

                if (nTotalInvaders == 0)
                {
                    InitializeAllGameObjects(false); 				
                    TheLevel++;
                }


            }

            TestBulletCollision();
            TestBombCollision();
            Invalidate();
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            string result = e.KeyData.ToString();
            if (result == "Left" || result == "Right")
            {
                LastKeyDown = "";
            }
        }

        private void Game_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == 113)
            {
                QuitScreen q = new QuitScreen();
                q.ShowDialog();
            }
        }
    }
}
