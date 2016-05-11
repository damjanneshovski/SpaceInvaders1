using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceInvaders1
{
    class Invader : GameObject
    {
        private Image otherImage = null;
        private const int kBombInterval = 200;
        private Bomb theBomb = new Bomb(0, 0);
        private bool activeBomb = false;
        public bool beenHit = false;
        public int countExplosion = 0;
        public bool died = false;
        private int rseed = (int)DateTime.Now.Ticks;
        private Random random = null;
        public bool directionRight = true;
        private const int kInterval = 5;
        private long counter = 0;

        public Invader(string img1, string img2) : base(img1)
        {
            movingBounds.Width = 55;
            movingBounds.Height = 42;
            otherImage = Image.FromFile(img2);
            random = new Random(rseed);
            position.X = 20;
            position.Y = 10;
            UpdateBounds();
        }

        public void ResetBombPosition()
        {
            theBomb.position.X = movingBounds.X + movingBounds.Width / 2;
            theBomb.ResetBomb(movingBounds.Y + 5);
        }

        public void SetCounter(long lCount)
        {
            counter = lCount;
        }

        public void DrawExplosion(Graphics g)
        {
            if (died)
                return;

            countExplosion++;
            if (countExplosion < 15)
            {
                for (int i = 0; i < 50; i++)
                {
                    int xval = movingBounds.Width / 2;
                    int yval = movingBounds.Height / 2;
                    xval += position.X;
                    yval += position.Y;
                    g.DrawLine(Pens.White, xval, yval, xval, yval + 1);
                }
            }
            else
            {
                died = true;
            }
        }

        public void Move()
        {
            if (beenHit)
                return;

            if (directionRight)
            {
                position.X += kInterval;
            }
            else
            {
                position.X -= kInterval;
            }

            counter++;
        }

        public void MoveInPlace()
        {
            counter++;
        }

        public Rectangle GetBombBounds()
        {
            return theBomb.GetBounds();
        }

        public bool IsBombColliding(Rectangle r)
        {
            if (activeBomb && theBomb.GetBounds().IntersectsWith(r))
            {
                return true;
            }

            return false;
        }

        public override void Draw(Graphics g)
        {
            UpdateBounds();

            if (beenHit)
            {
                DrawExplosion(g);
                return;
            }

            if (counter % 2 == 0)
                g.DrawImage(image, movingBounds, 0, 0, imageBounds.Width, imageBounds.Height, GraphicsUnit.Pixel);
            else
                g.DrawImage(otherImage, movingBounds, 0, 0, imageBounds.Width, imageBounds.Height, GraphicsUnit.Pixel);

            if (activeBomb)
            {
                theBomb.Draw(g);
                if (Game.ActiveForm != null)
                {
                    if (theBomb.position.Y > Game.ActiveForm.ClientRectangle.Height)
                    {
                        activeBomb = false;
                    }
                }
            }

            if ((activeBomb == false) && (counter % kBombInterval == 0))
            {
                activeBomb = true;
                theBomb.position.X = movingBounds.X + movingBounds.Width / 2;
                theBomb.position.Y = movingBounds.Y + 5;
            }
        }
    }
}