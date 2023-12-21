using System;
using Assets.Scripts.Models.Data;
using Assets.Scripts.Models.Data.Properties;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class PlayerData : MonoBehaviour
    {
        public IntProperty Hp = new IntProperty();
    
        [SerializeField] private InventoryData _inventory;
        public InventoryData Inventory => _inventory;
    }
}
