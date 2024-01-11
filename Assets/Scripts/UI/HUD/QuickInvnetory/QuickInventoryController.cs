using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Data;
using Assets.Scripts.UI.Widgets;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.HUD.QuickInvnetory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;

       private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;
        private List<InventoryItemWidget> _createdItems = new List<InventoryItemWidget>(); 
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _trash .Retain(_session.QuickInventory.Subscribe(Rebuild));

            Rebuild();
        }

        private void Rebuild()
        {
            var _inventory = _session.QuickInventory.Inventory;

            for (var i = _createdItems.Count; i < _inventory.Length; i++)
            {
                var item = Instantiate(_prefab, _container);
                _createdItems.Add(item);
            }

            for (var i = 0; i < _inventory.Length; i++)
            {
                _createdItems[i].SetData(_inventory[i], i);
                _createdItems[i].gameObject.SetActive(true);
            }

            for (var i = _inventory.Length; i < _createdItems.Count(); i++)
            {
                _createdItems[i].gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }

    
}