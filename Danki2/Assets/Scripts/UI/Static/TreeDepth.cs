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
        this.spriteWidth = this.repeatingImage.rectTransform.sizeDelta.x;
        this.spriteHeight = this.repeatingImage.rectTransform.sizeDelta.y;
    }

    private void Start()
    {
        this.player = RoomManager.Instance.Player;
        this.abilityTimeOutLimit = this.player.ComboTimeout;

        this.player.AbilityTree.CurrentDepthSubject.Subscribe(newDepth =>
        {
            this.repeatingImage.rectTransform.sizeDelta = new Vector2((newDepth) * this.spriteWidth, this.spriteHeight);
            this.repeatingImage.SetOpacity(1f);
            this.currentTreeDepth = newDepth;
        });
    }

    private void Update()
    {
        float opacity = this.repeatingImage.color.a;
        
        if(this.currentTreeDepth > 0)
        {
            this.repeatingImage.SetOpacity(opacity - (Time.deltaTime / abilityTimeOutLimit));
        }
    }
}
