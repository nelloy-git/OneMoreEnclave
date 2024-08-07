using Godot;

namespace Event {

    public partial class CursorPhyLayer : GodotObject {
        public CursorPhyLayer(PhysicsBody3D body, Vector3 position, float range) {
            Body = body;
            Position = position;
            Distance = range;
        }

        public readonly PhysicsBody3D Body;
        public readonly Vector3 Position;
        public readonly float Distance;
    }

}