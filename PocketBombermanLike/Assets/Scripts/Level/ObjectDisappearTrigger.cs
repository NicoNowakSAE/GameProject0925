using UnityEngine;

public class ObjectDisappearTrigger : Triggerable
{
    protected override void OnActivate()
    {
        gameObject.SetActive(true);
    }

    protected override void OnDeactivate()
    {
        gameObject.SetActive(false);
    }
}
