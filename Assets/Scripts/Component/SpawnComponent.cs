using UnityEngine;

namespace Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject[] _prefabs;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            for (int i = 0; i < _prefabs.Length; i++)
            {
                var prefab = Instantiate(_prefabs[i], _target.position, Quaternion.identity);
                prefab.transform.localScale = _target.lossyScale;
                Destroy(prefab, 3f);
            }           
        }
    }
}
