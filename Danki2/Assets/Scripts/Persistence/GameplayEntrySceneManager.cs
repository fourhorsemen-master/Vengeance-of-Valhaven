using UnityEngine;

public class GameplayEntrySceneManager : MonoBehaviour
{
    private void Start()
    {
        int id = PersistenceManager.Instance.SaveData.CurrentSceneId;
        Scene scene = PersistenceManager.Instance.SaveData.SceneSaveDataLookup[id].Scene;
        SceneUtils.LoadScene(scene);
    }
}
