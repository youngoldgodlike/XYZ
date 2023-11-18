﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Models.Difinitions;
using UnityEngine;

namespace Assets.Scripts.Models.Data
{
    [Serializable]
    public class InventoryData 
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;
            
            var item = GetItem(id);
            if (item == null)
            {
                item = new InventoryItemData(id);
                _inventory.Add(item);
            }
            
            item.Value += value;
            
            OnChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int value)
        {
            var item = GetItem(id);
            if (item == null) return;
            
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);
            
            OnChanged?.Invoke(id, Count(id));
        }

        public int Count(string id)
        {
            var count = 0;
            
            foreach (var item in _inventory)
            {
                if (item.Id == id)
                {
                    return  count +=  item.Value;
                }
            }

            return count;
        }
        
        private InventoryItemData GetItem(string id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                    return itemData;
            }

            return null;
        }
        
    }

    [Serializable]
    public class InventoryItemData
    {
        public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}

