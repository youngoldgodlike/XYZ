using UnityEngine;

public class CoinsBehavior : MonoBehaviour
{
    
    [SerializeField] private int _valueCoins;   

    public void PickupCoin()
    {
        GameBehavior.instance.ChangeCoinsCount(_valueCoins);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Destroy(GetComponent<Rigidbody2D>());
        }
            
    }
}
