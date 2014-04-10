using GameModel;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MainMenu.Scripts.MenuWindows
{
    class LoginMenu : MonoBehaviour, IMenuWindow
    {
        private MainMenuManager _menuManager = null;
        private bool _show = false;
        private Rect _loginWindowRect;

        // Use this for initialization
        void Start()
        {
            _menuManager = GetComponent<MainMenuManager>();
            _loginWindowRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100);
        }

        void OnGUI()
        {
            if (_show)
                _loginWindowRect = GUILayout.Window(1, _loginWindowRect, LoginMenuWindow, "Sign in");
        }

        private void LoginMenuWindow(int id)
        {
            string login = "", pass = "";
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Login");
            login = GUILayout.TextField("player");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Password");
            pass = GUILayout.TextField("pw");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Sign in"))
            {
                GameCore.Instance.Network.Connect(login, pass);   
                _menuManager.CloseWindow(typeof(LoginMenu));
            }
            if (GUILayout.Button("Cancel"))
            {
                _menuManager.CloseWindow(typeof(LoginMenu));
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show()
        {
            _show = true;
            Log.Debug("Show login menu");
        }

        public void Hide()
        {
            _show = false;
            Log.Debug("Hide login menu");
        }
    }
}
