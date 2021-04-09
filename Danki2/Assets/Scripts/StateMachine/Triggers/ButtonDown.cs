using UnityEngine;

public class ButtonDown : StateMachineTrigger
{
    private readonly string buttonName;

    public ButtonDown(string buttonName)
    {
        this.buttonName = buttonName;
    }

    public override void Activate() {}

    public override void Deactivate() {}

    public override bool Triggers()
    {
        return Input.GetButtonDown(buttonName);
    }
}
