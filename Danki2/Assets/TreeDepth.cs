using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeDepth : MonoBehaviour
{
    [SerializeField]
    private Player _player = null;

    [SerializeField]
    private Image _repeatingImage = null;

    private float _spriteWidth;
    private float _spriteHeight;

    private void Awake()
    {
        _spriteWidth = _repeatingImage.rectTransform.sizeDelta.x;
        _spriteHeight = _repeatingImage.rectTransform.sizeDelta.y;
    }

    private void Start()
    {
        _player.AbilityTree.CurrentDepthSubject.Subscribe(TreeDepthChangeCallback);
    }

    private void TreeDepthChangeCallback(int newDepth)
    {
        _repeatingImage.rectTransform.sizeDelta = new Vector2(newDepth * _spriteWidth, _spriteHeight);
    }
}
