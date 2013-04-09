using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WP7_Client.PresentationLayer.XNAScreenManager;
using Texture = WP7_Client.PresentationLayer.XNAScreenManager.Texture;

namespace WP7_Client.PresentationLayer.Components
{
    class Button : TappableComponent
    {
        public override bool HandleTap(Vector2 tap)
        {
            if (tap.X >= Position.X &&
                tap.Y >= Position.Y &&
                tap.X <= Position.X + Size.X &&
                tap.Y <= Position.Y + Size.Y)
            {
                OnTapped();
                return true;
            }

            return false;
        }

        public override void Draw(GameScreen parent)
        {
            var spriteBatch = parent.ScreenManager.SpriteBatch;
            var font = parent.ScreenManager.Game.ContentManager.Load<SpriteFont>("basic");
            var blank = Textures.Get(Texture.Blank);
            var r = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)Size.X,
                (int)Size.Y);
            spriteBatch.Draw(blank, r, FillColor * Alpha);
            spriteBatch.Draw(
                blank,
                new Rectangle(r.Left, r.Top, r.Width, BorderThickness),
                BorderColor * Alpha);
            spriteBatch.Draw(
                blank,
                new Rectangle(r.Left, r.Top, BorderThickness, r.Height),
                BorderColor * Alpha);
            spriteBatch.Draw(
                blank,
                new Rectangle(r.Right - BorderThickness, r.Top, BorderThickness, r.Height),
                BorderColor * Alpha);
            spriteBatch.Draw(
                blank,
                new Rectangle(r.Left, r.Bottom - BorderThickness, r.Width, BorderThickness),
                BorderColor * Alpha);

            var textSize = font.MeasureString(Text);
            var textPosition = new Vector2(r.Center.X, r.Center.Y) - textSize / 2f;
            textPosition.X = (int)textPosition.X;
            textPosition.Y = (int)textPosition.Y;
            spriteBatch.DrawString(font, Text, textPosition, TextColor * Alpha);
        }
    }
}
