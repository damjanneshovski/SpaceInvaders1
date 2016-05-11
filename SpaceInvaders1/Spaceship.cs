using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceInvaders1
{
    class Spaceship : GameObject
    {
        private int kInterval = 10;
        public bool died = false;

        public Spaceship() : base("spaceship.gif")
		{
            position.X = 20;
            position.Y = 400;
        }

        public Point GetBulletStart()
        {
            Point theStart = new Point(movingBounds.Left + movingBounds.Width / 2 - 2, movingBounds.Top - 10);
            return theStart;
        }

        int countExplosion = 0;
        Random random = new Random((int)DateTime.Now.Ticks);

        public void DrawExplosion(Graphics g)
        {
            countExplosion++;
            if (countExplosion < 15)
            {
                for (int i = 0; i < 50; i++)
                {
                    int xval = random.Next(movingBounds.Width);
                    int yval = random.Next(movingBounds.Height);
                    xval += position.X;
                    yval += position.Y;
                    g.DrawLine(Pens.Chartreuse, xval, yval, xval, yval + 1);
                }
            }
        }

        public bool beenHit = false;
        
        public void MoveLeft()
        {
            position.X -= kInterval;
            if (position.X < 0)
                position.X = 0;
        }

        public void MoveRight(int nLimit)
        {
            position.X += kInterval;
            if (position.X > nLimit - Width)
                position.X = nLimit - Width;
        }

        public override void Draw(Graphics g)
        {
            if (died)
                return;

            if (beenHit == false)
            {
                base.Draw(g);
            }
            else
            {
                if (countExplosion < 15)
                    DrawExplosion(g);
                else
                    died = true;
            }
        }

        public void Reset()
        {
            beenHit = false;
            died = false;
            countExplosion = 0;
        }
    }
}
