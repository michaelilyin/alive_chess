using UnityEngine;
using System.Collections;
using Logger;
using GameModel;

namespace Assets.MainMenu.Scripts.MenuWindows
{

    public class MainMenu : MonoBehaviour, IMenuWindow
    {
        private MainMenuManager _menuManager = null;
        private bool _show = false;
        private Rect _mainWindowRect;

        // Use this for initialization
        void Start()
        {
            _menuManager = GetComponent<MainMenuManager>();
            _mainWindowRect = new Rect(Screen.width / 2 + Screen.width / 4 - 100, Screen.height / 2 - 150, 200, 300);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {
            if (_show)
                _mainWindowRect = GUILayout.Window(0, _mainWindowRect, MainMenuWindow, "Alive Chess");
        }

        private void MainMenuWindow(int id)
        {
            GUILayout.BeginVertical();
            if (GameCore.Instance.IsAuthorized)
            {
                if (GUILayout.Button("Start"))
                {
                    Log.Message("Start game");
                    Application.LoadLevel(1);
                    GameCore.Instance.StartGame();
                }
            }
            else
            {
                if (GUILayout.Button("Connect"))
                {
                    GameCore.Instance.Network.Connect("player", "pw");   
                }
            }
            if (GUILayout.Button("Options"))
            {
                _menuManager.NavigateTo(typeof(OptionsMenu));
            }
            if (GUILayout.Button("Exit"))
            {
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
