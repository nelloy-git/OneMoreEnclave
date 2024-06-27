extends Node3D

class_name GridPosDetector;

@export_category("Main")
@export var enabled: bool = true;
@export var camera: Camera3D;
@export var collision_mask: int = randi();
@export var ray_distance: float = 1000;

@export_category("Draw grid")
@export var draw_grid_enabled: bool = true;
@export var draw_grid_size: Vector2 = Vector2(3, 3):
    set(val):
        draw_grid_size = val;
        self._updareGridMesh();
        
@export var draw_grid_color: Color = Color.WHITE_SMOKE:
    set(val):
        draw_grid_color = val;
        self._updareGridMesh();

var collider: StaticBody3D;
var grid: MeshInstance3D;

class GridPoint:
    var pos: Vector2i;
    
var last_mouse_pos: Vector2i;
var last_mouse_grid: GridPoint;

func getMousePos() -> GridPoint:
    if (not enabled):
        push_warning("Called getMousePos() while disabled")
        return null;
    return last_mouse_grid;

func getGlobalPos(grid_pos: Vector2i):
    return Vector3(grid_pos.x, 0, grid_pos.y) * transform;

func _ready():
    collider = get_node("Collider");
    collider.set_collision_layer(collision_mask);
    _updareGridMesh();

func _input(event):
    if (not enabled):
        return ;
    
    if (event is InputEventMouseMotion):
        last_mouse_pos = event.position;
        return ;
    elif (event is InputEventMouseButton) and last_mouse_grid != null:
        print("GridPos: ", last_mouse_grid.pos);
        return ;

func _process(_delta):
    if (not enabled):
        return ;
    _drawGrid();
                        
func _physics_process(_delta):
    last_mouse_grid = _calculateMousePos();
    
func _calculateMousePos() -> GridPoint:
    var mouse_pos = get_viewport().get_mouse_position();
    var mouse_vec_origin = camera.project_ray_origin(mouse_pos);
    var mouse_vec_normal = camera.project_ray_normal(mouse_pos);
    
    var spaceState = get_world_3d().direct_space_state;
    var intersect = spaceState.intersect_ray(
        PhysicsRayQueryParameters3D.create(
            mouse_vec_origin,
            mouse_vec_origin + mouse_vec_normal.normalized() * ray_distance,
            collision_mask
        )
    );
    
    if intersect.is_empty() or not intersect.has("position"):
        return null;
        
    var point_transformed = transform.orthonormalized().inverse() * intersect.position;
    
    var result = GridPoint.new();
    result.pos = Vector2(
        round(point_transformed.x / scale.x - 0.5),
        round(point_transformed.z / scale.z - 0.5)
    );
    return result;
    
func _drawGrid():
    if (not draw_grid_enabled):
        return ;
    
    if (last_mouse_grid == null):
        grid.hide();
        return ;
                
    grid.position = Vector3(
        last_mouse_grid.pos.x,
        0,
        last_mouse_grid.pos.y
    );
    grid.show();

func _updareGridMesh() -> void:
    var current = grid;
    grid = _createGridMesh();
    add_child(grid);
    if (current != null):
        grid.visible = current.visible;
        current.free();
    else:
        grid.hide();

func _createGridMesh() -> MeshInstance3D:
    var grid_inst: MeshInstance3D = MeshInstance3D.new();
    var grid_mesh: ImmediateMesh = ImmediateMesh.new();
    grid_inst.mesh = grid_mesh;
    grid_inst.cast_shadow = GeometryInstance3D.SHADOW_CASTING_SETTING_OFF
    
    var len_x: float = draw_grid_size.x;
    var len_y: float = draw_grid_size.y;
    var n_x: int = int((len_x + 1) / 2);
    var n_y: int = int((len_y + 1) / 2);
    
    for i in range( - n_x + 1, n_x + 1):
        Primitive3D.addLine(
            grid_mesh,
            Vector3(i, 0, -len_y / 2 + 0.5),
            Vector3(i, 0, len_y / 2 + 0.5),
            draw_grid_color
        );
    for i in range( - n_y + 1, n_y + 1):
        Primitive3D.addLine(
            grid_mesh,
            Vector3( - len_x / 2 + 0.5, 0, i),
            Vector3(len_x / 2 + 0.5, 0, i),
            draw_grid_color
        );

    return grid_inst;
