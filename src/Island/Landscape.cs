using System.Data;
using Godot;

[Tool]
public partial class Landscape : Node3D {
    [Export]
    public bool UpdateBtn {
        set {
            if (!value) { return; }
            if (!Update(false)) {
                GD.PushError("Landscape::Update failed");
            }
        }
        get {
            return false;
        }
    }

    [Export]
    public bool RandomizeBtn {
        set {
            if (!value) { return; }
            if (!Update(true)) {
                GD.PushError("Landscape::Update failed");
            }
        }
        get {
            return false;
        }
    }

    [Export]
    public Island Island;
    [Export]
    public LandscapeHeightMap HeightMap;
    [Export]
    public LandscapeMesh Mesh;
    [Export]
    public LandscapeBody Body;

    public bool Update(bool useRandomSeed = false) {
        if (HeightMap == null || Mesh == null || Body == null) {
            return false;
        }

        HeightMap.Update(useRandomSeed);
        Mesh.Mesh = HeightMap.Mesh;
        Body.Shape.Shape = HeightMap.Mesh.CreateTrimeshShape();

        return true;
    }
}