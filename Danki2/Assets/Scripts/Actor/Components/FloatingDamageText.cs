using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField]
    private Diegetic diegetic = null;

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
        HealthManager healthManager = diegetic.Actor.HealthManager;
        SubscribeToNumberSource(healthManager.ModifiedDamageSubject.Map(d => d.Damage), damageColour);
        SubscribeToNumberSource(healthManager.ModifiedTickDamageSubject, damageColour);
        SubscribeToNumberSource(healthManager.HealSubject, healingColour);
    }

    private void OnDestroy()
    {
        foreach (Text number in floatingNumbers)
        {
            Destroy(number);
        }
    }

    private void SubscribeToNumberSource(IObservable<int> numberSource, Color color)
    {
        numberSource.Subscribe(number =>
        {
            Text textNumber = AddFloatingNumber(number.ToString());
            StartCoroutine(ScrollAndFadeText(textNumber, color));
        });
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
