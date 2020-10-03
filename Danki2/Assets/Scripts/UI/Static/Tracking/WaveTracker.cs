using UnityEngine;
using UnityEngine.UI;

public class WaveTracker : MonoBehaviour
{
    [SerializeField]
    private Text text = null;

    private void Start()
    {
        RoomManager.Instance.WaveStartSubject.Subscribe(i => text.text = i.ToString());
    }
}
