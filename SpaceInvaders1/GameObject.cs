using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceInvaders1
{
    class GameObject
    {
        protected Image image = null;
        public Point position = new Point(50, 50);
        protected Rectangle imageBounds = new Rectangle(0, 0, 10, 10);
        protected Rectangle movingBounds = new Rectangle();

        public int Width
        {
            get
            {
                return imageBounds.Width;
            }
        }

        public int Height
        {
            get
            {
                return imageBounds.Height;
            }
        }

        public GameObject(string fileName)
        {
            image = Image.FromFile(fileName);
            imageBounds.Width = image.Width;
            imageBounds.Height = image.Height;
        }

        public GameObject()
        {
            image = null;
        }
                
        public virtual int GetWidth()
        {
            return imageBounds.Width;
        }

        public Image GetImage()
        {
            return image;
        }

        public virtual Rectangle GetBounds()
        {
            return movingBounds;
        }

        public void UpdateBounds()
        {
            movingBounds = imageBounds;
            movingBounds.Offset(position);
        }
        
        public virtual void Draw(Graphics g)
        {
            UpdateBounds();
            g.DrawImage(image, movingBounds, 0, 0, imageBounds.Width, imageBounds.Height, GraphicsUnit.Pixel);
        }
    }
}
