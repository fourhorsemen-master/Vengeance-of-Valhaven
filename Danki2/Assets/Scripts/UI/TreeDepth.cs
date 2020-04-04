using UnityEngine;
using UnityEngine.UI;

public class TreeDepth : MonoBehaviour
{
    [SerializeField]
    private Image _repeatingImage = null;

    private Player _player = null;

    private float _spriteWidth;
    private float _spriteHeight;
    private float abilityTimeOutLimit;
    private int currentTreeDepth;

    private void Awake()
    {
        _spriteWidth = _repeatingImage.rectTransform.sizeDelta.x;
        _spriteHeight = _repeatingImage.rectTransform.sizeDelta.y;
    }

    private void Start()
    {
        _player = RoomManager.Instance.Player;
        this.abilityTimeOutLimit = _player.abilityTimeoutLimit;

        _player.AbilityTree.CurrentDepthSubject.Subscribe(newDepth =>
        {
            _repeatingImage.rectTransform.sizeDelta = new Vector2((newDepth) * _spriteWidth, _spriteHeight);
            _repeatingImage.SetOpacity(1f);
            this.currentTreeDepth = newDepth;
        });
    }

    private void Update()
    {
        float opacity = _repeatingImage.color.a;
        
        if(this.currentTreeDepth > 0)
        {
            _repeatingImage.SetOpacity(opacity - (Time.deltaTime / abilityTimeOutLimit));
        }
    }
}
