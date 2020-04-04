using UnityEngine;
using UnityEngine.UI;

public class NextAbilityIcons : MonoBehaviour
{
    private Player _player;

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
        _player = RoomManager.Instance.Player;
        _player.SubscribeToTreeWalk(TreeWalkCallback);
    }

    private void TreeWalkCallback(Node node)
    {
        SetSpritesForDirection(node, Direction.Left, _leftAbilityIcon, _leftAbilityFrame);
        SetSpritesForDirection(node, Direction.Right, _rightAbilityIcon, _rightAbilityFrame);
    }

    private void SetSpritesForDirection(Node node, Direction direction, Image icon, Image frame)
    {
        if (!node.HasChild(direction))
        {
            icon.enabled = false;
            frame.sprite = _emptyAbilityFrameSprite;
            return;
        }

        AbilityReference ability = node.GetChild(direction).Ability.AbilityReference;
        Sprite iconSprite = AbilityIconManager.Instance.GetIcon(ability);
        icon.sprite = iconSprite;
        icon.enabled = true;
        frame.sprite = _activeAbilityFrameSprite;
    }
}
