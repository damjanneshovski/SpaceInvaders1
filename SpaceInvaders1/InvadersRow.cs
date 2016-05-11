using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SpaceInvaders1
{
    class InvadersRow
    {
        public Invader[] invaders = new Invader[10];
        public Point lastPosition = new Point(0, 0);
        public const int kBombIntervalSpacing = 50;

        public InvadersRow(string img1, string img2, int rowNum)
        {
            for (int i = 0; i < invaders.Length; i++)
            {
                invaders[i] = new Invader(img1, img2);
                invaders[i].position.X = i * invaders[i].GetBounds().Width + 5;
                invaders[i].position.Y = rowNum * invaders[i].GetBounds().Height + 10;
                invaders[i].SetCounter(i * kBombIntervalSpacing);
            }

            lastPosition = invaders[invaders.Length - 1].position;
        }

        public void ResetBombCounters()
        {
            for (int i = 0; i < invaders.Length; i++)
            {
                invaders[i].ResetBombPosition();
                invaders[i].SetCounter(i * kBombIntervalSpacing);
            }
        }

        public Invader this[int index]
        {
            get
            {
                return invaders[index];
            }
        }


        public void Draw(Graphics g)
        {
            for (int i = 0; i < invaders.Length; i++)
            {
                invaders[i].Draw(g);
            }
        }

        public int CollisionTest(Rectangle aRect)
        {
            for (int i = 0; i < invaders.Length; i++)
            {
                if ((invaders[i].GetBounds().IntersectsWith(aRect)) && (!invaders[i].beenHit))
                    return i;
            }

            return -1;
        }

        public bool DirectionRight
        {
            set
            {
                for (int i = 0; i < invaders.Length; i++)
                {
                    invaders[i].directionRight = value;
                }
            }
        }

        public void Move()
        {
            for (int i = 0; i < invaders.Length; i++)
            {
                invaders[i].Move();
            }

            if (invaders[0].directionRight)
                lastPosition = invaders[invaders.Length - 1].position;
            else
                lastPosition = invaders[0].position;

        }

        public void MoveInPlace()
        {
            for (int i = 0; i < invaders.Length; i++)
            {
                invaders[i].MoveInPlace();
            }

        }


        public Invader GetFirstInvader()
        {
            int count = 0;
            Invader TheInvader = invaders[count];
            while ((TheInvader.beenHit == true) && (count < invaders.Length - 1))
            {
                count++;
                TheInvader = invaders[count];
            }

            return TheInvader;
        }

        public Invader GetLastInvader()
        {
            int count = invaders.Length - 1;
            Invader TheInvader = invaders[count];
            while ((TheInvader.beenHit == true) && (count > 0))
            {
                count--;
                TheInvader = invaders[count];
            }

            return TheInvader;
        }

        public int NumberOfLiveInvaders()
        {
            int count = 0;
            for (int i = 0; i < invaders.Length; i++)
            {
                if (invaders[i].died == false)
                    count++;
            }

            return count;
        }

        public bool AlienHasLanded(int bottom)
        {
            for (int i = 0; i < invaders.Length; i++)
            {
                if ((invaders[i].GetBounds().Bottom >= bottom) &&
                     (invaders[i].beenHit = false))
                    return true;
            }

            return false;

        }

        public void MoveDown()
        {
            for (int i = 0; i < invaders.Length; i++)
            {
                invaders[i].position.Y += invaders[i].GetBounds().Height / 8;
                invaders[i].UpdateBounds();
            }
        }
    }
}
