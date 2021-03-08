using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    private const float TransitionDistance = 1;

    [SerializeField] public int id = 0;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private Light pointLight = null;

    private void Start()
    {
        pointLight.color = color;
        GameplaySceneTransitionManager.Instance.CanTransitionSubject.Subscribe(() => pointLight.enabled = true);
    }

    private void Update()
    {
        if (!GameplaySceneTransitionManager.Instance.CanTransition) return;
        if (transform.DistanceFromPlayer() > TransitionDistance) return;

        int currentSceneId = PersistenceManager.Instance.SaveData.CurrentSceneId;
        SceneSaveData sceneSaveData = PersistenceManager.Instance.SaveData.SceneSaveDataLookup[currentSceneId];
        int nextSceneId = sceneSaveData.SceneTransitionerIdToNextSceneId[id];
        PersistenceManager.Instance.TransitionToNextScene(nextSceneId);
    }
}
