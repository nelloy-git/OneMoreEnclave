using Godot;

[Tool]
[GlobalClass]
public partial class BuildingGraphicsData : Resource {
    [ExportCategory("Tile")]
    [Export]
    public CompressedTexture2D Tile = null;
    [ExportCategory("Model")]
    [Export]
    public PackedScene Model = null;

    BuildingGraphicsData(){}
}
