using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private int _frameRate;
    [SerializeField] private string[] _targetClipNames;   
    [SerializeField] public Clips[] _clips;


    private SpriteRenderer _renderer;
    private float _secondsPerFrame;
    private int _currentSpriteIndex;
    private float _nextFrameTime;
    private bool _isPlaying = true;
    private int _clipsCount = 0;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _secondsPerFrame = 1f / _frameRate;
        _nextFrameTime = Time.time + _secondsPerFrame;
    }

    private void Update()
    {
        SetClip(_targetClipNames[_clipsCount]);
    }

    private void SetClip(string clipName)
    {
        foreach (var item in _clips)
        {
            if (item.name == clipName)
            {                                 
                
                if (!_isPlaying || _nextFrameTime > Time.time) return;

                if (_currentSpriteIndex >= item.sprites.Length)
                {
                    if (item.loop)
                    {
                        _currentSpriteIndex = 0;
                        if (item.allowNext == true)
                        {
                            _clipsCount++;                          
                            return;
                        }                          
                    }
                    else
                    {
                        _isPlaying = false;
                        item.onComplete?.Invoke();
                        return;
                    }
                }
                _renderer.sprite = item.sprites[_currentSpriteIndex];
                _nextFrameTime += _secondsPerFrame;
                _currentSpriteIndex++;
            }
        }
    }
}

[Serializable]
public struct Clips
{
    public string name;
    public Sprite[] sprites;
    public bool allowNext;
    public bool loop;
    public UnityEvent onComplete;

}
