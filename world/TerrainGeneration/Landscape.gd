class_name TerrainGenerator_Landscape
extends RefCounted

var height_map: TerrainGeneration_HeightMap;

var _surf_tool: SurfaceTool = null;
var _cells: Vector2i;
var _mesh: ArrayMesh = null;

func getMesh() -> ArrayMesh:
    return _mesh;

func _init(_height_map: TerrainGeneration_HeightMap):
    height_map = _height_map;
    _cells = Vector2i(height_map.width - 1, height_map.height - 1);

    _surf_tool = SurfaceTool.new();
    _surf_tool.begin(Mesh.PRIMITIVE_TRIANGLES);

    _fillVertixes();
    _fillIndixes();

    _surf_tool.generate_normals();
    _surf_tool.generate_tangents();
    _mesh = _surf_tool.commit();

func _fillVertixes():
    for x in range(0, height_map.width):
        for y in range(0, height_map.height):
            _fillVert(x, y)

    # count % 3 == 0 is required for SurfaceTool
    var vert_count_div_3 = height_map.width * height_map.height;
    if (vert_count_div_3 != 0):
        for i in range(0, 3 - vert_count_div_3):
            _surf_tool.add_vertex(Vector3(0, 0, 0));

func _fillVert(x: int, y: int):
    _surf_tool.set_uv(
        Vector2(0, 0)
    );
    _surf_tool.add_vertex(
        Vector3(
            x - float(_cells.x) / 2,
            height_map.at(x, y),
            y - float(_cells.y) / 2
        )
    );

func _fillIndixes():
    for x in range(0, height_map.width - 1):
        for y in range(0, height_map.height - 1):
            _addCell(x, y);
    pass

func _addCell(x: int, y: int):
    if ((x >= height_map.width) or (y >= height_map.height)):
        push_error("Out of range");
        return

    var use: Array[bool] = [];
    use.resize(4);
    use[0] = height_map.at(x, y) >= 0;
    use[1] = height_map.at(x + 1, y) >= 0;
    use[2] = height_map.at(x, y + 1) >= 0;
    use[3] = height_map.at(x + 1, y + 1) >= 0;

#   0 --- 1
#   |   / |
#   | /   |
#   2 --- 3
    if (use[1] and use[2]):
        if (use[0]):
            _surf_tool.add_index(_getVertexIndex(x, y));
            _surf_tool.add_index(_getVertexIndex(x + 1, y));
            _surf_tool.add_index(_getVertexIndex(x, y + 1));
        
        if (use[3]):
            _surf_tool.add_index(_getVertexIndex(x + 1, y));
            _surf_tool.add_index(_getVertexIndex(x + 1, y + 1));
            _surf_tool.add_index(_getVertexIndex(x, y + 1));

        return

#   0 --- 1
#   | \   |
#   |   \ |
#   2 --- 3
    if (use[0] and use[3]):
        if (use[1]):
            _surf_tool.add_index(_getVertexIndex(x, y));
            _surf_tool.add_index(_getVertexIndex(x + 1, y));
            _surf_tool.add_index(_getVertexIndex(x + 1, y + 1));
        
        if (use[2]):
            _surf_tool.add_index(_getVertexIndex(x, y));
            _surf_tool.add_index(_getVertexIndex(x + 1, y + 1));
            _surf_tool.add_index(_getVertexIndex(x, y + 1));
        
        return

func _getVertexIndex(x: int, y: int):
    return y + x * height_map.width;