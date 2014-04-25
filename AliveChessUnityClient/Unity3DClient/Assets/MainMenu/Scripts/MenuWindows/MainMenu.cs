using UnityEngine;
using System.Collections;
using Logger;
using GameModel;

namespace Assets.MainMenu.Scripts.MenuWindows
{
    [RequireComponent(typeof(AudioSource))]
    public class MainMenu : MonoBehaviour, IMenuWindow
    {
        private MainMenuManager _menuManager = null;
        private bool _show = false;
        private Rect _mainWindowRect;

        public AudioSource ClickFX;

        // Use this for initialization
        void Start()
        {
            _menuManager = GetComponent<MainMenuManager>();
            _mainWindowRect = new Rect(Screen.width / 2 + Screen.width / 4 - 175, Screen.height / 2 - 150, 350, 300);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {
            if (_show)
            {
                GUI.skin = _menuManager.skin;
                _mainWindowRect = GUILayout.Window(0, _mainWindowRect, MainMenuWindow, "Alive Chess");
            }
        }

        private void MainMenuWindow(int id)
        {
            GUILayout.BeginVertical();
            if (GameCore.Instance.IsAuthorized)
            {
                if (GUILayout.Button("Start"))
                {
                    ClickFX.Play();
                    Log.Message("Start game");
                    Application.LoadLevel(1);
                    GameCore.Instance.StartGame();
                }
            }
            else
            {
                if (GUILayout.Button("Connect"))
                {
                    ClickFX.Play();
                    _menuManager.AddWindow(typeof(LoginMenu));
                    //GameCore.Instance.Network.Connect("player", "pw");   
                }
            }
            if (GUILayout.Button("Options"))
            {
                ClickFX.Play();
                _menuManager.NavigateTo(typeof(OptionsMenu));
            }
            if (GUILayout.Button("Exit"))
            {
                ClickFX.Play();
                Application.Quit();
            }
            GUILayout.EndVertical();
        }

        public void Show()
        {
            _show = true;
            Log.Debug("Show main menu");
        }

        public void Hide()
        {
            _show = false;
            Log.Debug("Hide main menu");
        }
    }
}
