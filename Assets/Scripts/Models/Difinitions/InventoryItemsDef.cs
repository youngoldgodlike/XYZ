﻿using System;
using UnityEngine;

namespace Assets.Scripts.Models.Difinitions
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "ItemDef")]
    
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items ;

        public ItemDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }
        
#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;
        public string Id => _id;
        public bool IsVoid => string.IsNullOrEmpty(_id);
    }
}