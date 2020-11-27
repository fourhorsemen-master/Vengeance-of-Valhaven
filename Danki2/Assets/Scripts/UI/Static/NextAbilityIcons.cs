using System;
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

    private void Start()
    {
        player = RoomManager.Instance.Player;

        RefreshAbilityIcons();
        DisplayCooldown(0);

        player.ComboManager.SubscribeToStateEntry(ComboState.ReadyAtRoot, RefreshAbilityIcons);
        player.ComboManager.SubscribeToStateEntry(ComboState.ReadyInCombo, RefreshAbilityIcons);
        player.ComboManager.SubscribeToStateExit(ComboState.ReadyAtRoot, ShowAbilityInProgres);
        player.ComboManager.SubscribeToStateExit(ComboState.ReadyInCombo, ShowAbilityInProgres);
        player.ComboManager.SubscribeToStateEntry(ComboState.LongCooldown, () => ResetCooldown(player.LongCooldown));
        player.ComboManager.SubscribeToStateEntry(ComboState.ShortCooldown, () => ResetCooldown(player.ShortCooldown));
        player.ComboManager.SubscribeToStateEntry(ComboState.Whiff, ShowWhiff);

        player.AbilityFeedbackSubject.Subscribe(ShowFeedback);
        player.AbilityTree.ChangeSubject.Subscribe(RefreshAbilityIcons);
    }

    private void Update()
    {
        if (abilityInProgress) return;

        remainingCooldown = Mathf.Max(remainingCooldown - Time.deltaTime, 0f);

        DisplayCooldown(remainingCooldown / currentCooldownPeriod);
    }

    private void ShowAbilityInProgres()
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

    public void DisplayCooldown(float proportion)
    {
        Vector3 newScale = new Vector3(1f, 2 * proportion, 1f);

        leftAbilityCooldown.transform.localScale = newScale;
        rightAbilityCooldown.transform.localScale = newScale;
    }

    private void ShowFeedback(bool result)
    {
        IndicateAbilityCompletion(player.AbilityTree.DirectionLastWalked, result);
    }

    private void ShowWhiff()
    {
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

        AbilityReference ability = player.AbilityTree.GetAbility(direction);
        Sprite iconSprite = AbilityIconManager.Instance.GetIcon(ability);
        icon.sprite = iconSprite;
        icon.enabled = true;
        cooldown.enabled = true;
        frame.sprite = activeAbilityFrameSprite;
    }
}
