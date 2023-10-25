using Assets.Scripts.Models;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior instance;   
   [SerializeField] private GameSession _session;

    public int coinsCount 
    { 
        get => _session.Data.Coins; 
        set 
        {
            _session.Data.Coins = value;
        } 
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
    }
    public void ChangeCoinsCount(int value)
    {
        coinsCount += value;
        Debug.Log($"You have {_session.Data.Coins} coins");
    }
}
