using UnityEngine;

public class CoinsBehavior : MonoBehaviour
{
    
    [SerializeField] private int _valueCoins;

    public void PickupCoin()
    {
        GameBehavior.instance.ChangeCoinsCount(_valueCoins);
    }
}
