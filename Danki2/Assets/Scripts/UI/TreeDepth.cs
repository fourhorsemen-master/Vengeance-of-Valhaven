using UnityEngine;
using UnityEngine.UI;

public class TreeDepth : MonoBehaviour
{
    [SerializeField]
    private Image _repeatingImage = null;

    private Player _player = null;

    private float _spriteWidth;
    private float _spriteHeight;

    private void Awake()
    {
        _spriteWidth = _repeatingImage.rectTransform.sizeDelta.x;
        _spriteHeight = _repeatingImage.rectTransform.sizeDelta.y;
    }

    private void Start()
    {
        _player = RoomManager.Instance.Player;
        _player.AbilityTree.CurrentDepthSubject.Subscribe(newDepth =>
        {
            _repeatingImage.rectTransform.sizeDelta = new Vector2(newDepth * _spriteWidth, _spriteHeight);
        });
    }
}
