using Godot;
using System;

public partial class Player : Entity
{
    [ExportGroup("Movement")]
    [Export]
    private float speed = 250f;
    [Export]
    private float jumpSpeed = 800f;
    [Export]
    public RayCast2D[] jumpRaycasts;

    private float moveVelocity = 0f;
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("up") && CanJump()) Jump();
        if (Input.IsActionPressed("down")) AddForce(new(this, Position.DirectionTo(GetGlobalMousePosition()), 10f));

        base._PhysicsProcess(delta);
    }

    public void Jump() {
        Force jumpForce = new(this, new(0, -1), jumpSpeed, decay: 0.2f, canDecay: true);
        AddForce(jumpForce);
        
        Timer timer = new() { WaitTime = 0.5f, Autostart = true };
        AddChild(timer);
        timer.Timeout += () => { forces.Remove(jumpForce); timer.QueueFree(); };
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

        return IsOnFloor() || raysTouching;
    }

    public override Vector2 GetVelocity()
    {
        Vector2 velocity = base.GetVelocity();

        moveVelocity = Mathf.Lerp(moveVelocity, GetInputDirection() * speed, 0.35f);

        velocity += new Vector2(moveVelocity, 0);
        return velocity;
    }
}
