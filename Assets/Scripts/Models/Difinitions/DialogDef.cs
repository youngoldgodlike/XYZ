using Assets.Scripts.Models.Data;
using UnityEngine;

namespace Assets.Scripts.Models.Difinitions
{
    [CreateAssetMenu(menuName = "Defs/Dialog", fileName =  "Dialog")]
    
    public class DialogDef : ScriptableObject
    {
        [SerializeField] private DialogData _data;
        public DialogData Data => _data;
    }
}