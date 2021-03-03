using UnityEngine;

public class GameplayEntrySceneManager : MonoBehaviour
{
    private void Start()
    {
        SceneUtils.LoadScene(PersistenceManager.Instance.SaveData.CurrentScene);
    }
}
