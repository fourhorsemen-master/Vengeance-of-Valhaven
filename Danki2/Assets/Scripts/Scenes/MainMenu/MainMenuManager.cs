using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button continueButton = null;

    private void Start()
    {
        continueButton.interactable = SaveDataManager.Instance.HasSaveData;
    }

    public void Continue()
    {
        SceneUtils.LoadScene(Scene.GameplayEntryScene);
    }

    public void NewGame()
    {
        SaveDataManager.Instance.Clear();
        SceneUtils.LoadScene(Scene.GameplayEntryScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
