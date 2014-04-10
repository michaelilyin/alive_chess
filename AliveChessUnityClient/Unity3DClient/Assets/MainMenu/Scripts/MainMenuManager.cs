using UnityEngine;
using System.Collections;
using Assets.MainMenu.Scripts.MenuWindows;
using Logger;
using System.Collections.Generic;
using System;
using GameModel;

public class MainMenuManager : MonoBehaviour
{

    private IMenuWindow _currentWindow = null;
    private Dictionary<Type, IMenuWindow> _windows;

    void Awake()
    {
        Log.SetLevel(LogLevel.ALL);
        Log.Message("Application started");
    }

    void Start()
    {
        _windows = new Dictionary<Type, IMenuWindow>();
        _windows[typeof(MainMenu)] = GetComponent<MainMenu>();
        _windows[typeof(OptionsMenu)] = GetComponent<OptionsMenu>();
        _windows[typeof(LoginMenu)] = GetComponent<LoginMenu>();
        NavigateTo(typeof(MainMenu));
    }

    void Update()
    {

    }

    public void NavigateTo(Type windowType)
    {
        //if (_currentWindow != null) _currentWindow.Hide();
        foreach (var w in _windows)
        {
            w.Value.Hide();
        }
        _windows[windowType].Show();
        _currentWindow = _windows[windowType];

    }

    public void AddWindow(Type windowType)
    {
        _windows[windowType].Show();
    }

    public void CloseWindow(Type windowType)
    {
        _windows[windowType].Hide();
    }
}
