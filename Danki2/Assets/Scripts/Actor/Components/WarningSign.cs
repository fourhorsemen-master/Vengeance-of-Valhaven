using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarningSign : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy = null;
    [SerializeField]
    private Text _exclaimationMark = null;

    private const float Duration = 0.5f;

    public void Start()
    {
        if (_exclaimationMark == null)
        {
            Debug.LogError("No image found for exclaimation mark");
            return;
        }

        if (_exclaimationMark.canvas == null)
        {
            return;
        }

        _exclaimationMark.enabled = false;

        _enemy.OnTelegraph.Subscribe(ShowWarning);
    }

    private void ShowWarning()
    {
        StopAllCoroutines();

        _exclaimationMark.enabled = true;
        this.WaitAndAct(Duration, () =>
        {
            _exclaimationMark.enabled = false;
        });
    }
}
