using UnityEngine;
using System.Collections;
using Logger;
using GameModel;


namespace Assets.MainMenu.Scripts.MenuWindows
{
    public class OptionsMenu : MonoBehaviour, IMenuWindow
    {
        private MainMenuManager _menuManager = null;
        private bool _show = false;
        private Rect _optionWindowRect;

        // Use this for initialization
        void Start()
        {
            _menuManager = GetComponent<MainMenuManager>();
            _optionWindowRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 150, 200, 300);
        }

        void OnGUI()
        {
            if (_show)
                _optionWindowRect = GUILayout.Window(0, _optionWindowRect, OptionsMenuWindow, "Options");
        }

        private void OptionsMenuWindow(int id)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Back"))
            {
                _menuManager.NavigateTo(typeof(MainMenu));
            }
            GUILayout.EndVertical();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show()
        {
            _show = true;
            Log.Debug("Show options menu");
        }

        public void Hide()
        {
            _show = false;
            Log.Debug("Hide options menu");
        }
    }
}
