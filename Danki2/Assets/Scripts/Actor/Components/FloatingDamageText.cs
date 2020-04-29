using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField]
    private Actor actor = null;

    [SerializeField]
    private Text floatingDamageNumberPrefab = null;
    
    [SerializeField]
    private Color textColor = Color.white;

    private float floatTime = 1f;
    private float scrollSpeed = 0.6f;

    private void Start()
    {
        actor.HealthManager.DamageSubject.Subscribe(ShowDamage);
    }

    private void ShowDamage(int damage)
    {
        Text damageNumber = Instantiate(floatingDamageNumberPrefab, transform, false);
        damageNumber.text = damage.ToString();

        StartCoroutine(ScrollAndFadeText(damageNumber));
    }

    private IEnumerator ScrollAndFadeText(Text text)
    {
        float timeElapsed = 0f;
        text.color = textColor;

        Color lerpTarget = textColor;
        lerpTarget.a = 0f;

        while (timeElapsed < floatTime)
        {
            timeElapsed += Time.deltaTime;
            text.transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);
            text.color = Color.Lerp(textColor, lerpTarget, timeElapsed/floatTime);
            yield return null;
        }

        Destroy(text);
    }
}
