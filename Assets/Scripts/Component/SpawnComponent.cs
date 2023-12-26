using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Component
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private List<GameObject> _prefabs;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            foreach (var prefab in _prefabs)
            {
                var go = Instantiate(prefab, _target.position, Quaternion.identity);
                go.transform.localScale = _target.lossyScale;
            }
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefabs.Clear();
            _prefabs.Add(prefab);
        }
    }
}
