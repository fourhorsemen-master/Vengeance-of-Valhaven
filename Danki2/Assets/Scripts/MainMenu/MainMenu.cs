using UnityEngine;

public enum GameType
{
    Continue,
    NewGame
}

public class MainMenu : MonoBehaviour
{
    public static GameType GameType { get; private set; }
    
    public void Continue()
    {
        GameType = GameType.Continue;
        LoadEntryScene();
    }
    
    public void NewGame()
    {
        GameType = GameType.NewGame;
        LoadEntryScene();
    }

    private void LoadEntryScene()
    {
        SceneUtils.LoadScene(Scene.EntryScene);
    }
}
