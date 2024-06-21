extends Resource

class_name BuildingData

@export var name: String = "Unnamed";

@export_category("Tile")
@export_file var tile_path: String = "";
@export var tile_anchor: Vector2i = Vector2i.ZERO;

@export_category("Model")
@export_file var model_path: String = "":
    set(val):
        model_path = val;
        var dir = DirAccess.open("res://");
        if dir.file_exists(model_path):
            model = ResourceLoader.load(model_path.erase(0, 6));
        else:
            push_error(name, ": can not load model: \"", model_path.erase(0, 6), "\"");

@export var model_position: Vector3 = Vector3.ZERO;
@export var model_rotation: Vector3 = Vector3.ZERO;
@export var model_scale: Vector3 = Vector3.ONE;

var tile: Resource;
var model: PackedScene;
