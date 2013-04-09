using System;
using Microsoft.Xna.Framework;

namespace WP7_Client.PresentationLayer.Components
{
    public abstract class TappableComponent : AliveComponent
    {
        public event EventHandler<EventArgs> Tapped;

        protected virtual void OnTapped()
        {
            if (Tapped != null)
                Tapped(this, EventArgs.Empty);
        }

        public abstract bool HandleTap(Vector2 tap);
    }
}
