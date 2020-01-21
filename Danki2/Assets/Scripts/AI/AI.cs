using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    private Actor _actor = null;

    [HideInInspector]
    public SerializablePlanner serializablePlanner = new SerializablePlanner();

    [HideInInspector]
    public SerializablePersonality serializablePersonality = new SerializablePersonality(new SerializableBehaviour());

    [HideInInspector]
    public Actor Target = null;

    private Agenda _agenda = new Agenda();

    private void Update()
    {
        _agenda = serializablePlanner.aiElement.Plan(this, _actor, _agenda);

        foreach (AIAction action in _agenda.Keys)
        {
            if (_agenda[action])
            {
                serializablePersonality[action].aiElement.Behave(this, _actor);
            }
        }
    }
}
