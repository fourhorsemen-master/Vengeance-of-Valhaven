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
    private int currentTreeDepth;

    private void Awake()
    {
        spriteWidth = repeatingImage.rectTransform.sizeDelta.x;
        spriteHeight = repeatingImage.rectTransform.sizeDelta.y;
    }

    private void Start()
    {
        player = RoomManager.Instance.Player;
        abilityTimeOutLimit = player.ComboTimeout;

        player.AbilityTree.CurrentDepthSubject.Subscribe(newDepth =>
        {
            repeatingImage.rectTransform.sizeDelta = new Vector2((newDepth) * spriteWidth, spriteHeight);
            repeatingImage.SetOpacity(1f);
            currentTreeDepth = newDepth;
        });
    }

    private void Update()
    {
        float opacity = repeatingImage.color.a;
        
        if(currentTreeDepth > 0)
        {
            repeatingImage.SetOpacity(opacity - (Time.deltaTime / abilityTimeOutLimit));
        }
    }
}
