using System;
using Assets.Scripts.Models.Difinitions.Editor;
using UnityEngine;

namespace Assets.Scripts.Models.Difinitions
{
    [CreateAssetMenu(menuName = "Defs/ThrowableItems", fileName = "ThrowableDef")]
    
    public class ThrowableItemDef : ScriptableObject
    {
        [SerializeField] private ThrowableDef[] _items ;

        public ThrowableDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }
    }

    [Serializable]
    public struct ThrowableDef
    {
       [InventoryId] [SerializeField] private string _id;
       [SerializeField] private GameObject _projectile;
       
       public string Id =>_id;
       public GameObject Projectile => _projectile;
    }
}