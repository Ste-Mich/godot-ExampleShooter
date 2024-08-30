using Godot;

public partial class CharacterMover : Node3D
{
    [Export]
    public float JumpForce { get; set; } = 15F;
    [Export]
    public float Gravity { get; set; } = 30F;
    [Export]
    public float MaxSpeed { get; set; } = 15F;
    [Export]
    public float MoveAcceleration { get; set; } = 4F;
    [Export]
    public float StopDrag { get; set; } = 0.9F;

    public CharacterBody3D CharacterBody { get; set; }
    public float MoveDrag { get; set; } = 0F;
    public Vector3 MoveDir { get; set; } = new Vector3();

    public override void _Ready()
    {
        base._Ready();

        CharacterBody = GetParent<CharacterBody3D>();

        MoveDrag = MoveAcceleration / MaxSpeed;
    }

    public void SetMoveDir(Vector3 newMoveDir)
    {
        MoveDir = newMoveDir;
    }

    public void Jump()
    {
        if (CharacterBody.IsOnFloor())
        {
            CharacterBody.Velocity = new Vector3(
                CharacterBody.Velocity.X,
                JumpForce,
                CharacterBody.Velocity.Z
                );
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (CharacterBody.Velocity.Y > 0F && CharacterBody.IsOnCeiling())
            CharacterBody.Velocity = new Vector3(
                CharacterBody.Velocity.X,
                0,
                CharacterBody.Velocity.Z
                );

        if (CharacterBody.IsOnFloor() == false)
        {
            CharacterBody.Velocity = new Vector3(
                CharacterBody.Velocity.X,
                CharacterBody.Velocity.Y - (float)(delta * Gravity),
                CharacterBody.Velocity.Z
                );
        }

        var drag = MoveDrag;

        if (MoveDir.IsZeroApprox())
        {
            drag = StopDrag;
        }

        var flatVelocity = CharacterBody.Velocity;
        flatVelocity = new Vector3(
            flatVelocity.X,
            0,
            flatVelocity.Z
            );

        CharacterBody.Velocity += MoveAcceleration * MoveDir - flatVelocity * drag;

        CharacterBody.MoveAndSlide();
    }
}
