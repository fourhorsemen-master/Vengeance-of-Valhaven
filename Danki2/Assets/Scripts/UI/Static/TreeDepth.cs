using UnityEngine;
using UnityEngine.UI;

public class TreeDepth : MonoBehaviour
{
    [SerializeField]
    private Image repeatingImage = null;

    private Player player;

    private float spriteWidth;
    private float spriteHeight;
    private bool fading;

    private void Awake()
    {
        spriteWidth = repeatingImage.rectTransform.sizeDelta.x;
        spriteHeight = repeatingImage.rectTransform.sizeDelta.y;
    }

    private void Start()
    {
        player = ActorCache.Instance.Player;

        UpdateDepth();

        player.ComboManager.SubscribeToStateEntry(ComboState.ShortCooldown, UpdateDepth);
        player.ComboManager.SubscribeToStateEntry(ComboState.LongCooldown, UpdateDepth);
        player.ComboManager.SubscribeToStateEntry(ComboState.ReadyInCombo, StartFade);
        player.ComboManager.SubscribeToStateExit(ComboState.ReadyInCombo, StopFade);
    }

    private void Update()
    {
        float opacity = repeatingImage.color.a;

        if (fading)
        {
            repeatingImage.SetOpacity(opacity - (Time.deltaTime / player.ComboTimeout));
        }
    }

    private void UpdateDepth()
    {
        int depth = player.AbilityTree.CurrentDepth;

        repeatingImage.rectTransform.sizeDelta = new Vector2(depth * spriteWidth, spriteHeight);
        repeatingImage.SetOpacity(1f);
    }

    private void StartFade()
    {
        fading = true;
    }

    private void StopFade()
    {
        repeatingImage.SetOpacity(1f);
        fading = false;
    }
}
