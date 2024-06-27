@tool

extends Resource

class_name BuildingData

@export var name: String = "Unnamed";

@export_category("Tile")
@export_file var tile_path: String = "":
    set(val):
        tile_path = val;
        val = val.erase(0, 6);
        var dir = DirAccess.open("res://");
        if dir.file_exists(val):
            tile = Image.load_from_file(val);
        else:
            push_error(name, ": can not load tile: \"", val, "\"");
            tile = null;

@export var tile_anchor: Vector2i = Vector2i.ZERO;

@export_category("Model")
@export_file var model_path: String = "":
    set(val):
        model_path = val;
        val = val.erase(0, 6);
        var dir = DirAccess.open("res://");
        if dir.file_exists(val):
            model = ResourceLoader.load(val);
        else:
            push_error(name, ": can not load model: \"", val, "\"");
            model = null;

@export var model_position: Vector3 = Vector3.ZERO;
@export var model_rotation: Vector3 = Vector3.ZERO;
@export var model_scale: Vector3 = Vector3.ONE;

var tile: Image;
var model: PackedScene;

func _init():
    pass
