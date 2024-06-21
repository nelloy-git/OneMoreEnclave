extends Node;

@export var detector: GridPosDetector;
@export var building: BuildingData;

var last_grid_mouse_pos: Vector2i;
var model: Node3D;

func _ready():
    model = building.model.instantiate();
    add_child(model);
    model.position = Vector3.ZERO;

func _input(event):
    if not (event is InputEventMouseMotion):
        return

    if (detector == null or building == null):
        return

    var grid_mouse_pos = detector.getMousePos();
    if (grid_mouse_pos == null or grid_mouse_pos.pos == last_grid_mouse_pos):
        return

    last_grid_mouse_pos = grid_mouse_pos.pos;
    var pos_to_draw = detector.getGlobalPos(last_grid_mouse_pos);
    model.position = pos_to_draw + building.model_position;
