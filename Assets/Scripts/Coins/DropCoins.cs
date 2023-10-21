using UnityEngine;

public class DropCoins : MonoBehaviour
{
    [SerializeField] private GameObject _silverCoin;
    [SerializeField] private GameObject _goldCoin;

    [Space]
    [Header("Parametrs")]    
    [SerializeField, Min(0)] private int _maxSizeCoins;
    [SerializeField] private float _departureForce;
    [SerializeField, Range(1, 100)] private int _dipositionThreshold;
    
    
    [ContextMenu("GetCoins")]
    public void GetCoins()
    {
        for (int i = 0; i < _maxSizeCoins; i++)
        {
            if (Random.Range(0, 100) >= _dipositionThreshold)         
                SpawnCoins(_goldCoin);    
            else           
                SpawnCoins(_silverCoin);           
        }         
    }

    private void SpawnCoins(GameObject prefab)
    {
        var coin = Instantiate(prefab, transform.position, transform.rotation);
        coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-45,45), (Random.Range(-45, 45))) * _departureForce);    
    }
}
