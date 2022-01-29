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

    internal void Activate(Node node)
    {
        // Set Central area
        if (!node.IsRootNode)
        {
            centralArea.gameObject.SetActive(true);
        }

        bottomLeftArrow.gameObject.SetActive(true);
        bottomRightArrow.gameObject.SetActive(true);
    }

    internal void Deactivate()
    {
        bottomLeftArrow.gameObject.SetActive(false);

        bottomRightArrow.gameObject.SetActive(false);

        centralArea.gameObject.SetActive(false);
    }
}
