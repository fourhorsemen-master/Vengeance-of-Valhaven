using UnityEngine;

public class OrbGenerationPanel : MonoBehaviour
{
    [SerializeField]
    private TooltipAbilityOrb tooltipAbilityOrbPrefab = null;

    /// <summary>
    /// Displays the orbs in the panel. If <paramref name="inputOrbs"/> passed in, only provided orbs are highlighted.
    /// </summary>
    public void DisplayOrbs(OrbCollection generatedOrbs, OrbCollection inputOrbs = null)
    {
        bool hasInputOrbs = inputOrbs != null;
        OrbCollection remainingInputOrbs = hasInputOrbs ? new OrbCollection(inputOrbs) : null;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        generatedOrbs.ForEachOrb(orbType => {
            bool highlighted = false;

            if (!hasInputOrbs)
            {
                highlighted = true;
            }
            else if (remainingInputOrbs[orbType] > 0)
            {
                remainingInputOrbs[orbType] -= 1;
                highlighted = true;
            }

            Instantiate(tooltipAbilityOrbPrefab, transform, false)
                .SetType(orbType, highlighted);
        });
    }
}
