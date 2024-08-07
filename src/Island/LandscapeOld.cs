// using Godot;
// using static Utils.ExpandGodotNode;

// namespace IslandNS {

//     [Tool]
//     [GlobalClass]
//     public partial class Landscape : Node3D {
//         public const int CollisionLayer = 0;

//         [Export]
//         public HeightMap HeightMap {
//             set {
//                 if (!IsNodeReady()) {
//                     this.OnceOnReady(() => { HeightMap = value; });
//                     return;
//                 }

//                 if (_model == null) {
//                     UpdateChildren();
//                 }

//                 _height_map = value;
//                 if (_height_map == null) {
//                     _model.Mesh = null;
//                     _body_shape.Shape = null;
//                     return;

//                 }

//                 var mesh = _mesh_builder.Build(_height_map);
//                 if (mesh == null) {
//                     GD.PushError("Can not build mesh with selected HeightMap");
//                     _height_map = null;
//                     return;
//                 }

//                 _model.Mesh = mesh;
//                 _body_shape.Shape = mesh.CreateTrimeshShape();
//             }
//             get {
//                 return _height_map;
//             }
//         }

//         public override void _Ready() {
//             UpdateChildren();
//             _body.CollisionMask = 1 << CollisionLayer;
//         }

//         public void UpdateChildren() {
//             var node = GetNode("Model");
//             if (node is MeshInstance3D) {
//                 _model = node as MeshInstance3D;
//             }
//             else { GD.PushError(""); }

//             node = GetNode("Body");
//             if (node is LandscapeNS.Body) {
//                 _body = node as LandscapeNS.Body;
//                 _body.Landscape = this;

//                 node = GetNode("Body/Shape");
//                 if (node is CollisionShape3D) {
//                     _body_shape = node as CollisionShape3D;
//                 }
//                 else { GD.PushError(""); }
//             }
//             else { GD.PushError(""); }
//         }

//         private HeightMap _height_map;

//         private MeshInstance3D _model;
//         private LandscapeNS.Body _body;
//         private CollisionShape3D _body_shape;

//         private LandscapeMeshBuilder _mesh_builder = new();
//     }

// }