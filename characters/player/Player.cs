using Godot;
using System;

public partial class Player : CharacterBody3D
{
    public Camera3D PlayerCamera3D { get; set; }
    public CharacterMover CharacterMover { get; set; }

    [Export]
    public float MouseSensitivityH { get; set; } = 0.15F;
    [Export]
    public float MouseSensitivityV { get; set; } = 0.15F;

    public override void _Ready()
    {
        base._Ready();
        PlayerCamera3D = (Camera3D)GetNode("Camera3D");
        CharacterMover = (CharacterMover)GetNode("CharacterMover");

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventMouseMotion)
        {
            var ev = (InputEventMouseMotion)@event;
            RotationDegrees = new Vector3(
                RotationDegrees.X,
                RotationDegrees.Y - ev.Relative.X * MouseSensitivityH,
                RotationDegrees.Z
            );
            PlayerCamera3D.RotationDegrees = new Vector3(
                Math.Clamp(PlayerCamera3D.RotationDegrees.X - ev.Relative.Y * MouseSensitivityV, -90, 90),
                PlayerCamera3D.RotationDegrees.Y,
                PlayerCamera3D.RotationDegrees.Z
            );
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("quit"))
        {
            GetTree().Quit();
        }
        if (Input.IsActionJustReleased("restart"))
        {
            GetTree().ReloadCurrentScene();
        }
        if (Input.IsActionJustPressed("fullscreen"))
        {
            var fs = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;
            if (fs)
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            }
            else
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
            }
        }

        var inputDirection = Input.GetVector("move_left", "move_right", "move_forwards", "move_backwards");
        var moveDir = (Transform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();

        CharacterMover.SetMoveDir(moveDir);

        if (Input.IsActionJustPressed("jump"))
        {
            CharacterMover.Jump();
        }
    }
}
