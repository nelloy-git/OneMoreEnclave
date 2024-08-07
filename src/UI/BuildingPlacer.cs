// using Event;
// using Godot;

// namespace UI {
//     [GlobalClass]
//     public partial class BuildingPlacer : Node {
//         [Export]
//         public bool Enabled {
//             set {
//                 if (_Enabled == value) {
//                     return;
//                 }

//                 if (value) {
//                     if (!EventHandler.Inst.CursorDetector.UseMask(this, Mask)) {
//                         GD.PushError("CursorDetector is already in use");
//                         return;
//                     }
//                     EventHandler.Inst.CursorDetector.OnHover += _ProcessHover;
//                 } else {
//                     if (!EventHandler.Inst.CursorDetector.FreeMask(this)) {
//                         GD.PushError("CursorDetector has other user");
//                         return;
//                     }
//                     EventHandler.Inst.CursorDetector.OnHover -= _ProcessHover;
//                 }
//                 _Enabled = value;
//             }
//             get { return _Enabled; }
//         }
//         [Export]
//         public Resource Building = null;
//         [Export]
//         public Node3D Mark = null;
//         [Export]
//         public PhyLayer Mask = PhyLayer.Landscape;

//         private void _ProcessHover(CursorCapture capture) {
//             if (!_Enabled) {
//                 return;
//             }

//             var intersect = capture[PhyLayer.Landscape];
//             if (intersect == null) {
//                 GD.Print("island_intersect == null");
//                 return;
//             }

//             if (intersect.Body == null) {
//                 GD.Print("island_intersect.Body == null");
//                 return;
//             }

//             if (intersect.Body is not IslandNS.LandscapeNS.Body) {
//                 GD.PushError("Wrong type: ", intersect.Body.GetType());
//                 return;
//             }

//             if (Mark != null) {
//                 Mark.Position = new Vector3(
//                     (int)System.Math.Round(intersect.Position.X),
//                     intersect.Position.Y,
//                     (int)System.Math.Round(intersect.Position.Z));
//             }

//             if (Building != null && _TileMesh == null) {
//                 var tile_obj = Building.Get("tile").AsGodotObject();
//                 if (tile_obj == null) {
//                     GD.PushError("Can not get property ", Building.Get("tile").VariantType);
//                     return;
//                 }

//                 if (tile_obj is not Image tile) {
//                     GD.PushError("Is not image");
//                     return;
//                 }

//                 _TileMesh = new BuildingTileMesh(tile);
//                 AddChild(_TileMesh);
//                 _TileMesh.Owner = GetTree().Root;
//             }

//             if (_TileMesh != null) {
//                 _TileMesh.Position = new Vector3(
//                     (int)System.Math.Round(intersect.Position.X),
//                     intersect.Position.Y,
//                     (int)System.Math.Round(intersect.Position.Z));
//             }

//             // _TileMesh.Position = island_intersect.Position;
//         }

//         // public override void _Input(InputEvent @event) {
//         //     if (@event is InputEventMouseButton) {
//         //         if ((@event as InputEventMouseButton).ButtonIndex == MouseButton.Left
//         //             && (@event as InputEventMouseButton).Pressed) {
//         //             GD.Print("Click");
//         //             Enabled = !Enabled;
//         //         }
//         //     }
//         // }

//         private bool _Enabled = false;
//         private BuildingTileMesh _TileMesh = null;

//     }

// }