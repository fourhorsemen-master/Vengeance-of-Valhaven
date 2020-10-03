using UnityEngine;
using UnityEngine.UI;

public class KillTracker : MonoBehaviour
{
    [SerializeField]
    private Text text = null;

    private void Start()
    {
        RoomManager.Instance.KillsSubject.Subscribe(i => text.text = i.ToString());
    }
}
