using UnityEngine;
using UnityEngine.UI;

public class WarningSign : MonoBehaviour
{
    [SerializeField]
    private Diegetic diegetic = null;

    [SerializeField]
    private Text exclaimationMark = null;

    private Enemy enemy;

    public void Start()
    {
        exclaimationMark.enabled = false;

        if (diegetic.Actor.IsPlayer) Debug.LogError("Warning sign added to player.");

        enemy = (Enemy)diegetic.Actor;
    }

    private void Update()
    {
        if (enemy.CurrentTelegraph == null)
        {
            exclaimationMark.enabled = false;
            return;
        }

        exclaimationMark.enabled = true;
        exclaimationMark.color = enemy.CurrentTelegraph.Value;
    }
}
