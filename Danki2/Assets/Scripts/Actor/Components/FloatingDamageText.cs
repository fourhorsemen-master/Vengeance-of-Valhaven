using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField]
    private Actor actor = null;

    [SerializeField]
    private Text floatingDamageNumberPrefab = null;

    private float floatTime = 5f;
    private float scrollSpeed = 0.5f;

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
        Color lerpTarget = text.color;
        lerpTarget.a = 0f;

        while (timeElapsed < floatTime)
        {
            timeElapsed += Time.deltaTime;
            text.transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);
            text.color = Color.Lerp(text.color, lerpTarget, timeElapsed/floatTime);
            yield return null;
        }

        Destroy(text);
    }
}
