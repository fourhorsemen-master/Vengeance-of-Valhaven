using UnityEngine;

public class FlashOnHit : MonoBehaviour
{
    [SerializeField]
    private Actor actor = null;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    void Start()
    {
        actor.HealthManager.DamageSubject.Subscribe(_ => Flash());
    }

    public void Flash()
    {
        meshRenderer.material.SetEmissiveColour(new Color(0.3f, 0.3f, 0.3f));

        this.WaitAndAct(0.1f, () => meshRenderer.material.SetEmissiveColour(Color.black));
    }
}
