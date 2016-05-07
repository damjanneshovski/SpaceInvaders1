using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceInvaders1
{
    class Bomb : GameObject
    {
        public const int kBombInterval = 10;
        public int bombInterval = kBombInterval;

        public Bomb(int x, int y)
        {
            imageBounds.Width = 5;
            imageBounds.Height = 15;
            position.X = x;
            position.Y = y;
        }
        
        public void ResetBomb(int yPos)
        {
            position.Y = yPos;
            bombInterval = kBombInterval;
            UpdateBounds();
        }

        public override void Draw(Graphics g)
        {
            UpdateBounds();
            g.FillRectangle(Brushes.White, movingBounds);
            position.Y += bombInterval;
        }
    }
}
