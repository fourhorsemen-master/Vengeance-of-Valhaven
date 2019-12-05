using System.Collections.Generic;
using UnityEngine;

public class EffectTracker : MonoBehaviour
{
    private List<Effect> _effects;
    private Actor _actor;

    void Start()
    {
        _effects = new List<Effect>();
        _actor = gameObject.GetComponent<Actor>();
    }

    private void Update()
    {
        _effects = _effects.FindAll(e => !e.Expired);
    }

    public void Process()
    {
        _effects.ForEach(e => e.Update(_actor));
    }

    public void AddEffect(Effect effect)
    {
        _effects.Add(effect);
    }
}
