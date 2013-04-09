using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace WP7_Client.PresentationLayer.XNAScreenManager
{
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    public abstract class GameScreen
    {

        protected readonly int ScreenWidth;
        protected readonly int ScreenHeigth;
        public bool IsPopup { get; protected set; }
        public ScreenState ScreenState { get; protected set; }
        public bool IsExiting { get; protected internal set; }
        public ScreenManager ScreenManager { get; internal set; }


        public bool IsActive
        {
            get
            {
                return !_otherScreenHasFocus &&
                       (ScreenState == ScreenState.TransitionOn ||
                        ScreenState == ScreenState.Active);
            }
        }

        bool _otherScreenHasFocus;

        public PlayerIndex? ControllingPlayer { get; internal set; }

        public GestureType EnabledGestures
        {
            get { return _enabledGestures; }
            protected set
            {
                _enabledGestures = value;
                if (ScreenState == ScreenState.Active)
                {
                    TouchPanel.EnabledGestures = value;
                }
            }
        }

        GestureType _enabledGestures = GestureType.None;
        public bool IsSerializable { get; protected set; }

        public GameScreen()
        {
            ScreenWidth = SharedGraphicsDeviceManager.Current.GraphicsDevice.DisplayMode.Height;
            ScreenHeigth = SharedGraphicsDeviceManager.Current.GraphicsDevice.DisplayMode.Width;
            IsSerializable = true;
            IsExiting = false;
            ScreenState = ScreenState.TransitionOn;
            IsPopup = false;
        }


        /// <summary>
        /// Activates the screen. Called when the screen is added to the screen manager or if the game resumes
        /// from being paused or tombstoned.
        /// </summary>
        /// <param name="instancePreserved">
        /// True if the game was preserved during deactivation, false if the screen is just being added or if the game was tombstoned.
        /// On Xbox and Windows this will always be false.
        /// </param>
        public virtual void Activate(bool instancePreserved) { }

        
        /// <summary>
        /// Deactivates the screen. Called when the game is being deactivated due to pausing or tombstoning.
        /// </summary>
        public virtual void Deactivate() { }


        /// <summary>
        /// Unload content for the screen. Called when the screen is removed from the screen manager.
        /// </summary>
        public virtual void Unload() { }


        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void Update(bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _otherScreenHasFocus = otherScreenHasFocus;

            if (IsExiting)
            {
                ScreenState = ScreenState.TransitionOff;
            }
            else if (coveredByOtherScreen)
            {
                ScreenState =  ScreenState.Hidden;
            }
            else
            {
                ScreenState =  ScreenState.Active;
            }
        }



        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public abstract void HandleInput(InputState input);


        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public abstract void Draw();


        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            {
                // Otherwise flag that it should transition off and then exit.
                IsExiting = true;
            }
        }
    }
}
