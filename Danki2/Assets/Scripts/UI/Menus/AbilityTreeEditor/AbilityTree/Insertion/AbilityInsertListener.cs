using UnityEngine;

public class AbilityInsertListener : MonoBehaviour
{
    [SerializeField]
    private AbilityInsertionArea bottomRightArrow = null;

    [SerializeField]
    private AbilityInsertionArea bottomLeftArrow = null;

    [SerializeField]
    private AbilityInsertionArea centralArea = null;

    public Subject<InsertArea> AbilityInsertSubject { get; } = new Subject<InsertArea>();

    private void Start()
    {
        bottomLeftArrow.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertArea.BottomLeft)
        );

        bottomRightArrow.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertArea.BottomRight)
        );

        centralArea.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertArea.Centre)
        );
    }

    internal void Activate(Node node, SerializableGuid abilityId)
    {
        // bool abilityIsFinisher = AbilityLookup.Instance.IsFinisher(ability);
        bool abilityIsFinisher = false;
        // bool nodeAbilityIsFinisher = AbilityLookup.Instance.IsFinisher(node.Ability);
        bool nodeAbilityIsFinisher = false;

        // Set Central area
        if (
            !node.IsRootNode
            && !(abilityIsFinisher && node.IsParent)
            && node.AbilityId != abilityId
        )
        {
            centralArea.gameObject.SetActive(true);
        }

        // Set Child areas
        if (!nodeAbilityIsFinisher)
        {
            if (!abilityIsFinisher || !node.HasChild(Direction.Left))
                bottomLeftArrow.gameObject.SetActive(true);

            if (!abilityIsFinisher || !node.HasChild(Direction.Right))
                bottomRightArrow.gameObject.SetActive(true);
        }
    }

    internal void Deactivate()
    {
        bottomLeftArrow.gameObject.SetActive(false);

        bottomRightArrow.gameObject.SetActive(false);

        centralArea.gameObject.SetActive(false);
    }
}
