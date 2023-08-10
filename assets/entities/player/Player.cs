using Godot;
using System;

public partial class Player : Entity
{
    private float moveVelocity = 0f;
    private readonly float speed = 400f;
    private readonly float jumpSpeed = 800f;
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("up"))
        {
            AddForce(new Force(new Vector2(0, -1), jumpSpeed, decay: 0.1f, canDecay: true));
        }

        base._PhysicsProcess(delta);
    }

    public int GetInputDirection()
    {
        return (Input.IsActionPressed("left") ? 0 : 1) - (Input.IsActionPressed("right") ? 0 : 1);
    }

    public override Vector2 GetVelocity()
    {
        Vector2 velocity = base.GetVelocity();

        moveVelocity = Mathf.Lerp(moveVelocity, GetInputDirection() * speed, 0.1f);

        velocity += new Vector2(moveVelocity, 0);
        return velocity;
    }
}
