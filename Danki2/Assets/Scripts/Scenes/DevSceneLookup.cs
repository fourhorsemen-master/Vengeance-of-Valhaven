using UnityEngine;

public class DevSceneLookup : SceneLookup
{
    [SerializeField] private Pole entranceSide = Pole.South;

    public override Pole GetEntranceSide(Scene scene, int entranceId) => entranceSide;

    protected override bool DestroyOnLoad => true;
}
