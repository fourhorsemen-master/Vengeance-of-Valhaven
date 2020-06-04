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
    private Color damageColour = Color.white;

    [SerializeField]
    private Color healingColour = Color.green;

    private const float FloatTime = 1f;
    private const float ScrollSpeed = 0.6f;

    private readonly HashSet<Text> floatingNumbers = new HashSet<Text>();

    private void Start()
    {
        actor.HealthManager.DamageSubject.Subscribe(ShowDamage);
        actor.HealthManager.HealSubject.Subscribe(ShowHealing);
    }

    private void OnDestroy()
    {
        foreach (Text number in floatingNumbers)
        {
            Destroy(number);
        }
    }

    private void ShowDamage(int damage)
    {
        Text damageNumber = AddFloatingNumber(damage.ToString());
        StartCoroutine(ScrollAndFadeText(damageNumber, damageColour));
    }

    private void ShowHealing(int healing)
    {
        Text healingNumber = AddFloatingNumber(healing.ToString());
        StartCoroutine(ScrollAndFadeText(healingNumber, healingColour));
    }

    private Text AddFloatingNumber(string text)
    {
        Text healingNumber = Instantiate(floatingDamageNumberPrefab, transform, false);
        healingNumber.text = text;
        floatingNumbers.Add(healingNumber);
        return healingNumber;
    }

    private IEnumerator ScrollAndFadeText(Text text, Color colour)
    {
        float timeElapsed = 0f;
        text.color = colour;

        Color lerpTarget = colour;
        lerpTarget.a = 0f;

        while (timeElapsed < FloatTime)
        {
            timeElapsed += Time.deltaTime;
            text.transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed);
            text.color = Color.Lerp(colour, lerpTarget, timeElapsed / FloatTime);
            yield return null;
        }

        floatingNumbers.Remove(text);
        Destroy(text);
    }
}
