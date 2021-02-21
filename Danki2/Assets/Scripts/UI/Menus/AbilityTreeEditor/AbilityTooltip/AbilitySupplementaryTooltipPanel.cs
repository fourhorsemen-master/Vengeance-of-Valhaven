using UnityEngine;

public class AbilitySupplementaryTooltipPanel : MonoBehaviour
{
    [SerializeField]
    private AbilitySupplementaryTooltip abilitySupplementaryTooltipPrefab = null;

    public void ShowSupplementaryTooltips(AbilityReference ability)
    {
        Clear();

        var tooltipTypes = SupplementaryTooltipUtils.GetSupplementaryTooltips(ability);

        tooltipTypes.ForEach(t =>
        {
            Instantiate(abilitySupplementaryTooltipPrefab, transform)
            .Setup(t);
        });
    }

    private void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
