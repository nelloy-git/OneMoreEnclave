using Godot;

namespace Island {
    [Tool]
    [GlobalClass]
    public partial class LandscapeBuilder : Node {
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
        public float MinHeight = -1.0f;
        [Export]
        public float MaxHeight = 5.0f;

        [Export]
        public HeightMapBuilder HeightMapBuilder = new HeightMapBuilder();

        public Landscape Build() {
            HeightMapBuilder.Width = SizeX + 1;
            HeightMapBuilder.Height = SizeY + 1;

            HeightMap height_map = HeightMapBuilder.Build();
            if (height_map == null) {
                return null;
            }

            ArrayMesh mesh = _mesh_builder.Build(height_map);
            if (mesh == null) {
                return null;
            }

            Landscape landscape = new();
            AddChild(landscape);
            landscape.Owner = GetTree().EditedSceneRoot;
            landscape.Name = "Landscape";

            MeshInstance3D model = new();
            // landscape.AddChild(model);
            // model.Owner = GetTree().EditedSceneRoot;
            model.Name = "Model";
            model.Mesh = mesh;

            // StaticBody3D collider = new();
            // collider.Name = "Collider";

            // CollisionShape3D collider_shape = new();
            // collider.AddChild(collider_shape);
            // collider_shape.Owner = GetTree().EditedSceneRoot;
            // collider_shape.Name = "Shape";
            // collider_shape.Shape = mesh.CreateTrimeshShape();

            landscape.HeightMap = height_map;
            landscape.Model = model;
            // landscape.Collider = collider;

            return landscape;
        }

        private LandscapeMeshBuilder _mesh_builder = new();

    }

}