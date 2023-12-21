namespace Assets.Scripts.Models.Data.Properties
{
    public abstract class PrefsPersistentleProperty<TPropertyType> : PersistentleProperty<TPropertyType>
    {
        protected string Key;
        
        protected PrefsPersistentleProperty(TPropertyType defaultValue, string key) : base(defaultValue)
        {
            Key = key;
        }
        
    }
}