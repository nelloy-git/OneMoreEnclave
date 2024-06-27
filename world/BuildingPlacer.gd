extends Node;

@export var detector: GridPosDetector;
@export var building: BuildingData;

var last_grid_mouse_pos: Vector2i;
var model: Node3D;

func _ready():
    # model = building.model.instantiate();
    # add_child(model);
    # model.position = Vector3.ZERO;
    # model.visible = false;
    pass

func _input(_event):
    pass
    # if not (event is InputEventMouseMotion):
    #     return

    # if (detector == null or building == null):
    #     return

    # if (not _updateGridMousePos()):
    #     return

    # if (model != null):
    #     model.free();
    #     model = null;

    # model = _createMeshFromTile();
    # add_child(model);
    # var pos_to_draw = detector.getGlobalPos(last_grid_mouse_pos);
    # model.position = pos_to_draw - detector.getGlobalPos(building.tile_anchor);

func _updateGridMousePos() -> bool:
    var grid_mouse_pos = detector.getMousePos();
    if (grid_mouse_pos == null or grid_mouse_pos.pos == last_grid_mouse_pos):
        return false;
    last_grid_mouse_pos = grid_mouse_pos.pos
    return true;

func _createMeshFromTile():
    var inst: MeshInstance3D = MeshInstance3D.new();
    var mesh: ImmediateMesh = ImmediateMesh.new();

    inst.mesh = mesh;
    inst.cast_shadow = GeometryInstance3D.SHADOW_CASTING_SETTING_OFF;

    var tile = building.tile;
    for i in range(0, tile.get_width()):
        for j in range(0, tile.get_height()):
            var pos: Vector3 = detector.getGlobalPos(Vector2i(i, j))
            # print("pos: ", pos);
            var side1: Vector3 = detector.getGlobalPos(Vector2i(i + 1, j))
            # print("side1: ", side1);
            var side2: Vector3 = detector.getGlobalPos(Vector2i(i, j + 1))
            # print("side2: ", side2);
            Primitive3D.addRect(
                mesh,
                pos,
                side1,
                side2,
                tile.get_pixel(i, j)
            );

    return inst;
    
# func _

func _on_collider_input_event(_camera: Node, _event: InputEvent, _position: Vector3, _normal: Vector3, _shape_idx: int):
    if (model != null):
        model.free();

    model = _createMeshFromTile();
    add_child(model);
    model.position = _position;
