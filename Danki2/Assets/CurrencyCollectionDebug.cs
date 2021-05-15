using UnityEngine;
using UnityEngine.VFX;

public class CurrencyCollectionDebug : MonoBehaviour
{
    [SerializeField]
    private VisualEffect visualEffect;

    private Player player = null;

    private void Start()
    {
        player = ActorCache.Instance.Player;

        this.ActOnInterval(0.1f, _ => visualEffect.SetVector3("Target", player.Centre));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            visualEffect.Play();
        }
    }
}
