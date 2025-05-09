using UnityEngine;

public class TargetDummy : Enemy
{
    protected override void Update()
    {
        if (IsOutOfView() && isGrabbed)
        {
            Destroy(gameObject);
        }
    }
}
