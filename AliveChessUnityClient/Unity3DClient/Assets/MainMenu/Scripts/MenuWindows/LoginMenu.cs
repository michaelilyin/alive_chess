using GameModel;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MainMenu.Scripts.MenuWindows
{
    [RequireComponent(typeof(AudioSource))]
    class LoginMenu : MonoBehaviour, IMenuWindow
    {
        private MainMenuManager _menuManager = null;
        private bool _show = false;
        private Rect _loginWindowRect;

        public AudioSource ClickFX;

        // Use this for initialization
        void Start()
        {
            _menuManager = GetComponent<MainMenuManager>();
            _loginWindowRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 200);
        }

        void OnGUI()
        {
            if (_show)
            {
                GUI.skin = _menuManager.skin;
                _loginWindowRect = GUILayout.Window(1, _loginWindowRect, LoginMenuWindow, "Sign in");
            }
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
            pass = GUILayout.PasswordField("pw", '*');
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Sign in"))
            {
                ClickFX.Play();
                GameCore.Instance.Network.Connect(login, pass);   
                _menuManager.CloseWindow(typeof(LoginMenu));
            }
            if (GUILayout.Button("Cancel"))
            {
                ClickFX.Play();
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
