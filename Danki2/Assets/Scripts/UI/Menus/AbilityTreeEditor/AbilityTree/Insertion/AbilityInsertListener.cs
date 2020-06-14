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

    public Subject<InsertArea> AbilityInsertSubject { get; } = new Subject<InsertArea>();

    private void Start()
    {
        topLeftArrow.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertArea.TopLeft)
        );

        topRightArrow.MouseUpSubject.Subscribe(
            () => AbilityInsertSubject.Next(InsertArea.TopRight)
        );

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

    public void SetInsertableAreas(Node node)
    {
        if (node.IsRootNode) return;

        centralArea.gameObject.SetActive(true);

        bottomLeftArrow.gameObject.SetActive(!node.HasChild(Direction.Left));

        bottomRightArrow.gameObject.SetActive(!node.HasChild(Direction.Right));

        if (node.Parent.HasChild(Direction.Right))
        {
            topLeftArrow.gameObject.SetActive(node.Parent.GetChild(Direction.Right) == node);
        }

        if (node.Parent.HasChild(Direction.Left))
        {
            topRightArrow.gameObject.SetActive(node.Parent.GetChild(Direction.Left) == node);
        }
    }
}
