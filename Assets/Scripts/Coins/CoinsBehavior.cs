using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsBehavior : MonoBehaviour
{
    [SerializeField] private GameBehavior _gameBehavior;
    [SerializeField] private int _valueCoins;

    public void PickupCoin()
    {
        _gameBehavior.ChangeCoinsCount(_valueCoins);
    }
}
