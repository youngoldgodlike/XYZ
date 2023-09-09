using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior instance;

    [SerializeField] private int _coinsCount = 0;
    public int coinsCount 
    { 
        get => _coinsCount; 
        set 
        { 
            _coinsCount = value;
        } 
    }

    private void Awake()
    {
        instance = this;
    }
    public void ChangeCoinsCount(int value)
    {
        coinsCount += value;
        Debug.Log($"You have {coinsCount} coins");
    }
}
