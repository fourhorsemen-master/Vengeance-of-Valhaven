using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField]
    private Actor actor = null;

    [SerializeField]
    private Text floatingDamageNumberPrefab = null;

    private void Start()
    {
        actor.HealthManager.DamageSubject.Subscribe(ShowDamage);
    }

    private void ShowDamage(int damage)
    {
        var test = Instantiate(floatingDamageNumberPrefab, transform, false);
        test.text = damage.ToString();
        this.WaitAndAct(1f, () => Destroy(test));
    }
}
