using Assets.Scripts.Models;
using Assets.Scripts.Models.Data;
using Assets.Scripts.Models.Difinitions;
using Assets.Scripts.Utils;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.UI.Widgets
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Text _value;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        private int _index;
        

        private void Start()
        {
             _session = FindObjectOfType<GameSession>();
             _session.QuickInventory.SelectedIndex.SubscribeAndInvoke(OnIndexChanged);
        }

        private void OnIndexChanged(int newValue, int _)
        {
           _selection.SetActive(_index == newValue);
        }

        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.Id);
            _icon.sprite = def.Icon;
            _value.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
        }
    }
}