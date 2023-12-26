using System;
using Assets.Scripts.Models.Difinitions.Editor;
using UnityEngine;

namespace Assets.Scripts.Models.Difinitions
{
    [CreateAssetMenu(menuName = "Defs/HealingItems", fileName = "HealingDef")]
    
    public class HealingItemDef : ScriptableObject
    {
        [SerializeField] private HealingDef[] _healingItems;

        public HealingDef Get(string id)
        {
            foreach (var itemDef in _healingItems)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }
    }
    
    [Serializable]
    public struct HealingDef
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private HealingTag _healingTag;

        public string Id => _id;

        public HealingTag HealingTag => _healingTag;
    }
}