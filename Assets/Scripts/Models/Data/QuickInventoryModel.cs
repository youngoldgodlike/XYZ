using System;
using Assets.Scripts.Models.Data.Properties;
using Assets.Scripts.Models.Difinitions;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Models.Data
{
    public class QuickInventoryModel : IDisposable
    {
        private readonly PlayerData _data;
        
        public InventoryItemData[] Inventory { get; private set; }

        public readonly IntProperty SelectedIndex = new IntProperty();

        public InventoryItemData SelectedItem => Inventory[SelectedIndex.Value];

        public event Action OnChanged;
        
        public QuickInventoryModel(PlayerData data)
        {
            _data = data;

            Inventory = data.Inventory.GetAll(ItemTag.Usable);
            _data.Inventory.OnChanged += OnChangedInventory;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        private void OnChangedInventory(string id, int value)
        {
            Inventory = _data.Inventory.GetAll(ItemTag.Usable);
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            OnChanged?.Invoke();
        }

        public void SetNextItem()
        {
            SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
        }
        

        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;
        }
    }
}