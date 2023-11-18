using UnityEngine;

namespace Assets.Scripts.Models.Difinitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;

        private static DefsFacade _instance;
        
        public InventoryItemsDef Items => _items;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }

        
    }
}