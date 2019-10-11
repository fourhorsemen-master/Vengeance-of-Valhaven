using Assets.Scripts.Effects;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private EffectTracker effectTracker;

    // Start is called before the first frame update
    void Start()
    {
        this.effectTracker = new EffectTracker(this);
    }

    // Update is called once per frame
    void Update()
    {
        this.effectTracker.ProcessEffects();
    }

    public void AddEffect(Effect effect)
    {
        this.effectTracker.AddEffect(effect);
    }
}
