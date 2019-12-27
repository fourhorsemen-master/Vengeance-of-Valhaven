using UnityEngine;

public class AIComponent : MonoBehaviour
{
    [SerializeField]
    private Actor _actor = null;

    [HideInInspector]
    public SerializablePlanner _serializablePlanner = new SerializablePlanner();

    [HideInInspector]
    public SerializablePersonality _serializablePersonality = new SerializablePersonality(new SerializableBehaviour());

    private Agenda _agenda = new Agenda();

    private void Update()
    {
        _agenda = _serializablePlanner.planner.Plan(_actor, _agenda);

        foreach (AIAction action in _agenda.Keys)
        {
            if (_agenda[action])
            {
                _serializablePersonality[action].behaviour.Behave(_actor);
            }
        }
    }
}
