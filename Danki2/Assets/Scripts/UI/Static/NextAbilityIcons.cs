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

    private void Start()
    {
        player = RoomManager.Instance.Player;
        player.AbilityTree.TreeWalkSubject.Subscribe(TreeWalkCallback);
        player.AbilityManager.AbilityCompletionSubject.Subscribe(IndicateAbilityCompletion);

        UpdateCooldown(0f);
    }

    private void Update()
    {
        UpdateCooldown(player.AbilityManager.RemainingCooldownProportion);

    }

    private void UpdateCooldown(float remainingCooldownProportion)
    {
        leftAbilityCooldown.transform.localScale = new Vector3(1f, 2 * remainingCooldownProportion, 1f);
        rightAbilityCooldown.transform.localScale = new Vector3(1f, 2 * remainingCooldownProportion, 1f);
    }

    private void IndicateAbilityCompletion(Tuple<bool, Direction> successDirectionTuple)
    {
        (bool success, Direction direction) = successDirectionTuple;

        Image frame = direction == Direction.Left ? leftAbilityFrame : rightAbilityFrame;
        frame.color = success ? Color.green : Color.red;
    }

    private void TreeWalkCallback(Node node)
    {
        SetSpritesForDirection(node, Direction.Left, leftAbilityIcon, leftAbilityCooldown, leftAbilityFrame);
        SetSpritesForDirection(node, Direction.Right, rightAbilityIcon, rightAbilityCooldown, rightAbilityFrame);
    }

    private void SetSpritesForDirection(Node node, Direction direction, Image icon, Image cooldown, Image frame)
    {
        frame.color = Color.white;

        if (!node.HasChild(direction))
        {
            icon.enabled = false;
            cooldown.enabled = false;
            frame.sprite = emptyAbilityFrameSprite;
            return;
        }

        AbilityReference ability = node.GetChild(direction).Ability;
        Sprite iconSprite = AbilityIconManager.Instance.GetIcon(ability);
        icon.sprite = iconSprite;
        icon.enabled = true;
        cooldown.enabled = true;
        frame.sprite = activeAbilityFrameSprite;
    }
}
