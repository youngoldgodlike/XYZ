using Assets.Scripts.Models;
using Assets.Scripts.Models.Difinitions.Editor;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InventoryInformationBar : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _tag;
        
        private TextMeshProUGUI _text;
        private GameSession _gameSession;

        private void OnEnable() => Creatures.Hero.OnAddInInventory += ChangeInformation;
        private void OnDestroy() => Creatures.Hero.OnAddInInventory -= ChangeInformation;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _gameSession = FindObjectOfType<GameSession>();
        }

        private void Start()
        {
            ChangeInformation();
        }
        
        private void ChangeInformation()
        {
            var count = _gameSession.Data.Inventory.Count(_tag).ToString();
            _text.text = count;
        }
    }
}
