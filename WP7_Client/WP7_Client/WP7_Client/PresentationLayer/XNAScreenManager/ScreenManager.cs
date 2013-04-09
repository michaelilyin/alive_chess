using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;
using System.IO.IsolatedStorage;

namespace WP7_Client.PresentationLayer.XNAScreenManager
{
    public class ScreenManager
    {
        #region Fields

        private const string StateFilename = "ScreenManagerState.xml";
        readonly List<GameScreen> _screens = new List<GameScreen>();
        readonly List<GameScreen> _tempScreensList = new List<GameScreen>();

        readonly InputState _input = new InputState();

        #endregion

        #region Properties

        public SpriteBatch SpriteBatch { get; private set; }
        public bool TraceEnabled { get; set; }
        public GamePage Game { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }

        #endregion

        #region Initialization

        public ScreenManager(GamePage game, GraphicsDevice device)
        {
            GraphicsDevice = device;
            Game = game;
            SpriteBatch = Game.SpriteBatch;
            TouchPanel.EnabledGestures = GestureType.None;
            LoadContent();
        }

        protected void LoadContent()
        {
            foreach (var screen in _screens)
            {
                screen.Activate(false);
            }
        }

        protected void UnloadContent()
        {
            foreach (var screen in _screens)
            {
                screen.Unload();
            }
        }


        #endregion

        #region Update and Draw

        public void Update()
        {
            _input.Update();
            _tempScreensList.Clear();

            foreach (var screen in _screens)
                _tempScreensList.Add(screen);

            var otherScreenHasFocus = false;
            var coveredByOtherScreen = false;

            while (_tempScreensList.Count > 0)
            {
                var screen = _tempScreensList[_tempScreensList.Count - 1];
                _tempScreensList.RemoveAt(_tempScreensList.Count - 1);
                screen.Update(otherScreenHasFocus, coveredByOtherScreen);
                if (screen.ScreenState != ScreenState.TransitionOn && screen.ScreenState != ScreenState.Active)
                    continue;
                if (!otherScreenHasFocus)
                {
                    screen.HandleInput(_input);
                    otherScreenHasFocus = true;
                }
                if (!screen.IsPopup)
                    coveredByOtherScreen = true;
            }
            if (TraceEnabled)
                TraceScreens();
        }

        void TraceScreens()
        {
            Debug.WriteLine(string.Join(", ", _screens.Select(screen => screen.GetType().Name).ToArray()));
        }

        public void Draw()
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (var screen in _screens.Where(screen => screen.ScreenState != ScreenState.Hidden))
            {
                screen.Draw();
            }
        }

        #endregion

        #region Public Methods

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;
            screen.Activate(false);
            _screens.Add(screen);

            TouchPanel.EnabledGestures = screen.EnabledGestures;
        }

        public void RemoveScreen(GameScreen screen)
        {
            screen.Unload();
            _screens.Remove(screen);
            _tempScreensList.Remove(screen);
            if (_screens.Count > 0)
            {
                TouchPanel.EnabledGestures = _screens[_screens.Count - 1].EnabledGestures;
            }
        }


        public GameScreen[] GetScreens()
        {
            return _screens.ToArray();
        }

        public void FadeBackBufferToBlack(float alpha)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Textures.Get(Texture.Blank), SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            SpriteBatch.End();
        }

        public void Deactivate()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Create an XML document to hold the list of screen types currently in the stack
                var doc = new XDocument();
                var root = new XElement("ScreenManager");
                doc.Add(root);
                _tempScreensList.Clear();
                foreach (var screen in _screens)
                    _tempScreensList.Add(screen);
                foreach (var screen in _tempScreensList)
                {
                    if (screen.IsSerializable)
                    {
                        var playerValue = screen.ControllingPlayer.HasValue
                            ? screen.ControllingPlayer.Value.ToString()
                            : "";

                        root.Add(new XElement("GameScreen", new XAttribute("Type", screen.GetType().AssemblyQualifiedName), new XAttribute("ControllingPlayer", playerValue)));
                    }
                    screen.Deactivate();
                }

                using (var stream = storage.CreateFile(StateFilename))
                {
                    doc.Save(stream);
                }
            }
        }

        public bool Activate(bool instancePreserved)
        {
            if (instancePreserved)
            {
                _tempScreensList.Clear();

                foreach (var screen in _screens)
                    _tempScreensList.Add(screen);

                foreach (var screen in _tempScreensList)
                    screen.Activate(true);
            }
            else
            {
                var screenFactory = ((App)Application.Current).Services.GetService(typeof(IScreenFactory)) as IScreenFactory;
                if (screenFactory == null)
                {
                    throw new InvalidOperationException(
                        "Game.Services must contain an IScreenFactory in order to activate the ScreenManager.");
                }

                using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!storage.FileExists(StateFilename))
                        return false;
                    using (var stream = storage.OpenFile(StateFilename, FileMode.Open))
                    {
                        var doc = XDocument.Load(stream);
                        if (doc.Root != null)
                            foreach (var screenElem in doc.Root.Elements("GameScreen"))
                            {
                                var xAttribute = screenElem.Attribute("Type");
                                if (xAttribute == null) continue;
                                var screenType = Type.GetType(xAttribute.Value);
                                var screen = screenFactory.CreateScreen(screenType);
                                var attribute = screenElem.Attribute("ControllingPlayer");
                                var xAttribute1 = screenElem.Attribute("ControllingPlayer");
                                if (xAttribute1 != null)
                                {
                                    var controllingPlayer = attribute != null && attribute.Value != ""
                                                                         ? (PlayerIndex)Enum.Parse(typeof(PlayerIndex), xAttribute1.Value, true)
                                                                         : (PlayerIndex?)null;
                                    screen.ControllingPlayer = controllingPlayer;
                                }
                                screen.ScreenManager = this;
                                _screens.Add(screen);
                                screen.Activate(false);
                                TouchPanel.EnabledGestures = screen.EnabledGestures;
                            }
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
