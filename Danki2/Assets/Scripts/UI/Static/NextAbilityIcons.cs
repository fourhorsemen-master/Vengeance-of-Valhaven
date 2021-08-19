using UnityEngine;
using UnityEngine.UI;

public class NextAbilityIcons : MonoBehaviour
{
    private Player player;

    // Image components
    [SerializeField]
    private Image leftAbilityIcon = null;
    [SerializeField]
    private Image leftAbilityFrame = null;
    [SerializeField]
    private Image leftAbilityCooldown = null;

    [SerializeField]
    private Image rightAbilityIcon = null;
    [SerializeField]
    private Image rightAbilityFrame = null;
    [SerializeField]
    private Image rightAbilityCooldown = null;

    // Frame sprites
    [SerializeField]
    private Sprite emptyAbilityFrameSprite = null;
    [SerializeField]
    private Sprite activeAbilityFrameSprite = null;

    private float remainingCooldown = 0f;
    private float currentCooldownPeriod = 1f;
    private bool abilityInProgress = false;
    private bool hasWhiffed;

    private void Start()
    {
        player = ActorCache.Instance.Player;

        RefreshAbilityIcons();
        DisplayCooldown(0);

        player.ComboManager.SubscribeToStateEntry(ComboState.ReadyAtRoot, RefreshAbilityIcons);
        player.ComboManager.SubscribeToStateEntry(ComboState.ReadyInCombo, RefreshAbilityIcons);
        player.ComboManager.SubscribeToStateExit(ComboState.ReadyAtRoot, ShowAbilityInProgress);
        player.ComboManager.SubscribeToStateExit(ComboState.ReadyInCombo, ShowAbilityInProgress);
        player.ComboManager.SubscribeToStateEntry(ComboState.LongCooldown, () => ResetCooldown(player.LongCooldown));
        player.ComboManager.SubscribeToStateEntry(ComboState.ShortCooldown, () => ResetCooldown(player.ShortCooldown));
        player.ComboManager.SubscribeToStateEntry(ComboState.Interrupted, ShowWhiff);
        player.ComboManager.SubscribeToStateEntry(ComboState.ContinueCombo, ShowSuccess);
        player.ComboManager.SubscribeToStateEntry(ComboState.CompleteCombo, ShowSuccess);
        player.AbilityTree.ChangeSubject.Subscribe(RefreshAbilityIcons);
    }

    private void Update()
    {
        if (abilityInProgress) return;

        remainingCooldown = Mathf.Max(remainingCooldown - Time.deltaTime, 0f);

        DisplayCooldown(remainingCooldown / currentCooldownPeriod);
    }

    private void ShowAbilityInProgress()
    {
        abilityInProgress = true;
        DisplayCooldown(1);
    }

    private void ResetCooldown(float duration)
    {
        abilityInProgress = false;
        currentCooldownPeriod = duration;
        remainingCooldown = duration;
    }

    private void DisplayCooldown(float proportion)
    {
        Vector3 newScale = new Vector3(1f, 2 * proportion, 1f);

        leftAbilityCooldown.transform.localScale = newScale;
        rightAbilityCooldown.transform.localScale = newScale;
    }

    private void ShowSuccess()
    {
        if (!hasWhiffed)
        {
            IndicateAbilityCompletion(player.AbilityTree.DirectionLastWalked, true);
        }
    }

    private void ShowWhiff()
    {
        hasWhiffed = true;

        if (leftAbilityIcon.enabled) IndicateAbilityCompletion(Direction.Left, false);
        if (rightAbilityIcon.enabled) IndicateAbilityCompletion(Direction.Right, false);
    }

    private void IndicateAbilityCompletion(Direction direction, bool succeeded)
    {
        Image frame = direction == Direction.Left ? leftAbilityFrame : rightAbilityFrame;
        frame.color = succeeded? Color.green : Color.red;
    }

    private void RefreshAbilityIcons()
    {
        hasWhiffed = false;

        SetSpritesForDirection(Direction.Left, leftAbilityIcon, leftAbilityCooldown, leftAbilityFrame);
        SetSpritesForDirection(Direction.Right, rightAbilityIcon, rightAbilityCooldown, rightAbilityFrame);
    }

    private void SetSpritesForDirection(Direction direction, Image icon, Image cooldown, Image frame)
    {
        frame.color = Color.white;

        if (!player.AbilityTree.CanWalkDirection(direction))
        {
            icon.enabled = false;
            cooldown.enabled = false;
            frame.sprite = emptyAbilityFrameSprite;
            return;
        }

        Ability2 ability = player.AbilityTree.GetAbility(direction);
        Sprite iconSprite = AbilityLookup2.Instance.GetIcon(ability);
        icon.sprite = iconSprite;
        icon.enabled = true;
        cooldown.enabled = true;
        frame.sprite = activeAbilityFrameSprite;
    }
}
