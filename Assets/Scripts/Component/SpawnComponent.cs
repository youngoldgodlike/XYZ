using UnityEngine;

namespace Assets.Scripts.Component
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject[] _prefabs;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            foreach (var prefab in _prefabs)
            {
                var go = Instantiate(prefab, _target.position, Quaternion.identity);
                go.transform.localScale = _target.lossyScale;
            }
        }
    }
}
