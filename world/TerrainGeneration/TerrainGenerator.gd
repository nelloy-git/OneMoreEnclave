@tool

class_name TerrainGenerator;
extends Node3D;

@export var generate_btn: bool = false:
	get:
		return false;
	set(val):
		if (Engine.is_editor_hint()):
			createRootNode();
		else:
			push_error("generate_btn can not be used in runtime");

@export var height_noise: FastNoiseLite = FastNoiseLite.new()
@export var cell_size: Vector2 = Vector2.ONE;
@export var size_x: int = 64;
@export var size_y: int = 64;
@export var min_height: float = -5;
@export var max_height: float = 10;

@export var test_csharp: Resource = null;

var _height_map: TerrainGeneration_HeightMap;
var _landscape: TerrainGenerator_Landscape;

var _root: Node = null;
var _model: MeshInstance3D = null;
var _collider: StaticBody3D = null;
var _collider_shape: CollisionShape3D = null;

func _ready():
	print(test_csharp.to_string());

func createRootNode():
	if (_root != null):
		_root.free();

	_height_map = TerrainGeneration_HeightMap.new(
		size_x + 1, size_y + 1,
		min_height, max_height,
		TerrainGeneration_HeightMap.GradientFunc.SquaredHeight,
		height_noise);
	_landscape = TerrainGenerator_Landscape.new(_height_map);

	_root = TerrainGenerator._addChild(self, Node.new());
	_root.name = "Terrain";
	_createModel();
	_createCollider();

func _createModel():
	if (_model != null):
		_model.free();

	_model = TerrainGenerator._addChild(_root, MeshInstance3D.new());
	_model.name = "Model"
	_model.mesh = _landscape.getMesh();

func _createCollider():
	if (_collider != null):
		_collider.free();

	_collider = TerrainGenerator._addChild(_root, StaticBody3D.new());
	_collider.name = "Collider";
	
	_collider_shape = TerrainGenerator._addChild(_collider, CollisionShape3D.new());
	_collider_shape.name = "ColliderShape";
	_collider_shape.shape = _landscape.getMesh().create_trimesh_shape();

static func _addChild(parent, child):
	parent.add_child(child);
	child.set_owner(parent.get_tree().get_edited_scene_root());
	return child;
