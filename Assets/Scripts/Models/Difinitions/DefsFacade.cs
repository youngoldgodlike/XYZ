using UnityEngine;

namespace Assets.Scripts.Models.Difinitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;
        [SerializeField] private ThrowableItemDef _throwableItems;
        [SerializeField] private HealingItemDef _healingItems;
        [SerializeField] private PlayerDef _player;

        private static DefsFacade _instance;

        public HealingItemDef HealingItems => _healingItems;
        public PlayerDef Player => _player;
        public InventoryItemsDef Items => _items;
        public ThrowableItemDef ThrowableItems => _throwableItems;

        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }

        
    }
}