using UnityEngine;

namespace Assets.Scripts.Models.Data.Properties
{
    public class ObservableProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;

        public delegate  void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;
        
        public TPropertyType Value
        {
            get => _value;
            set
            {
                var inSame = _value.Equals(value);
                if(inSame) return;
                var oldValue = _value;

                _value = value;
                OnChanged?.Invoke(_value, oldValue);
            }
        }
       
        
        
    }
}