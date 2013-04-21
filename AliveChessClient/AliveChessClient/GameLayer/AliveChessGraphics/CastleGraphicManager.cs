using System.Drawing;

namespace AliveChessClient.GameLayer.AliveChessGraphics
{
    public class CastleGraphicManager : IGraphicManager
    {
        private Game game;
        private Bitmap bmp;
        private Image img;
        private Graphics graphics;

        public CastleGraphicManager(Game game, Bitmap bitmap)
        {
             this.game = game;
             this.bmp = bitmap;
             this.img = Image.FromFile("Pictures/zamok.jpg");
             this.graphics = Graphics.FromImage(bmp);
        }

        public void Draw()
        {
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height));
            game.GameForm.CastleControl.CastleScene = bmp;
        }
    }
}
