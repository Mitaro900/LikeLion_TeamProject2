using UnityEngine;

public class Player_Test : PhysicsObject
{
    [SerializeField] private float moveSpeed = 3f;
    private float xInput;

    protected override void ComputeVelocity()
    {
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            xInput = 0f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            xInput = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            xInput = -1f;
        }
        else
        {
            xInput = 0f;
        }

        targetVelocity.x = xInput * moveSpeed;
    }
}
