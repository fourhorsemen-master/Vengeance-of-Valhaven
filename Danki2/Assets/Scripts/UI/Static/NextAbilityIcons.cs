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

    private float totalCooldownDuration = 1;
    private float remainingCooldownDuration = 0;

    private void Start()
    {
        player = RoomManager.Instance.Player;

        UpdateAbilityIcons();

        player.ComboCompleteSubject.Subscribe(() =>
            ResetCooldown(player.LongCooldown)
        );
        player.ComboContinueSubject.Subscribe(() =>
            ResetCooldown(player.ShortCooldown)
        );
        player.ComboFailedSubject.Subscribe(() =>
            ResetCooldown(player.LongCooldown)
        );
        player.WhiffSubject.Subscribe(() => {
            ResetCooldown(player.LongCooldown);
            IndicateWhiff();
        });

        player.AbilityTree.ChangeSubject.Subscribe(UpdateAbilityIcons);
        player.ReadyToCastSubject.Subscribe(UpdateAbilityIcons);
        player.AbilityFeedbackSubject.Subscribe(IndicateAbilityCompletion);
    }

    private void Update()
    {
        UpdateCooldown();
    }

    private void ResetCooldown(float duration)
    {
        totalCooldownDuration = duration;
        remainingCooldownDuration = duration;
    }

    private void UpdateCooldown()
    {
        remainingCooldownDuration = Math.Max(0f, remainingCooldownDuration - Time.deltaTime);

        Vector3 newScale = new Vector3(1f, 2 * remainingCooldownDuration / totalCooldownDuration, 1f);
        leftAbilityCooldown.transform.localScale = newScale;
        rightAbilityCooldown.transform.localScale = newScale;
    }

    private void IndicateAbilityCompletion(bool feedback)
    {
        Image frame = player.LastCastDirection == Direction.Left ? leftAbilityFrame : rightAbilityFrame;
        frame.color = feedback ? Color.green : Color.red;
    }

    private void IndicateWhiff()
    {
        if (leftAbilityIcon.enabled) leftAbilityFrame.color = Color.red;
        if (rightAbilityIcon.enabled) rightAbilityFrame.color = Color.red;
    }

    private void UpdateAbilityIcons()
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
