using UnityEngine;

public class OrbGenerationPanel : MonoBehaviour
{
    [SerializeField]
    private TooltipAbilityOrb tooltipAbilityOrbPrefab = null;

    /// <summary>
    /// Displays the orbs in the panel. If <paramref name="providedOrbs"/> passed in, only provided orbs are highlighted.
    /// </summary>
    public void DisplayOrbs(OrbCollection generatedOrbs, OrbCollection providedOrbs = null)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        generatedOrbs.ForEachOrb(orbType => {
            bool highlighted = false;

            if (providedOrbs == null)
            {
                highlighted = true;
            }
            else if (providedOrbs[orbType] > 0)
            {
                providedOrbs[orbType] -= 1;
                highlighted = true;
            }

            TooltipAbilityOrb orb = Instantiate(tooltipAbilityOrbPrefab, transform, false);
            orb.SetType(orbType, highlighted);
        });
    }
}
