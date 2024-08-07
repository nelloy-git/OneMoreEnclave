using Godot;

[Tool]
[GlobalClass]
public partial class LandscapeBuilderSettings : Resource {
    [Export]
    public uint Width = 32;
    [Export]
    public uint Height = 32;
}