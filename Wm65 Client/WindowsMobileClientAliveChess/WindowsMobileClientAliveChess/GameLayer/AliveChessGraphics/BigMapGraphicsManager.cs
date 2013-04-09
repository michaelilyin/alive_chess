using System.Collections.Generic;
using System.Drawing;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Resources;
using System.Windows.Forms;

namespace WindowsMobileClientAliveChess.GameLayer.AliveChessGraphics
{
    public class BigMapGraphicsManager : IGraphicManager
    {
        private Game game;
        private Rectangle drawableArea;

        private Bitmap bmp;
        private Graphics graphics;

        private Image imgCurr;

        private int cellSizeX =30;
        private int cellSizeY = 30;

        private int distanceX;
        private int distanceY;

        private Dictionary<long, Image> textures;
        private Dictionary<ResourceTypes, Image> icos;

        public BigMapGraphicsManager(Game game, Bitmap bitmap)
        {
            this.game = game;
            this.bmp = bitmap;
            this.graphics = Graphics.FromImage(bmp);
            this.textures = new Dictionary<long, Image>();
            this.icos = new Dictionary<ResourceTypes, Image>();
            this.distanceX = 10;
            this.distanceY = 10;
            this.drawableArea.X = 0;
            this.drawableArea.Y = 0;
            this.drawableArea.Width = bitmap.Width/cellSizeX+1;
            this.drawableArea.Height = bitmap.Height / cellSizeY+1;

        }

        public void Load()
        {
            textures.Add(0, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/empty.png"));
            textures.Add(1, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/king.png"));
            textures.Add(2, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/castle.png"));
            textures.Add(3, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/gold_mine.png"));
            textures.Add(4, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/gold.png"));
            textures.Add(5, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/tree.png"));
            textures.Add(6, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/obstacle.png"));
            textures.Add(7, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/ryobi.png"));
            icos.Add(ResourceTypes.Gold, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/gold_ico.png"));
            icos.Add(ResourceTypes.Wood, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/wood_ico.png"));
            icos.Add(ResourceTypes.Iron, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/iron_ico.png"));
            icos.Add(ResourceTypes.Stone, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/stone_ico.png"));
            icos.Add(ResourceTypes.Coal, new Bitmap(LanguageSwitcher.getCurrentPath() + "/Pictures/coal_ico.png"));
        }

        public void Draw()
        {

                graphics.Clear(Color.Black);
                for (int i = drawableArea.X; i < drawableArea.Width+drawableArea.X; i++)
                {
                    for (int j = drawableArea.Y; j < drawableArea.Height+drawableArea.Y; j++)
                    {

                        if (game.Player.Map[i, j] != null)
                        {
                            MapPoint mp = game.Player.Map[i, j];
                            int code = (int)game.Player.Map[i, j].MapPointType;
                            if (textures.ContainsKey(code))
                            {
                                imgCurr = textures[code];
                                Draw(game.Player.Map[i, j]);
                            }
                            if (mp.Owner != null)
                            {
                                int code1 = (int)game.Player.Map[i, j].Owner.Type;
                                if (textures.ContainsKey(code1))
                                {
                                    imgCurr = textures[code1];
                                    Draw(game.Player.Map[i, j]);
                                }
                            }
                        }
                        else
                        {
                            MapPoint empty = new MapPoint();
                            empty.MapPointType = 0;
                            imgCurr = textures[0];
                            empty.X = i;
                            empty.Y = j;
                            Draw(empty);
                        }
                    }
                }

                game.GameForm.BigMapControl.GameScene = bmp;
        }
        private static Image Resize(Image image, Size size)
        {
            Image bmp = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(
                    image,
                    new Rectangle(0, 0, size.Width, size.Height),
                    new Rectangle(0, 0, image.Width, image.Height),
                    GraphicsUnit.Pixel);
            }
            return bmp;
        }
        private void Draw(MapPoint mObject)
        {
            imgCurr = Resize(imgCurr, new Size(cellSizeX, cellSizeY));
            graphics.DrawImage(imgCurr, new Rectangle(((int)mObject.X - drawableArea.X) * cellSizeX,
                ((int)mObject.Y - drawableArea.Y) * cellSizeY,cellSizeX,cellSizeY),new Rectangle(0,0,cellSizeX,cellSizeY), GraphicsUnit.Pixel);
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
            set { drawableArea = value; }
        }

        public void DrawResourceIcon(ResourceTypes rec, int amount, Bitmap place)
        {
            Graphics gr = Graphics.FromImage(place);
            switch (rec)
            {
                case ResourceTypes.Gold:
                    {
                        gr.DrawRectangle(new Pen(Color.White),0,0,place.Width/2,place.Height/3);
                        gr.DrawImage(icos[rec], 0, 0);
                        gr.DrawString(amount.ToString(), default(Font), default(Brush), place.Height / 3, 0);
                        break;
                    }
                case ResourceTypes.Wood:
                    {
                        gr.DrawRectangle(new Pen(Color.White),0,place.Height/3,place.Width/2,place.Height/3);
                        gr.DrawImage(icos[rec], 0, place.Height/3);
                        gr.DrawString(amount.ToString(), default(Font), default(Brush), place.Height / 3, place.Height / 3);
                        break;
                    }
                case ResourceTypes.Stone:
                    {
                        gr.DrawRectangle(new Pen(Color.White),0,2*place.Height/3,place.Width/2,place.Height/3);
                        gr.DrawImage(icos[rec], 0, 2* place.Height / 3);
                        gr.DrawString(amount.ToString(), default(Font), default(Brush), place.Height / 3, 2 * place.Height / 3);
                        break;
                    }
                case ResourceTypes.Iron:
                    {
                        gr.DrawRectangle(new Pen(Color.White),place.Width/2,place.Height/3,place.Width/2,place.Height/3);
                        gr.DrawImage(icos[rec], place.Width/2, place.Height / 3);
                        gr.DrawString(amount.ToString(), default(Font), default(Brush), place.Width/2+ place.Height / 3, place.Height / 3);
                        break;
                    }
                case ResourceTypes.Coal:
                    {
                        gr.DrawRectangle(new Pen(Color.White),place.Width/2,2*place.Height/3,place.Width/2,place.Height/3);
                        gr.DrawImage(icos[rec], place.Width / 2, 2 * place.Height / 3);
                        gr.DrawString(amount.ToString(), default(Font), default(Brush), place.Width/2+ place.Height / 3, 2 * place.Height / 3);
                        break;
                    }
            }
            game.GameForm.BigMapControl.RecourcesPlace = place;
        }
    }

    public delegate void DrawHandler();
}
