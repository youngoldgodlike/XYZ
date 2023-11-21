using Assets.Scripts.Models.Difinitions.Editor;
using UnityEngine;

namespace Assets.Scripts.Component.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField]  private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetComponent<Creatures.Hero>();
            if (hero != null)
                hero.AddInInventory(_id, _count);
        }
    }

    
}