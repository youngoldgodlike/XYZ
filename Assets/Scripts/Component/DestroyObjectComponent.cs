using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] GameObject _objectToDestroy;
        public void DestroyObject()
        {
            if (_objectToDestroy != null)           
                Destroy(_objectToDestroy);
            else
                Destroy(gameObject);
            
        }
    }
}
