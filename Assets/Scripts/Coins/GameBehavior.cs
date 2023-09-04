using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    [SerializeField] private int _coinsCount = 0;
    public int coinsCount 
    { 
        get => _coinsCount; 
        set 
        { 
            _coinsCount = value;
        } 
    }

    public void ChangeCoinsCount(int value)
    {
        coinsCount += value;
        Debug.Log($"You have {coinsCount} coins");
    }
}
