using Godot;

namespace Island {
    [Tool]
    [GlobalClass]
    public partial class LandscapeBuilder : Node {
        static private PackedScene LandscapeScene = ResourceLoader.Load<PackedScene>("res://resource/Island/Landscape.tscn");

        [Export]
        public bool BuildBtn {
            get { return false; }
            set {
                if (!Engine.IsEditorHint()) {
                    GD.PushError("Editor only");
                    return;
                }
                if (!value) {
                    return;
                }
                Build();
            }
        }

        [Export]
        public int SizeX = 32;
        [Export]
        public int SizeY = 32;

        [Export]
        public HeightMapBuilder HeightMapBuilder = new HeightMapBuilder();

        public Landscape Build() {
            HeightMapBuilder.Width = SizeX + 1;
            HeightMapBuilder.Height = SizeY + 1;

            HeightMap height_map = HeightMapBuilder.Build();
            if (height_map == null) {
                return null;
            }

            // ArrayMesh mesh = _mesh_builder.Build(height_map);
            // if (mesh == null) {
            //     return null;
            // }

            var landscape = LandscapeScene.Instantiate() as Landscape;
            AddChild(landscape);
            landscape.Owner = GetTree().EditedSceneRoot;
            landscape.HeightMap = height_map;



            // Landscape landscape = new() {
            //     Name = "Landscape"
            // };
            // AddChild(landscape);
            // landscape.Owner = GetTree().EditedSceneRoot;

            // landscape.HeightMap = height_map;
            // landscape.Model.Mesh = mesh;
            // landscape.ColliderShape.Shape = mesh.CreateTrimeshShape();

            // return landscape;
            return null;
        }

        private LandscapeMeshBuilder _mesh_builder = new();

    }

}