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
        NavigateTo(typeof(MainMenu));
    }

    void Update()
    {

    }

    public void NavigateTo(Type windowType)
    {
        if (_currentWindow != null) _currentWindow.Hide();
        _windows[windowType].Show();
        _currentWindow = _windows[windowType];

    }
}
