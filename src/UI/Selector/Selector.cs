using Godot;

namespace UI {

    public partial class Selector : Node3D {
        [Export]
        public bool Enabled = true;
        [Export]
        public uint CollisionMask = 1;
        [Export]
        public Node3D Mark;
        [Export]
        public StaticBody3D Selected {
            set {
                lock (_lock) {
                    _selected = value;
                }
            }
            get {
                lock (_lock) {
                    return _selected;
                }
            }
        }

        public override void _PhysicsProcess(double delta) {
            if (!Enabled) {
                return;
            }

            Vector2 mouse_pos = GetViewport().GetMousePosition();
            Vector3 mouse_origin = GetViewport().GetCamera3D().ProjectRayOrigin(mouse_pos);
            Vector3 mouse_normal = GetViewport().GetCamera3D().ProjectRayNormal(mouse_pos);
            GD.Print(mouse_origin, mouse_normal);

            var intersects = GetWorld3D().DirectSpaceState.IntersectRay(
                PhysicsRayQueryParameters3D.Create(
                    mouse_origin,
                    mouse_origin + mouse_normal * 100000,
                    CollisionMask));

            GD.Print(intersects);

            Variant collider_variant;
            if (intersects.TryGetValue("collider", out collider_variant)) {
                if (collider_variant.VariantType == Variant.Type.Object) {
                    var collider = collider_variant.As<GodotObject>();
                    if (collider is StaticBody3D) {
                        Selected = collider as StaticBody3D;
                    }
                }
            }

            Variant pos_variant;
            if (intersects.TryGetValue("position", out pos_variant)) {
                if (Mark != null) {
                    Mark.Position = pos_variant.As<Vector3>();
                }
            }

        }

        private object _lock = new();
        private StaticBody3D _selected;
    }

}