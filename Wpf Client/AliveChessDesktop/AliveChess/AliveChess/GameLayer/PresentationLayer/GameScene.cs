using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AliveChess.GameLayer.PresentationLayer
{
    public class GameScene : Page
    {
        public GameScene()
        {
            GameCore.Instance.WindowContext.AttachPage(this);
        }

        protected virtual void MoveTo(Uri uri)
        {
            GameCore.Instance.WindowContext.DetachPage(this);
        }
    }
}
