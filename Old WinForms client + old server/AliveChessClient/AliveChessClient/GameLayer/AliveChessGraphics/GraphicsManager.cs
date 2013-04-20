using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using AliveChessLibrary.Entities.Abstract;

namespace AliveChessClient.GameLayer.AliveChessGraphics
{
    public class GraphicsManager
    {
        private GameContext context;
        private Rectangle drawableArea;

        private Bitmap bmp;
        private Graphics graphics;

        private Image imgCurr;

        private int cellSizeX = 20;
        private int cellSizeY = 20;

        private int distanceX;
        private int distanceY;

        private Dictionary<string, Image> textures;

        public GraphicsManager(GameContext context)
        {
            this.context = context;
            this.bmp = context.GameForm.Bitmap;
            this.graphics = Graphics.FromImage(bmp);
            this.textures = new Dictionary<string, Image>();

            this.distanceX = 10;
            this.distanceY = 10;
        }

        public void Load()
        {
            textures.Add("Empty", Image.FromFile("Pictures/empty.png"));
            textures.Add("Wood", Image.FromFile("Pictures/tree.png"));
            textures.Add("Obstacle", Image.FromFile("Pictures/obstacle.png"));
            textures.Add("Castle", Image.FromFile("Pictures/castle.png"));
            textures.Add("King", Image.FromFile("Pictures/king.png"));
            textures.Add("Mine", Image.FromFile("Pictures/gold_mine.png"));
            textures.Add("Gold", Image.FromFile("Pictures/gold.png"));
            textures.Add("Timber", Image.FromFile("Pictures/timber.png"));
        }

        public void Draw()
        {
            graphics.Clear(Color.Black);

            for (int i = drawableArea.X; i < drawableArea.Width; i++)
            {
                for (int j = drawableArea.Y; j < drawableArea.Height; j++)
                {
                    string[] elems = context.Context.ParseEntityName(context.Player.Map[i, j].Name).Split(':');
                    if (context.Player.Map[i, j].Detected)
                    {
                        imgCurr = textures[elems[2]]; ;
                        Draw(context.Player.Map[i, j]);
                    }
                }
            }

            context.GameForm.GameScene = bmp;
        }

        private void Draw(MapObject mObject)
        {
            graphics.DrawImage(imgCurr, new Rectangle((mObject.X - drawableArea.X) * cellSizeX,
                (mObject.Y - drawableArea.Y) * cellSizeY, cellSizeX, cellSizeY));
        }

        public void CalculateBounds()
        {
            int distanceX0, distanceY0;
            int distanceX1, distanceY1;

            if (context.Player.King.X - distanceX < 0)
                distanceX0 = 0;
            else distanceX0 = context.Player.King.X - distanceX;

            if (context.Player.King.Y - distanceY < 0)
                distanceY0 = 0;
            else distanceY0 = context.Player.King.Y - distanceY;

            if (context.Player.King.X + distanceX >= context.Player.Map.SizeX)
                distanceX1 = context.Player.Map.SizeX;
            else distanceX1 = distanceX0 + 2 * distanceX;

            if (context.Player.King.Y + distanceY >= context.Player.Map.SizeY)
                distanceY1 = context.Player.Map.SizeY;
            else distanceY1 = distanceY0 + 2 * distanceY;

            drawableArea = new Rectangle(distanceX0, distanceY0, distanceX1, distanceY1);
        }

        public Rectangle DrawableArea
        {
            get { return drawableArea; }
        }
    }

    public delegate void DrawHandler();
}
