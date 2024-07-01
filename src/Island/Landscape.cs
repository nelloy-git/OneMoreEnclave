using Godot;

namespace Island {

    [Tool]
    [GlobalClass]
    public partial class Landscape : Node3D {
        [Export]
        public HeightMap HeightMap;

        public MeshInstance3D Model {
            get { return _model; }
            set {
                _model = value;
                if (_model != null) {
                    AddChild(_model);
                    _model.Owner = GetTree().EditedSceneRoot;
                }
            }
        }

        public StaticBody3D Collider {
            get { return _collider; }
            set {
                _collider = value;
                if (_collider != null) {
                    AddChild(_collider);
                    _collider.Owner = GetTree().EditedSceneRoot;
                }
            }
        }


        private MeshInstance3D _model;
        private StaticBody3D _collider;
    }

}