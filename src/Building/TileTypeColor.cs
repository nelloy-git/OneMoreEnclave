using Godot;
using Godot.Collections;

public partial class TileTypeColor : Resource {
    [Export]
    public Dictionary<Color, TileType> Map;
}