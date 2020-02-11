using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarningSign : MonoBehaviour
{
    [SerializeField]
    private Text _exclaimationMark = null;

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
    }

    public void ShowWarning(float duration)
    {
        StopAllCoroutines();
        _exclaimationMark.enabled = true;
        StartCoroutine(HideWarningAfter(duration));
    }

    IEnumerator HideWarningAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _exclaimationMark.enabled = false;
    }
}
