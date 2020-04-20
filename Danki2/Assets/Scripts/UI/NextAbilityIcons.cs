using System;
using UnityEngine;
using UnityEngine.UI;

public class NextAbilityIcons : MonoBehaviour
{
    private Player player;

    // Image components
    [SerializeField]
    private Image _leftAbilityIcon = null;
    [SerializeField]
    private Image _leftAbilityFrame = null;
    [SerializeField]
    private Image _rightAbilityIcon = null;
    [SerializeField]
    private Image _rightAbilityFrame = null;

    // Frame sprites
    [SerializeField]
    private Sprite _emptyAbilityFrameSprite = null;
    [SerializeField]
    private Sprite _activeAbilityFrameSprite = null;

    private void Start()
    {
        player = RoomManager.Instance.Player;
        player.AbilityTree.TreeWalkSubject.Subscribe(TreeWalkCallback);
        player.AbilityCompletionSubject.Subscribe(IndicateAbilityCompletion);
    }

    private void IndicateAbilityCompletion(Tuple<bool, Direction> successDirectionTuple)
    {
        (bool success, Direction direction) = successDirectionTuple;

        Image icon = direction == Direction.Left ? _leftAbilityFrame : _rightAbilityFrame;
        icon.color = success ? Color.green : Color.red;
    }

    private void TreeWalkCallback(Node node)
    {
        SetSpritesForDirection(node, Direction.Left, _leftAbilityIcon, _leftAbilityFrame);
        SetSpritesForDirection(node, Direction.Right, _rightAbilityIcon, _rightAbilityFrame);
    }

    private void SetSpritesForDirection(Node node, Direction direction, Image icon, Image frame)
    {
        frame.color = Color.white;

        if (!node.HasChild(direction))
        {
            icon.enabled = false;
            frame.sprite = _emptyAbilityFrameSprite;
            return;
        }

        AbilityReference ability = node.GetChild(direction).Ability;
        Sprite iconSprite = AbilityIconManager.Instance.GetIcon(ability);
        icon.sprite = iconSprite;
        icon.enabled = true;
        frame.sprite = _activeAbilityFrameSprite;
    }
}
