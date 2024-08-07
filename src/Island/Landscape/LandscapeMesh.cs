using Godot;

[Tool]
[GlobalClass]
public partial class LandscapeMesh : MeshInstance3D {
    [Export]
    public Landscape Landscape;
}