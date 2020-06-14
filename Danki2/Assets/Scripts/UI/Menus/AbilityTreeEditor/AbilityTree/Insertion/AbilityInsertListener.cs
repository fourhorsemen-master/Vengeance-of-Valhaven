using UnityEngine;

public class AbilityInsertListener : MonoBehaviour
{
    [SerializeField]
    private AbilityInsertionArea topLeftArrow = null;

    [SerializeField]
    private AbilityInsertionArea topRightArrow = null;

    [SerializeField]
    private AbilityInsertionArea bottomRightArrow = null;

    [SerializeField]
    private AbilityInsertionArea bottomLeftArrow = null;

    [SerializeField]
    private AbilityInsertionArea centralArea = null;

    public Subject<InsertLocation> AbilityInsertSubject { get; } = new Subject<InsertLocation>();

    private void Start()
    {
        topLeftArrow.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertLocation.TopLeft)
        );

        topRightArrow.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertLocation.TopRight)
        );

        bottomLeftArrow.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertLocation.BottomLeft)
        );

        bottomRightArrow.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertLocation.BottomRight)
        );

        centralArea.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertLocation.Centre)
        );
    }
}
