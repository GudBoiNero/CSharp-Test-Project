using Godot;
using System;

public partial class Player : Entity
{
    [ExportGroup("Movement")]
    [Export]
    private float speed = 250f;
    [Export]
    private float jumpSpeed = 1200f;
    [Export]
    public RayCast2D[] jumpRaycasts;

    private float moveVelocity = 0f;

    readonly private float rotationAccel = 0.2f;
    readonly private float rotationSpeed = 0.25f;
    private float rotationVelocity = 0f;
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("up") && CanJump()) Jump();

        base._PhysicsProcess(delta);
    }

    public void Jump() {
        Force jumpForce = new(this, new(0, -1), jumpSpeed, decay: 0.1f, canDecay: true);
        AddForce(jumpForce);

        Wait.For(this, 0.5f, () => { forces.Remove(jumpForce); });
    }

    public int GetInputDirection()
    {
        return (Input.IsActionPressed("left") ? 0 : 1) - (Input.IsActionPressed("right") ? 0 : 1);
    }

    public bool CanJump() {
        bool raysTouching = false;

        foreach (RayCast2D ray in jumpRaycasts) {
            if (ray.IsColliding()) raysTouching = true;
        }

        return raysTouching;
    }

    public override Vector2 GetVelocity()
    {
        Vector2 velocity = base.GetVelocity();

        int inputDirection = GetInputDirection();
        moveVelocity = Mathf.Lerp(moveVelocity, inputDirection * speed, 0.15f);
        rotationVelocity = Mathf.Lerp(rotationVelocity, inputDirection * rotationSpeed, rotationAccel);

        sprite.Rotation += rotationVelocity;

        velocity += new Vector2(moveVelocity, 0);
        return velocity;
    }
}
