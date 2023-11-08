﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CheatControlller : MonoBehaviour
{
    [SerializeField] private CheatItem[] _cheats;
    private string _currentInput;
    [SerializeField] private float _inputTimeToLive;

    private float _inputTime;
    
    private void Awake()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDestroy()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    private void OnTextInput(char inputChar)
    {
        _currentInput += inputChar;
        _inputTime = _inputTimeToLive;
        FindAnyCheats();
    }

    private void FindAnyCheats()
    {
        foreach (var cheatItem in _cheats)
        {
            if (_currentInput.Contains(cheatItem.Name))
            {
                cheatItem.Action.Invoke();
                _currentInput = string.Empty;
            }
        }
    }

    private void Update()
    {
        if (_inputTime < 0)
        {
            _currentInput = string.Empty;
        }
        else
        {
            _inputTime -= Time.deltaTime;
        }
    }
}

[Serializable]
public class CheatItem
{
    public string Name;
    public UnityEvent Action;
}

