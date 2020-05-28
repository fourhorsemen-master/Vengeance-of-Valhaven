using UnityEngine;

public class OrbGenerationPanel : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private TooltipAbilityOrb tooltipAbilityOrbPrefab = null;

    public void DisplayOrbs(OrbCollection generatedOrbs)
    {
        for (int i = 0; i < rectTransform.childCount; i++)
        {
            Destroy(rectTransform.GetChild(i).gameObject);
        }

        generatedOrbs.ForEachOrb(orbType => {
            TooltipAbilityOrb orb = Instantiate(tooltipAbilityOrbPrefab, transform, false);
            orb.SetType(orbType);
        });
    }
}
