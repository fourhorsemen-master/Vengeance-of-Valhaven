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

    private void Start()
    {
        player = RoomManager.Instance.Player;

        RefreshAbilityIcons();
        UpdateCooldown();

        player.ComboManager.SubscribeToStateEntry(ComboState.ReadyAtRoot, RefreshAbilityIcons);
        player.ComboManager.SubscribeToStateEntry(ComboState.ReadyInCombo, RefreshAbilityIcons);
        player.ComboManager.SubscribeToStateEntry(ComboState.LongCooldown, () => ResetCooldown(player.LongCooldown));
        player.ComboManager.SubscribeToStateEntry(ComboState.ShortCooldown, () => ResetCooldown(player.ShortCooldown));
        player.ComboManager.SubscribeToStateEntry(ComboState.Whiff, ShowWhiff);
        player.AbilityFeedbackSubject.Subscribe(ShowFeedback);
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

    private void Update()
    {
        UpdateCooldown();
    }

    private void UpdateCooldown()
    {
        remainingCooldown = Mathf.Max(remainingCooldown - Time.deltaTime, 0f);

        Vector3 newScale = new Vector3(1f, 2 * remainingCooldown / currentCooldownPeriod, 1f);

        leftAbilityCooldown.transform.localScale = newScale;
        rightAbilityCooldown.transform.localScale = newScale;
    }

    private void ResetCooldown(float duration)
    {
        currentCooldownPeriod = duration;
        remainingCooldown = duration;
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
