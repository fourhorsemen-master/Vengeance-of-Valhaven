using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void Quit() => Application.Quit();

    public void Restart() => Application.LoadLevel(Application.loadedLevel);
}
