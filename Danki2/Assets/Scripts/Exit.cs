using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private Light pointLight;

    private void Start()
    {
        Debug.Log("Slay all enemies to advance...");
        RoomManager.Instance.CanAdvanceSubject.Subscribe(() =>
        {
            Debug.Log("All enemies slain. You may advance.");
            pointLight.enabled = true;
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(Tags.Player)) return;

        if (RoomManager.Instance.CanAdvance)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "PlayableLevel":
                    SceneManager.LoadSceneAsync("DemoScene");
                    break;
                case "DemoScene":
                    SceneManager.LoadSceneAsync("PlayableLevel");
                    break;
            }
        }
    }
}
