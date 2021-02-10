using UnityEngine;
using UnityEngine.UI;

public class WarningSign : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy = null;
    [SerializeField]
    private Text exclaimationMark = null;

    public void Start()
    {
        exclaimationMark.enabled = false;
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
