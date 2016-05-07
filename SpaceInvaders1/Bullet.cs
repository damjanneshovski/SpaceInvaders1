using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceInvaders1
{
    class Bullet : GameObject
    {
        public const int kBulletInterval = 30;
        public int bulletInterval = kBulletInterval;

        public Bullet(int x, int y)
        {
            imageBounds.Width = 5;
            imageBounds.Height = 15;
            position.X = x;
            position.Y = y;
        }

        public void Reset()
        {
            if (Game.ActiveForm != null)
            {
                position.Y = Game.ActiveForm.ClientRectangle.Bottom;
                movingBounds.Y = position.Y;
            }
            bulletInterval = kBulletInterval;
        }

        public override void Draw(Graphics g)
        {
            UpdateBounds();
            g.FillRectangle(Brushes.Chartreuse, movingBounds);
            position.Y -= bulletInterval;
        }
    }
}
