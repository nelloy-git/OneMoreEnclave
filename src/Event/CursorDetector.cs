using System;
using Godot;
using System.Collections.Generic;

namespace Event {
    [GlobalClass]
    public partial class CursorDetector : Node3D {
        [Export]
        public float Range = 10000;
        [Export]
        public bool IsWindowFocused = true;
        [Export]
        public PhyLayer Mask {
            set {
                GD.PushError("Readonly");
            }
            get {
                return _Mask;
            }
        }
        private object _MaskUser = null;

        [Signal]
        public delegate void OnHoverEventHandler(CursorCapture capture);

        // TODO queue users
        public bool InUse() {
            return _MaskUser != null;
        }

        public bool UseMask(object user, PhyLayer mask) {
            if (_MaskUser != null && _MaskUser != user) {
                return false;
            }

            _MaskUser = user;
            _Mask = mask;
            return true;
        }

        public bool FreeMask(object user) {
            if (_MaskUser != user) {
                GD.PushError("Can not free mask by wrong user");
                return false;
            }
            _Mask = 0;
            return true;
        }

        public override void _PhysicsProcess(double delta) {
            if (!IsWindowFocused || !InUse()) {
                return;
            }

            if (!_GetCameraPosition(out Vector3 camera_pos)) {
                return;
            }

            if (!_GetCursorProjection(out Vector3 cursor_proj)) {
                return;
            }

            Dictionary<PhyLayer, CursorPhyLayer> map = new();
            foreach (PhyLayer curLayer in Enum.GetValues<PhyLayer>()) {
                if (!Convert.ToBoolean(curLayer & Mask)) {
                    continue;
                }

                var intersect = GetWorld3D().DirectSpaceState.IntersectRay(
                    PhysicsRayQueryParameters3D.Create(
                        camera_pos,
                        camera_pos + cursor_proj * Range,
                        Convert.ToUInt32(curLayer)));

                if (!_GetCaptureLayer(intersect, camera_pos, out CursorPhyLayer layer_data)) {
                    continue;
                }

                map[curLayer] = layer_data;
            }

            if (map.Count > 0) {
                CallDeferred(MethodName.EmitSignal, SignalName.OnHover, new CursorCapture(map));
            }
        }

        private bool _GetCameraPosition(out Vector3 position) {
            position = Vector3.Zero;
            if (!_GetCursorPos(out Vector2 cursor_pos)) {
                return false;
            }
            position = GetViewport().GetCamera3D().ProjectRayOrigin(cursor_pos);
            return true;
        }

        private bool _GetCursorProjection(out Vector3 projection) {
            projection = Vector3.Zero;
            if (!_GetCursorPos(out Vector2 cursor_pos)) {
                return false;
            }
            projection = GetViewport().GetCamera3D().ProjectRayNormal(cursor_pos);
            return true;
        }

        private bool _GetCursorPos(out Vector2 cursor_position) {
            Viewport viewport = GetViewport();
            cursor_position = viewport.GetMousePosition();
            if (cursor_position.X < 0 || cursor_position.X > viewport.GetVisibleRect().Size.X
                || cursor_position.Y < 0 || cursor_position.Y > viewport.GetVisibleRect().Size.Y) {
                return false;
            }
            return true;
        }

        private static bool _GetCaptureLayer(in Godot.Collections.Dictionary intersect,
                                             in Vector3 mouse_position,
                                             out CursorPhyLayer layer_data) {
            layer_data = null;

            if (!_GetIntersectBody(intersect, out PhysicsBody3D body)) {
                return false;
            }

            if (!_GetIntersectPosition(intersect, out Vector3 position)) {
                return false;
            }

            layer_data = new(body, position, mouse_position.DistanceTo(position));
            return true;
        }

        private static bool _GetIntersectBody(in Godot.Collections.Dictionary intersect,
                                              out PhysicsBody3D body) {
            body = null;

            if (!intersect.TryGetValue("collider", out Variant body_variant)) {
                return false;
            }

            if (body_variant.VariantType != Variant.Type.Object) {
                return false;
            }

            GodotObject body_obj = body_variant.AsGodotObject();
            if (body_obj == null) {
                return false;
            }

            body = body_obj as PhysicsBody3D;
            return body != null;
        }

        private static bool _GetIntersectPosition(in Godot.Collections.Dictionary intersect,
                                                  out Vector3 position) {
            position = Vector3.Zero;

            if (!intersect.TryGetValue("position", out Variant position_variant)) {
                return false;
            }

            if (position_variant.VariantType != Variant.Type.Vector3) {
                return false;
            }

            position = position_variant.AsVector3();
            return true;
        }

        public override void _Notification(int what) {
            switch ((long)what) {
                case MainLoop.NotificationApplicationFocusIn:
                    IsWindowFocused = true;
                    return;
                case MainLoop.NotificationApplicationFocusOut:
                    IsWindowFocused = false;
                    return;
                default:
                    return;
            }
        }

        private PhyLayer _Mask = 0;
    }
}
