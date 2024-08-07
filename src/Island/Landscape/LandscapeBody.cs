using Godot;

[Tool]
[GlobalClass]
public partial class LandscapeBody : StaticBody3D {
    [Export]
    public Landscape Landscape;
    [Export]
    public CollisionShape3D Shape;
}