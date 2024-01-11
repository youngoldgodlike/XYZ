using System;
using Assets.Scripts.Models.Data;
using Assets.Scripts.Models.Difinitions;
using Assets.Scripts.UI.HUD.Dialogs;
using UnityEngine;

namespace Assets.Scripts.Component.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;
        private Sprite _sprite;
        
        private DialogBoxController _dialogBox;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>().sprite;
        }
        
        public DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                        break;
                    case  Mode.External:
                        return _external.Data;
                        break;
                    default:
                        throw  new ArgumentOutOfRangeException();
                }
            }
        }

        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }

        public void Show()
        {
            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<DialogBoxController>();
            
            _dialogBox.ShowDialog(Data, _sprite);
        }

        public enum Mode
        {
            Bound,
            External
        }
    }
}
