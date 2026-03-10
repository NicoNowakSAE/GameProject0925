using UnityEngine;

public class TriggerableTest : Triggerable
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
