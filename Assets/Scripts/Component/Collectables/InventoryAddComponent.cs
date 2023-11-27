using Assets.Scripts.Extensions;
using Assets.Scripts.Models.Data;
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
            var hero = go.GetInterface<ICanAddInInventory>();
            hero?.AddInInventory(_id, _count);
        }
    }

    
}