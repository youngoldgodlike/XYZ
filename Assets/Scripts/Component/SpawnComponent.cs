using UnityEngine;

namespace Assets.Scripts.Component
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject[] _prefabs;
        
        [Header("Destroy")]
        [SerializeField] private bool _isDestroy;
        [SerializeField] private float _destroyTime = 3f;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            for (int i = 0; i < _prefabs.Length; i++)
            {
                var prefab = Instantiate(_prefabs[i], _target.position, Quaternion.identity);
                prefab.transform.localScale = _target.lossyScale;
                
                if (_isDestroy)
                    Destroy(prefab, _destroyTime);
                
            }           
        }
    }
}
