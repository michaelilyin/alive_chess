using Microsoft.Xna.Framework;
using WP7_Client.PresentationLayer.XNAScreenManager;

namespace WP7_Client.PresentationLayer.Components
{
    public abstract class AliveComponent
    {
        public string Text = "Component";
        public int BorderThickness = 4;
        public Color BorderColor = new Color(200, 200, 200);
        public Color FillColor = new Color(100, 100, 100) * .75f;
        public Color TextColor = Color.White;
        public float Alpha = 1f;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Size = new Vector2(250, 75);

        public abstract void Draw(GameScreen parent);
    }
}
