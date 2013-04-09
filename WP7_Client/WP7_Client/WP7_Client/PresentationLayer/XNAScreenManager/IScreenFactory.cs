using System;

namespace WP7_Client.PresentationLayer.XNAScreenManager
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
