using Godot;
using System;

public partial class Player : Entity
{
    [ExportGroup("Movement")]
    [Export]
    private float speed = 400f;
    [Export]
    private float jumpSpeed = 800f;
    private float moveVelocity = 0f;
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("up") && IsOnFloor()) Jump();

        base._PhysicsProcess(delta);
    }

    public void Jump() {
        Force jumpForce = new Force(new Vector2(0, -1), jumpSpeed, decay: 0.2f, canDecay: true);
        AddForce(jumpForce);
        
        Timer timer = new() { WaitTime = 0.5f, Autostart = true };
        AddChild(timer);
        timer.Timeout += () => { forces.Remove(jumpForce); timer.QueueFree(); };
    }

    public int GetInputDirection()
    {
        return (Input.IsActionPressed("left") ? 0 : 1) - (Input.IsActionPressed("right") ? 0 : 1);
    }

    public override Vector2 GetVelocity()
    {
        Vector2 velocity = base.GetVelocity();

        moveVelocity = Mathf.Lerp(moveVelocity, GetInputDirection() * speed, 0.35f);

        velocity += new Vector2(moveVelocity, 0);
        return velocity;
    }
}
