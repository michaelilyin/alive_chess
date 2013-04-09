using System.Collections.Generic;
using Microsoft.Xna.Framework.Input.Touch;

namespace WP7_Client.PresentationLayer.XNAScreenManager
{
    public class InputState
    {
        public const int MaxInputs = 4;
        public TouchCollection TouchState;

        public readonly List<GestureSample> Gestures = new List<GestureSample>();
       
        public void Update()
        {
            TouchState = TouchPanel.GetState();
            Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }
        }
    }
}
