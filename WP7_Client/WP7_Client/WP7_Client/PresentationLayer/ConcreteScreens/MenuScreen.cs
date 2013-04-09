using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using WP7_Client.PresentationLayer.Components;
using WP7_Client.PresentationLayer.XNAScreenManager;

namespace WP7_Client.PresentationLayer.ConcreteScreens
{
    public class MenuScreen : GameScreen
    {
        private readonly List<AliveComponent> members = new List<AliveComponent>();
        public MenuScreen()
        {
            var btn = new Button {Text = "Exit",Size = new Vector2(400,100), Position = new Vector2(200,50)};
            btn.Tapped += (sender, args) => ((App) Application.Current).ExitNow();
            members.Add(item: btn);
            EnabledGestures = GestureType.Tap;
            IsPopup = true;
        }

        public override void HandleInput(InputState input)
        {
            foreach (var gesture in input.Gestures)
            {
                foreach (var aliveComponent in members)
                {
                    if (aliveComponent is TappableComponent && gesture.GestureType == GestureType.Tap)
                    {
                        (aliveComponent as TappableComponent).HandleTap(new Vector2 {X = gesture.Position.X, Y = gesture.Position.Y});
                    }
                }
            }
        }

        public override void Draw()
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(Textures.Get(Texture.Blank),new Rectangle(0,0,ScreenWidth,ScreenHeigth),Color.FromNonPremultiplied(0x00,0x00,0x00,0x3A));
            foreach (var aliveComponent in members)
            {
                aliveComponent.Draw(this);
            }
            ScreenManager.SpriteBatch.End();
        }

        public new void Update(bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

        }
    }
}
