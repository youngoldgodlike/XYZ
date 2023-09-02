using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Hero : MonoBehaviour
{
   [SerializeField] private float _speed;
    private float _directionX;
    private float _directionY;
    
    public void SetDirectionX(float directionX)
    {
        _directionX = directionX;     
    }

    public void SetDirectionY(float directionY)
    {
        _directionY = directionY;
    }

    private void Update()
    {
        if (_directionX != 0)
        {
            var delta = _directionX * _speed * Time.deltaTime;
            var newXPosition = transform.position.x + delta;
            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        }
        if (_directionY != 0) 
        {
            var delta = _directionY * _speed * Time.deltaTime;
            var newYPosition = transform.position.y + delta;
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        }
    }

    public void SaySomething()
    {
        Debug.Log("Something!");
    }
}
