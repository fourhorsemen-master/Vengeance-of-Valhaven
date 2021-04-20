using UnityEngine;
using UnityEngine.UI;

public class SavingIndicator : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] private Image image = null;
    [SerializeField] private float minOpacity = 0;
    [SerializeField] private float maxOpacity = 0;

    private bool saving;

    private void Start()
    {
        SaveDataManager.Instance.SavingSubject.Subscribe(s => saving = s);
    }

    private void Update()
    {
        if (!saving)
        {
            image.SetOpacity(0);
            return;
        }

        float sin = Mathf.Sin(Time.time * speed);
        float opacity = minOpacity + (sin + 1) * (maxOpacity - minOpacity) / 2;
        image.SetOpacity(opacity);
    }
}
