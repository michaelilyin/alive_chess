using System.Collections.Generic;
using System.Drawing;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessClient.GameLayer.AliveChessGraphics
{
    public class BigMapGraphicsManager : IGraphicManager
    {
        private Game game;
        private Rectangle drawableArea;

        private Bitmap bmp;
        private Graphics graphics;

        private Image imgCurr;

        private int cellSizeX = 20;
        private int cellSizeY = 20;

        private int distanceX;
        private int distanceY;

        //private Dictionary<MapObjectTypes, Image> textures;
        private Dictionary<long, Image> textures;

        public BigMapGraphicsManager(Game game, Bitmap bitmap)
        {
            this.game = game;
            this.bmp = bitmap;
            this.graphics = Graphics.FromImage(bmp);
            this.textures = new Dictionary<long, Image>();
            //this.textures = new Dictionary<MapObjectTypes, Image>();

            this.distanceX = 10;
            this.distanceY = 10;
        }

        public void Load()
        {
            textures.Add(0, Image.FromFile("Pictures/empty.png"));
            textures.Add(3, Image.FromFile("Pictures/tree.png"));
            textures.Add(4, Image.FromFile("Pictures/obstacle.png"));
            textures.Add(1, Image.FromFile("Pictures/castle.png"));
            textures.Add(8, Image.FromFile("Pictures/king.png"));
            textures.Add(2, Image.FromFile("Pictures/gold_mine.png"));
            textures.Add(7, Image.FromFile("Pictures/ryobi.png"));
            textures.Add(5, Image.FromFile("Pictures/gold.png"));
            textures.Add(6, Image.FromFile("Pictures/timber.png"));
        }

        public void Draw()
        {
            graphics.Clear(Color.Black);

            for (int i = drawableArea.X; i < drawableArea.Width; i++)
            {
                for (int j = drawableArea.Y; j < drawableArea.Height; j++)
                {
                    if (game.Player.Map[i, j] != null && game.Player.Map[i, j].Detected)
                    {
                        //int code = (int)game.Player.Map[i, j].MapObjectType;
                        MapPoint mp = game.Player.Map[i, j];
                        if (mp.MapSector == null && mp.ImageId.HasValue)
                        {
                            int code = game.Player.Map[i, j].ImageId.Value;
                            if (textures.ContainsKey(code))
                            {
                                imgCurr = textures[code];
                                Draw(game.Player.Map[i, j]);
                            }
                        }
                        if (mp.MapSector != null)
                        {
                            int code = game.Player.Map[i, j].MapSector.ImageId;
                            if (textures.ContainsKey(code))
                            {
                                imgCurr = textures[code];
                                Draw(game.Player.Map[i, j]);
                            }
                        }
                    }
                }
            }

            game.GameForm.BigMapControl.GameScene = bmp;
        }

        private void Draw(MapPoint mObject)
        {
            graphics.DrawImage(imgCurr, new Rectangle(((int)mObject.X - drawableArea.X) * cellSizeX,
                ((int)mObject.Y - drawableArea.Y) * cellSizeY, cellSizeX, cellSizeY));
        }

        public void CalculateBounds()
        {
            int distanceX0, distanceY0;
            int distanceX1, distanceY1;

            if (game.Player.King.X - distanceX < 0)
                distanceX0 = 0;
            else distanceX0 = (int)game.Player.King.X - distanceX;

            if (game.Player.King.Y - distanceY < 0)
                distanceY0 = 0;
            else distanceY0 = (int)game.Player.King.Y - distanceY;

            if (game.Player.King.X + distanceX >= game.Player.Map.SizeX)
                distanceX1 = (int)game.Player.Map.SizeX;
            else distanceX1 = distanceX0 + 2 * distanceX;

            if (game.Player.King.Y + distanceY >= game.Player.Map.SizeY)
                distanceY1 = (int)game.Player.Map.SizeY;
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
