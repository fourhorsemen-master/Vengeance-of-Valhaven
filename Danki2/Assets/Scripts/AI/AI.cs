using System;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    private Actor actor = null;

    [HideInInspector]
    public SerializablePlanner serializablePlanner = new SerializablePlanner();

    [HideInInspector]
    public SerializablePersonality serializablePersonality = new SerializablePersonality(() => new SerializableBehaviour());

    private Agenda agenda = new Agenda();

    private void Start()
    {
        serializablePlanner.AiElement.OnStart(actor);
        foreach (AIAction aiAction in Enum.GetValues(typeof(AIAction)))
        {
            serializablePersonality[aiAction].AiElement.OnStart(actor);
        }
    }

    private void Update()
    {
        if (actor.Dead) return;

        agenda = serializablePlanner.AiElement.Plan(actor, agenda);

        foreach (AIAction action in agenda.Keys)
        {
            if (agenda[action])
            {
                serializablePersonality[action].AiElement.Behave(actor);
            }
        }
    }
}
