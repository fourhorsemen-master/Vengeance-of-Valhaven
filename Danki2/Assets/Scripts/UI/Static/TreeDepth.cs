using UnityEngine;
using UnityEngine.UI;

public class TreeDepth : MonoBehaviour
{
    [SerializeField]
    private Image repeatingImage = null;

    private Player player;

    private float spriteWidth;
    private float spriteHeight;
    private float abilityTimeOutLimit;

    private void Awake()
    {
        spriteWidth = repeatingImage.rectTransform.sizeDelta.x;
        spriteHeight = repeatingImage.rectTransform.sizeDelta.y;
    }

    private void Start()
    {
        player = RoomManager.Instance.Player;
        abilityTimeOutLimit = player.ComboTimeout;

        UpdateDepth();
        player.ReadyToCastSubject.Subscribe(UpdateDepth);
    }

    private void Update()
    {
        float opacity = repeatingImage.color.a;
        
        repeatingImage.SetOpacity(opacity - (Time.deltaTime / abilityTimeOutLimit));
    }

    private void UpdateDepth()
    {
        int depth = player.AbilityTree.CurrentDepth;
        repeatingImage.rectTransform.sizeDelta = new Vector2(depth * spriteWidth, spriteHeight);
        repeatingImage.SetOpacity(1f);
    }
}
