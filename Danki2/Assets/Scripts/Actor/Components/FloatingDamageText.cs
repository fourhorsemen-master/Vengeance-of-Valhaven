using System.Collections;
using System.Collections.Generic;
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

    private const float FloatTime = 1f;
    private const float ScrollSpeed = 0.6f;

    private readonly HashSet<Text> damageNumbers = new HashSet<Text>();

    private void Start()
    {
        actor.HealthManager.DamageSubject.Subscribe(ShowDamage);
    }

    private void OnDestroy()
    {
        foreach (Text number in damageNumbers)
        {
            Destroy(number);
        }
    }

    private void ShowDamage(int damage)
    {
        Text damageNumber = Instantiate(floatingDamageNumberPrefab, transform, false);
        damageNumber.text = damage.ToString();
        damageNumbers.Add(damageNumber);

        StartCoroutine(ScrollAndFadeText(damageNumber));
    }

    private IEnumerator ScrollAndFadeText(Text text)
    {
        float timeElapsed = 0f;
        text.color = textColor;

        Color lerpTarget = textColor;
        lerpTarget.a = 0f;

        while (timeElapsed < FloatTime)
        {
            timeElapsed += Time.deltaTime;
            text.transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed);
            text.color = Color.Lerp(textColor, lerpTarget, timeElapsed / FloatTime);
            yield return null;
        }

        damageNumbers.Remove(text);
        Destroy(text);
    }
}
