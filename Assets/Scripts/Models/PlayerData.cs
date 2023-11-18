using System;
using Assets.Scripts.Models.Data;
using UnityEngine;

[Serializable]
public class PlayerData : MonoBehaviour
{
    public int Hp;
    
    [SerializeField] private InventoryData _inventory;
    public InventoryData Inventory => _inventory;
}
