﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class HeroInputReader : MonoBehaviour
{
   [SerializeField] private Hero _hero;

    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<float>();
        _hero.SetDirectionX(direction);
    }

    public void OnVerticalMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<float>();
        _hero.SetDirectionY(direction);
    }

    public void OnSaySomething(InputAction.CallbackContext context)
    {
        if (context.canceled)
          _hero.SaySomething();
    }
}



