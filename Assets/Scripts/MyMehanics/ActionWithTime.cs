using System;
using UnityEngine;
using UnityEngine.Events;

public class ActionWithTime : MonoBehaviour
{
    [SerializeField] private float _spawnDelay;
    [SerializeField] private UnityEvent _action;
    private float _spawnTime;

    private void Update()
    {
        if (Time.time > _spawnTime)
        {
            _spawnTime = Time.time + 3f / _spawnDelay;
            _action?.Invoke();
        }
    }
}

